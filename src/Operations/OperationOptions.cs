using Amazon;

namespace S3Batcher.Operations
{
    class OperationOptions
    {
        public string BucketName { get; }
        public string Prefix { get; }
        public int MaxKeys { get; }

        public OperationOptions(string bucketName, string prefix)
        {
            BucketName = bucketName;
            Prefix = prefix;
            MaxKeys = 1000; // this is the default in S3 API.
        }
    }
}