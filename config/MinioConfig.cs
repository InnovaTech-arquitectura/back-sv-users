using Microsoft.Extensions.Configuration;
using Minio;

namespace YourNamespace.Config
{
    public class MinioConfig
    {
        private readonly IConfiguration _configuration;
        private readonly string? _accessKey;
        private readonly string? _secretKey;
        private readonly string? _minioUrl;

        public MinioConfig(IConfiguration configuration)
        {
            _configuration = configuration;
            _accessKey = _configuration["Minio:AccessKey"];
            _secretKey = _configuration["Minio:SecretKey"];
            _minioUrl = _configuration["Minio:Url"];
        }

        // Cambia el tipo de retorno a IMinioClient
        public IMinioClient CreateMinioClient()
        {
            return new MinioClient()
                .WithCredentials(_accessKey, _secretKey)
                .WithEndpoint(_minioUrl)
                .Build();
        }
    }
}
