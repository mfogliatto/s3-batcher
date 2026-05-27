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
    public sealed class DeleteVersions : IOperation
    {
        private const int BatchSize = 1000;
        private IAmazonS3 _s3Client;
        private Logger _logger;

        public DeleteVersions(IAmazonS3 s3Client)
        {
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
            _logger = new Logger(typeof(DeleteVersions));
        }

        public void Execute(OperationOptions options)
        {
            _logger.Info($"Starting versions deletion {(options.DryRun ? "(dry run)" : string.Empty)}...");
            var listRequest = new ListVersionsRequest
            {
                BucketName = options.BucketName,
                Prefix = options.Prefix,
                MaxKeys = options.MaxKeys
            };

            var versions = new S3ObjectVersionsEnumerable(_s3Client, listRequest);

            var batch = new List<KeyVersion>(BatchSize);
            foreach (var version in versions)
            {
                _logger.Info($"Adding to delete chunk: {version.Key} - [{version.VersionId}]");
                batch.Add(new KeyVersion { Key = version.Key, VersionId = version.VersionId });

                if (batch.Count >= BatchSize)
                {
                    DeleteBatch(options, batch);
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
            {
                DeleteBatch(options, batch);
            }

            _logger.Info("Deletion completed!");
        }

        private void DeleteBatch(OperationOptions options, List<KeyVersion> objects)
        {
            var deleteRequest = new DeleteObjectsRequest
            {
                BucketName = options.BucketName,
                Objects = new List<KeyVersion>(objects)
            };

            if (!options.DryRun)
            {
                _s3Client.DeleteObjectsAsync(deleteRequest).Wait();
            }
        }
    }
}
