using System;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;
using S3Batcher.Logging;

namespace S3Batcher.Operations
{
    class RestoreObjects : IOperation
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
            _logger.Info($"Starting objects restore...");
            var listRequest = new ListVersionsRequest
            {
                BucketName = options.BucketName,
                Prefix = options.Prefix,
                MaxKeys = int.MaxValue
            };

            var cont = true;
            while (cont)
            {
                var r = _s3Client.ListVersionsAsync(listRequest).Result;

                var deleteRequest = new DeleteObjectsRequest
                {
                    BucketName = listRequest.BucketName,
                    Objects = r.Versions.Where(_ => _.IsDeleteMarker).Select(dm =>
                    {
                        _logger.Info($"Adding to restore chunk: {dm.Key} - [{dm.VersionId}]");
                        return new KeyVersion { Key = dm.Key, VersionId = dm.VersionId };
                    }).ToList()
                };

                _logger.Info("Restoring chunk...");
                _s3Client.DeleteObjectsAsync(deleteRequest).Wait();

                listRequest.KeyMarker = r.NextKeyMarker;
                cont = !string.IsNullOrWhiteSpace(listRequest.KeyMarker);
            }
            _logger.Info("Restoring completed!");
        }
    }
}