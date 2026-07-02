using Backend.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Backend.Infrastructure.Storage;

public class LocalStorageService : IStorageService
{
    private readonly string _uploadsFolder;

    public LocalStorageService(IWebHostEnvironment env)
    {
        // Pfad auflösen: wwwroot/uploads/images
        var webRoot = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
        _uploadsFolder = Path.Combine(webRoot, "uploads", "images");

        if (!Directory.Exists(_uploadsFolder))
        {
            Directory.CreateDirectory(_uploadsFolder);
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string originalFileName, CancellationToken cancellationToken = default)
    {
        var uniqueFileName = $"{Guid.NewGuid()}_{originalFileName}";
        var filePath = Path.Combine(_uploadsFolder, uniqueFileName);

        using (var targetStream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(targetStream, cancellationToken);
        }

        return uniqueFileName;
    }

    public bool DeleteFile(string fileName)
    {
        var filePath = Path.Combine(_uploadsFolder, fileName);

        // Sicherheitscheck gegen Directory Traversal (../)
        if (!filePath.StartsWith(_uploadsFolder))
        {
            throw new UnauthorizedAccessException("Ungültiger Dateipfad.");
        }

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }

        return false;
    }
}