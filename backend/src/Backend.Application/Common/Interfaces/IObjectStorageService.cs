namespace Backend.Application.Common.Interfaces;

public interface IObjectStorageService
{
    Task<string> UploadImageAsync(string bucket, Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);

    Task<bool> DeleteFileAsync(string bucket, string objectName, CancellationToken cancellationToken = default);

    Task GetFileAsync(string bucket, string objectName, Stream outputStream, CancellationToken cancellationToken = default);
}

