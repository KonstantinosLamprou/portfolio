using Backend.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Common.Decorators;

public class ValidationDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _innerHandler;
    private readonly IEnumerable<IValidator<TCommand>> _validators;
    private readonly ILogger<ValidationDecorator<TCommand, TResult>> _logger;

    public ValidationDecorator(
        ICommandHandler<TCommand, TResult> innerHandler, 
        IEnumerable<IValidator<TCommand>> validators,
        ILogger<ValidationDecorator<TCommand, TResult>> logger)
    {
        _innerHandler = innerHandler;
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        // Kleine Optimierung: Wenn es gar keine Validatoren für dieses Command gibt (z.B. bei ganz simplen Commands),
        // können wir uns den ganzen Check sparen und direkt weitergehen.
        if (!_validators.Any())
        {
            return await _innerHandler.HandleAsync(command, cancellationToken);
        }

        var context = new ValidationContext<TCommand>(command);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        // Wenn Fehler existieren, wird geloggt und der Handler abgebrochen!
        if (failures.Count > 0)
        {
            var commandName = typeof(TCommand).Name;
            var errorMessages = string.Join(" | ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
            
            // LogWarning ist hier perfekt. Es ist kein Server-Absturz (Error), sondern der User hat falsche Daten geschickt.
            _logger.LogWarning("Validierung fehlgeschlagen für {CommandName}. Fehler: {ValidationErrors}", commandName, errorMessages);

            throw new ValidationException(failures);
        }

        // Alles ist gültig! Wir geben den Staffelstab an die nächste Schicht (den _innerHandler) weiter.
        return await _innerHandler.HandleAsync(command, cancellationToken);
    }
}