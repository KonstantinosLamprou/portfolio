using System.Diagnostics;
using Backend.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Common.Decorators;

public class QueryPerformanceLoggingDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _innerHandler;
    private readonly ILogger<QueryPerformanceLoggingDecorator<TQuery, TResult>> _logger;

    public QueryPerformanceLoggingDecorator(
        IQueryHandler<TQuery, TResult> innerHandler, 
        ILogger<QueryPerformanceLoggingDecorator<TQuery, TResult>> logger)
    {
        _innerHandler = innerHandler; // Das ist der echte Handler, der eingepackt wird
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        var queryName = typeof(TQuery).Name;
        var stopwatch = Stopwatch.StartNew();

        // Hier rufen wir den echten Handler auf!
        var result = await _innerHandler.HandleAsync(query, cancellationToken);

        stopwatch.Stop();
        
        // Dank Serilog und Seq können wir diese Info jetzt wunderschön strukturiert loggen
        if (stopwatch.ElapsedMilliseconds > 500)
        {
            _logger.LogWarning("Performance-Warnung: {QueryName} dauerte {ElapsedMilliseconds} ms", queryName, stopwatch.ElapsedMilliseconds);
        }
        else
        {
            _logger.LogInformation("{QueryName} erfolgreich nach {ElapsedMilliseconds} ms", queryName, stopwatch.ElapsedMilliseconds);
        }

        return result;
    }
}