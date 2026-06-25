using Backend.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Common.Decorators;

public class CachingDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _innerHandler;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachingDecorator<TQuery, TResult>> _logger;

    public CachingDecorator(
        IQueryHandler<TQuery, TResult> innerHandler,
        IMemoryCache cache,
        ILogger<CachingDecorator<TQuery, TResult>> logger)
    {
        _innerHandler = innerHandler;
        _cache = cache; 
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        // Hat dieses Record überhaupt das Interface unterschrieben?
        // Wenn nicht, überspringen wir den Cache und rufen sofort den Handler auf.
        if (query is not ICacheQueryQuery cacheableQuery)
        {
            return await _innerHandler.HandleAsync(query, cancellationToken);
        }

        // Haben wir das schon im Cache?
        if (_cache.TryGetValue(cacheableQuery.CacheKey, out TResult? cachedResult) && cachedResult is not null)
        {
            _logger.LogInformation("Lade Ergebnis für {CacheKey} blitzschnell aus dem RAM-Cache.", cacheableQuery.CacheKey);
            return cachedResult;
        }

        // Wenn NICHT im Cache: Wir müssen den echten Handler (die Datenbank) anwerfen
        _logger.LogInformation("Cache Miss für {CacheKey}. Hole Daten aus der Datenbank.", cacheableQuery.CacheKey);
        var result = await _innerHandler.HandleAsync(query, cancellationToken);

        // Das frische Ergebnis für das nächste Mal im Cache speichern
        if (result is not null)
        {
            _cache.Set(cacheableQuery.CacheKey, result, cacheableQuery.Expiration);
        }

        return result;
    }
}