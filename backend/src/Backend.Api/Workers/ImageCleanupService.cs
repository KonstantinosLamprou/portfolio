using Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Presentation.Workers; 

public class ImageCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ImageCleanupService> _logger;

    public ImageCleanupService(
        IServiceProvider serviceProvider, 
        IWebHostEnvironment env, 
        ILogger<ImageCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _env = env;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Image Cleanup Worker gestartet.");

        // Diese While-Schleife läuft, solange das Backend gestartet ist
        while (!stoppingToken.IsCancellationRequested)
        {
            await CleanupOrphanedImagesAsync();

            // Warte 24 Stunden, bevor die Schleife das nächste Mal läuft.
            // TIPP FÜRS TESTEN: Ändere dies temporär zu TimeSpan.FromMinutes(1)
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task CleanupOrphanedImagesAsync()
    {
        try
        {
            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var uploadsFolder = Path.Combine(webRoot, "uploads", "images");

            if (!Directory.Exists(uploadsFolder)) return;

            // Alle physischen Dateien (Bilder) auf der Festplatte finden
            var physicalFiles = Directory.GetFiles(uploadsFolder);

            // Den Entity Framework DbContext sicher über einen Scope öffnen
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); 

            // Alle aktuell in der DB genutzten Bild-URLs abfragen
            var postsWithImages = await context.Blogs
                .Where(p => !string.IsNullOrEmpty(p.ImgSrc))
                .Select(p => p.ImgSrc!)
                .ToListAsync();

            var projectsWithImages = await context.Projects
                .Where(p => !string.IsNullOrEmpty(p.ImgSrc))
                .Select(p => p.ImgSrc!)
                .ToListAsync();

            postsWithImages.AddRange(projectsWithImages);

            // URLs in reine Dateinamen umwandeln (z.B. "guid_bild.png")
            var activeImageNames = postsWithImages
                .Select(url => Path.GetFileName(new Uri(url).LocalPath))
                .ToHashSet(); 

            int deletedCount = 0;

            // Dateien überprüfen und löschen
            foreach (var filePath in physicalFiles)
            {
                var fileName = Path.GetFileName(filePath);
                var fileInfo = new FileInfo(filePath);

                // LOGIK: Ist der Name NICHT in der DB? 
                // UND ist die Datei älter als 24 Stunden? (Die 24-Stunden "Schonfrist" verhindert, 
                // dass wir ein Bild löschen, das ein User in genau dieser Sekunde hochlädt!)
                if (!activeImageNames.Contains(fileName) && 
                    fileInfo.CreationTimeUtc < DateTime.UtcNow.AddHours(-24))
                {
                    File.Delete(filePath);
                    deletedCount++;
                    _logger.LogInformation("Dateileiche gelöscht: {FileName}", fileName);
                }
            }

            if (deletedCount > 0)
            {
                _logger.LogInformation("Cleanup-Durchlauf beendet. {Count} Bilder gelöscht.", deletedCount);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler während des Background-Cleanups.");
        }
    }
}