using Backend.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IMinIOService _storageService;

    // Das Interface wird hier per Dependency Injection injiziert
    public FilesController(IMinIOService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UploadImage(IFormFile? file, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Keine Datei hochgeladen oder Datei ist leer.");

        if (!file.ContentType.StartsWith("image/"))
            return BadRequest($"Nur Bilder sind erlaubt. Erhalten: {file.ContentType}");

        // Öffne den Stream der hochgeladenen Datei
        using var stream = file.OpenReadStream();
        
        // Speicher-Logik an den Service delegieren
        var uniqueFileName = await _storageService.UploadImageAsync(stream, file.FileName, cancellationToken);

        // Web-Logik: Absolute URL für das Frontend bauen
        var request = HttpContext.Request;
        var fileUrl = $"{request.Scheme}://{request.Host}/api/files/{uniqueFileName}";

        return Ok(new { url = fileUrl });
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetImage(
        [FromRoute] string fileName, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
            return BadRequest("Dateiname fehlt.");

        Response.ContentType = "image/jpeg";
        await _storageService.GetFileAsync(fileName, Response.Body, cancellationToken);
        
        return new EmptyResult();
    }

    [HttpDelete("delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteImage([FromQuery] string fileUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return BadRequest("Keine URL angegeben.");

        try
        {
            // Reinen Dateinamen aus der URL extrahieren
            var uri = new Uri(fileUrl);
            var fileName = Path.GetFileName(uri.LocalPath);

            // Lösch-Logik an den Service delegieren
            var success = await _storageService.DeleteFileAsync(fileName, cancellationToken);

            if (success)
                return Ok(new { message = "Bild erfolgreich gelöscht." });

            return NotFound("Bild auf dem Storage nicht gefunden.");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid("Ungültiger Dateipfad.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interner Fehler beim Löschen: {ex.Message}");
        }
    }
}