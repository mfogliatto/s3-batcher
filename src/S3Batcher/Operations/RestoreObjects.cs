using System;
using System.Collections.Generic;
using Amazon.S3;
using Amazon.S3.Model;
using S3Batcher.Logging;
using S3Batcher.Objects;

namespace S3Batcher.Operations
{
    public sealed class RestoreObjects : IOperation
    {
        private const int BatchSize = 1000;
        private IAmazonS3 _s3Client;
        private Logger _logger;

        public RestoreObjects(IAmazonS3 s3Client)
        {
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
            _logger = new Logger(typeof(RestoreObjects));
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
            var batch = new List<KeyVersion>(BatchSize);

            foreach (var version in versions)
            {
                if (!version.IsDeleteMarker) continue;

                _logger.Info($"Adding to restore chunk: {version.Key} - [{version.VersionId}]");
                batch.Add(new KeyVersion { Key = version.Key, VersionId = version.VersionId });

                if (batch.Count >= BatchSize)
                {
                    FlushBatch(options, batch);
                }
            }

            FlushBatch(options, batch);

            _logger.Info("Restoring completed!");
        }

        private void FlushBatch(OperationOptions options, List<KeyVersion> batch)
        {
            if (batch.Count == 0) return;

            if (!options.DryRun)
            {
                _s3Client.DeleteObjectsAsync(new DeleteObjectsRequest
                {
                    BucketName = options.BucketName,
                    Objects = new List<KeyVersion>(batch)
                }).Wait();
            }

            batch.Clear();
        }
    }
}