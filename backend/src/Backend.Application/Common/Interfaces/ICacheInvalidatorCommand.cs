namespace Backend.Application.Common.Interfaces;

public interface ICacheInvalidatorCommand
{
    // Eine Liste von Namen (Keys), die jetzt veraltet sind und gelöscht werden müssen
    IEnumerable<string> CacheKeysToInvalidate { get; } 
}