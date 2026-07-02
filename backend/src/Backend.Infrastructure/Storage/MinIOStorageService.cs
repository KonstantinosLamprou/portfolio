using Backend.Application.Common.Interfaces;
using Minio;
using Minio.DataModel.Args;


namespace Backend.Infrastructure.Storage;

public class MinIOStorageService : IMinIOService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName = "portfolio-images"; 

    public MinIOStorageService(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }   

    public async Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {

        // Überprüfen, ob der Bucket existiert, und erstellen, falls nicht
        bool bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName), cancellationToken);
        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName), cancellationToken);
        }

        string uniqueFileName = $"{Guid.NewGuid()}_{fileName.Replace(" ", "-")}";

        // Datei hochladen
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(uniqueFileName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType("image/jpeg");

            
        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken: cancellationToken);

        return uniqueFileName; // Rückgabe des eindeutigen Dateinamens
    }

    public async Task GetFileAsync(string objectName, Stream outputStream, CancellationToken cancellationToken = default)
    {
        try
        {
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithCallbackStream(stream => stream.CopyToAsync(outputStream, cancellationToken));

            await _minioClient.GetObjectAsync(getObjectArgs, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Fehler beim Abrufen der Datei {objectName} aus MinIO: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteFileAsync(string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            return true;
        }
        catch (Exception)
        {
            // Wenn das Bild nicht existiert oder MinIO offline ist
            // Hier später ILogger<MinIOStorageService> einbauen!
            return false;
        }
    }


}