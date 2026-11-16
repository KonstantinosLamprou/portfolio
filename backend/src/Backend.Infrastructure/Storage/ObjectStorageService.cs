using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Backend.Application.Common.Interfaces; 
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Backend.Infrastructure.Storage;

public class ObjectStorageService : IObjectStorageService
{
    private readonly IAmazonS3 _s3;
    private static readonly ConcurrentDictionary<string, bool> _knownBuckets = new();
    public ObjectStorageService(IAmazonS3 s3) => _s3 = s3;

    public async Task<string> UploadImageAsync(string bucket, Stream stream, string fileName, string contentType, CancellationToken ct = default)
    {
        await EnsureBucketAsync(bucket);
        
        var key = $"{Guid.NewGuid()}_{fileName.Replace(" ", "-")}";
        await _s3.PutObjectAsync(new PutObjectRequest
        {
            BucketName = bucket,
            Key = key,
            InputStream = stream,
            ContentType = contentType,
            AutoCloseStream = false
        }, ct);

        return key;
    }

    public async Task GetFileAsync(string bucket, string objectName, Stream output, CancellationToken ct = default)
    {
        using var response = await _s3.GetObjectAsync(bucket, objectName, ct);
        await response.ResponseStream.CopyToAsync(output, ct);
    }

    public async Task<bool> DeleteFileAsync(string bucket, string objectName, CancellationToken ct = default)
    {
        try
        {
            await _s3.DeleteObjectAsync(bucket, objectName, ct);
            return true;
        }
        catch (AmazonS3Exception)
        {
            return false;
        }
    }

    private async Task EnsureBucketAsync(string bucket)
    {
        if (_knownBuckets.ContainsKey(bucket)) return;
        if (!await AmazonS3Util.DoesS3BucketExistV2Async(_s3, bucket))
            await _s3.PutBucketAsync(bucket);
        _knownBuckets[bucket] = true;
    }
}


