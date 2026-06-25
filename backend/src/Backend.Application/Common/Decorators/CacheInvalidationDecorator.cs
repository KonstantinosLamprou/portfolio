using Backend.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Common.Decorators;

public class CacheInvalidationDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _innerHandler;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheInvalidationDecorator<TCommand, TResult>> _logger;

    public CacheInvalidationDecorator(
        ICommandHandler<TCommand, TResult> innerHandler,
        IMemoryCache cache,
        ILogger<CacheInvalidationDecorator<TCommand, TResult>> logger)
    {
        _innerHandler = innerHandler;
        _cache = cache; // Tausch gegen IDistributedCache von Redis (später)
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _innerHandler.HandleAsync(command, cancellationToken);

        // Wenn der Command das Interface ICacheInvalidatorCommand implementiert, dann invalidieren wir die angegebenen Cache Keys
        if (command is ICacheInvalidatorCommand invalidator)
        {
            // Wenn ja, alle angegebenen Keys aus dem RAM (später Redis) werfen
            foreach (var key in invalidator.CacheKeysToInvalidate)
            {
                _cache.Remove(key);
                _logger.LogInformation("Cache Invalidated: Key '{CacheKey}' wurde gelöscht.", key);
            }
        }

        return result;
    }
}