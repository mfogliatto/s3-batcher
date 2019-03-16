using System;
using System.Linq;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;

namespace S3Batcher
{
    public sealed class Authenticator
    {
        public AmazonS3Client Authenticate(string accessKey, string secretKey, string regionName)
        {
            var region = RegionEndpoint.EnumerableAllRegions.First(_ => _.SystemName == regionName);
            return new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), region);
        }
    }
}