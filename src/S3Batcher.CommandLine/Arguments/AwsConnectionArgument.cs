using System.Linq;
using Amazon.Runtime;

namespace S3Batcher.CommandLine.Arguments
{
    sealed class AwsConnectionArgument
    {
        public string AccessKey { get; }
        public string SecretKey { get; }
        public string Region { get; }
        public string Profile { get; }

        public AwsConnectionArgument(string accessKey, string secretKey, string region, string profile)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            Region = region;
            Profile = profile;
        }

        public bool HasExplicitCredentials => !string.IsNullOrEmpty(AccessKey) && !string.IsNullOrEmpty(SecretKey);
    }
}
