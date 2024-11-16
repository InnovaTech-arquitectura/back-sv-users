using Minio;
using Minio.Exceptions;
using Minio.DataModel.Args;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace YourNamespace.Services
{
    public class MinioService
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;

        public MinioService(IConfiguration configuration, IMinioClient minioClient)
        {
            _minioClient = minioClient;
            _bucketName = configuration["Minio:BucketName"] ?? throw new ArgumentNullException("Minio:BucketName configuration is missing.");
        }

        // Método para crear el bucket si no existe
        private async Task CreateBucketAsync()
        {
            bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!found)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
            }
        }

    public async Task<Stream> GetObjectAsync(string filename)
{
    await CreateBucketAsync();
    MemoryStream memoryStream = new MemoryStream();
    try
    {
        // Verificar si el objeto existe
        await _minioClient.StatObjectAsync(new StatObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(filename));

        // Descargar el objeto
        await _minioClient.GetObjectAsync(new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(filename)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memoryStream);
            }));

        memoryStream.Seek(0, SeekOrigin.Begin);
    }
    catch (Minio.Exceptions.ObjectNotFoundException)
    {
        Console.WriteLine($"[MinIO] Object {filename} not found.");
        throw;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[MinIO] Error: {ex.Message}");
        throw;
    }

    return memoryStream;
}



        // Método para subir un archivo a MinIO
        public async Task UploadFileAsync(string filename, IFormFile file)
        {
            await CreateBucketAsync();
            try
            {
                await using var stream = file.OpenReadStream();
                await _minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(filename)
                    .WithStreamData(stream)
                    .WithObjectSize(file.Length)
                    .WithContentType(file.ContentType));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MinIO] Error uploading file: {ex.Message}");
                throw;
            }
        }

        // Método para eliminar un archivo de MinIO
        public async Task DeleteFileAsync(string filename)
        {
            await CreateBucketAsync();
            try
            {
                await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(filename));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MinIO] Error deleting file: {ex.Message}");
                throw;
            }
        }
    }
}
