using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using S3Batcher.Logging;
using S3Batcher.Objects;

namespace S3Batcher.Operations
{
    class DeleteVersions : IOperation
    {
        private IAmazonS3 _s3Client;
        private Logger _logger;

        public DeleteVersions(IAmazonS3 s3Client)
        {
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
            _logger = new Logger(typeof(DeleteVersions));
        }

        public void Execute(OperationOptions options)
        {
            _logger.Info($"Starting versions deletion...");
            var listRequest = new ListVersionsRequest
            {
                BucketName = options.BucketName,
                Prefix = options.Prefix,
                MaxKeys = int.MaxValue
            };

            var versions = new S3ObjectVersionsEnumerable(_s3Client, listRequest);

            var deleteRequest = new DeleteObjectsRequest
            {
                BucketName = listRequest.BucketName,
                Objects = versions.Select(dm =>
                {
                    _logger.Info($"Adding to delete chunk: {dm.Key} - [{dm.VersionId}]");
                    return new KeyVersion { Key = dm.Key, VersionId = dm.VersionId };
                }).ToList()
            };

            _s3Client.DeleteObjectsAsync(deleteRequest).Wait();
            _logger.Info("Deletion completed!");
        }
    }
}