namespace Backend.Application.Common.Interfaces;

public interface IMinIOService
{
    /// <summary>
    /// Lädt ein Bild in den MinIO-Bucket hoch und gibt den eindeutigen generierten Dateinamen zurück.
    /// </summary>
    Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
    /// <summary>
    /// Löscht eine Datei basierend auf dem Dateinamen.
    /// </summary>
    Task<bool> DeleteFileAsync(string objectName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Holt eine Datei aus MinIO und schreibt sie direkt in den übergebenen Stream (z.B. Response.Body).
    /// </summary>
    Task GetFileAsync(string objectName, Stream outputStream, CancellationToken cancellationToken = default);
}