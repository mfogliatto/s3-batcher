using Amazon;

namespace S3Batcher.Operations
{
    public sealed class OperationOptions
    {
        public string BucketName { get; }
        public string Prefix { get; }
        public int MaxKeys { get; }
        public bool DryRun { get; }

        public OperationOptions(string bucketName, string prefix, bool dryRun)
        {
            BucketName = bucketName;
            Prefix = prefix;
            DryRun = dryRun;
            MaxKeys = 1000; // this is the default in S3 API.
        }
    }
}