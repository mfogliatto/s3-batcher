using System.Linq;
using Amazon.Runtime;

namespace S3Batcher.Arguments
{
    class AwsConnectionArgument
    {
        public string AccessKey { get; }
        public string SecretKey { get; }
        public string Region { get; }

        public AwsConnectionArgument(string accessKey, string secretKey, string region)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            Region = region;
        }
    }
}