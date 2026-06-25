using System.Diagnostics;
using Backend.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Common.Decorators;

public class CommandPerformanceLoggingDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _innerHandler;
    private readonly ILogger<CommandPerformanceLoggingDecorator<TCommand, TResult>> _logger;

    public CommandPerformanceLoggingDecorator(
        ICommandHandler<TCommand, TResult> innerHandler, 
        ILogger<CommandPerformanceLoggingDecorator<TCommand, TResult>> logger)
    {
        _innerHandler = innerHandler;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var commandName = typeof(TCommand).Name;
        var stopwatch = Stopwatch.StartNew();

        // Den echten Handler (oder den nächsten Stecker) aufrufen
        var result = await _innerHandler.HandleAsync(command, cancellationToken);

        stopwatch.Stop();
        
        if (stopwatch.ElapsedMilliseconds > 500)
        {
            _logger.LogWarning("Performance-Warnung: {CommandName} dauerte {ElapsedMilliseconds} ms", commandName, stopwatch.ElapsedMilliseconds);
        }
        else
        {
            _logger.LogInformation("{CommandName} erfolgreich nach {ElapsedMilliseconds} ms", commandName, stopwatch.ElapsedMilliseconds);
        }

        return result;
    }
}