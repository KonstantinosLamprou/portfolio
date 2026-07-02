namespace Backend.Application.Common.Interfaces;

public interface IStorageService
{
    /// <summary>
    /// Speichert eine Datei und gibt den eindeutigen generierten Dateinamen zurück.
    /// </summary>
    Task<string> UploadFileAsync(Stream fileStream, string originalFileName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Löscht eine Datei basierend auf dem Dateinamen.
    /// </summary>
    bool DeleteFile(string fileName);
}