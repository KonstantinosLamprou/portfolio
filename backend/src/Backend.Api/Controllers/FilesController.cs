using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    // IWebHostEnvironment gibt uns den Pfad zum wwwroot-Ordner des Servers
    public FilesController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost("upload")]
    [Authorize] 
    public async Task<IActionResult> UploadImage(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Backend sagt: Keine Datei hochgeladen oder Datei ist leer.");

        if (!file.ContentType.StartsWith("image/"))
            return BadRequest($"Backend sagt: Nur Bilder sind erlaubt. Erhalten: {file.ContentType}");

        // Zielordner erstellen (wwwroot/uploads/images)
        var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
        var uploadsFolder = Path.Combine(webRoot, "uploads", "images");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        // Einen sicheren, einmaligen Dateinamen generieren
        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Datei physisch auf die Festplatte kopieren
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Die fertige URL generieren, die das Frontend später nutzen kann
        var request = HttpContext.Request;
        var fileUrl = $"{request.Scheme}://{request.Host}/uploads/images/{uniqueFileName}";

        // URL an Vue zurückgeben!
        return Ok(new { url = fileUrl });
    }
    [HttpDelete("delete")]
    [Authorize]
    public IActionResult DeleteImage([FromQuery] string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return BadRequest("Keine URL angegeben.");

        try
        {
            // reinen Dateinamen aus der URL (z.B. "guid_bild.png")
            var uri = new Uri(fileUrl);
            var fileName = Path.GetFileName(uri.LocalPath);

            // Physischer Pfad
            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var filePath = Path.Combine(webRoot, "uploads", "images", fileName);

            // Sicherheitscheck: Verhindern von Directory Traversal Attacks ("../")
            var uploadsFolder = Path.Combine(webRoot, "uploads", "images");
            if (!filePath.StartsWith(uploadsFolder))
                return Forbid("Ungültiger Dateipfad.");

            // Datei löschen
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return Ok(new { message = "Bild erfolgreich gelöscht." });
            }

            return NotFound("Bild auf dem Server nicht gefunden.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interner Fehler beim Löschen: {ex.Message}");
        }
    }
}