using System;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;
using S3Batcher.Logging;
using S3Batcher.Objects;

namespace S3Batcher.Operations
{
    public sealed class RestoreObjects : IOperation
    {
        private IAmazonS3 _s3Client;
        private Logger _logger;

        public RestoreObjects(IAmazonS3 s3Client)
        {
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
            _logger = new Logger(typeof(DeleteVersions));
        }

        public void Execute(OperationOptions options)
        {
            _logger.Info($"Starting objects restore {(options.DryRun ? "(dry run)" : string.Empty)}...");
            var listRequest = new ListVersionsRequest
            {
                BucketName = options.BucketName,
                Prefix = options.Prefix,
                MaxKeys = options.MaxKeys
            };

            var versions = new S3ObjectVersionsEnumerable(_s3Client, listRequest);

            var deleteRequest = new DeleteObjectsRequest
            {
                BucketName = listRequest.BucketName,
                Objects = versions.Where(_ => _.IsDeleteMarker).Select(dm =>
                {
                    _logger.Info($"Adding to restore chunk: {dm.Key} - [{dm.VersionId}]");
                    return new KeyVersion { Key = dm.Key, VersionId = dm.VersionId };
                }).ToList()
            };

            if (!options.DryRun)
            {
                _s3Client.DeleteObjectsAsync(deleteRequest).Wait();
            }

            _logger.Info("Restoring completed!");
        }
    }
}