using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartPoles.Domain.Interfaces;
using SmartPoles.Domain.Models;

namespace SmartPoles.Data.Repositories
{
    public class S3Repository : IStorageRepository
    {
        private readonly AmazonS3Config _config;
        private readonly AmazonS3Client _s3Client;
        private readonly ILogger<S3Repository> _logger;

        public S3Repository(IConfiguration configuration, ILogger<S3Repository> logger)
        {
            var accessKey = configuration["SmartPoleAccessKey"];
            var secretKey = configuration["SmartPoleSecretKey"];

            _logger = logger;
            _config = new AmazonS3Config();
            _s3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.USEast1);
        }
        public IEnumerable<Condominium> GetFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}