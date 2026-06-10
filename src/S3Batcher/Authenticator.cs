using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;

namespace S3Batcher
{
    public sealed class Authenticator
    {
        public AmazonS3Client Authenticate(string accessKey, string secretKey, string regionName)
        {
            var region = RegionEndpoint.GetBySystemName(regionName);
            return new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), region);
        }
    }
}