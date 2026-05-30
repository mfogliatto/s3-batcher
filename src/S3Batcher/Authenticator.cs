using System;
using System.Linq;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;

namespace S3Batcher
{
    public sealed class Authenticator
    {
        /// <summary>
        /// Authenticate with explicit credentials.
        /// </summary>
        public AmazonS3Client Authenticate(string accessKey, string secretKey, string regionName)
        {
            var region = RegionEndpoint.EnumerableAllRegions.First(_ => _.SystemName == regionName);
            return new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), region);
        }

        /// <summary>
        /// Authenticate using the AWS credential chain:
        /// 1. Explicit credentials (if provided)
        /// 2. Named profile from ~/.aws/credentials
        /// 3. Environment variables (AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY)
        /// 4. SDK default chain (IAM roles, instance profiles, etc.)
        /// </summary>
        public AmazonS3Client Authenticate(string accessKey, string secretKey, string regionName, string profile)
        {
            var region = RegionEndpoint.EnumerableAllRegions.First(_ => _.SystemName == regionName);
            var credentials = ResolveCredentials(accessKey, secretKey, profile);
            return new AmazonS3Client(credentials, region);
        }

        private AWSCredentials ResolveCredentials(string accessKey, string secretKey, string profile)
        {
            // 1. Explicit CLI arguments take precedence
            if (!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretKey))
            {
                return new BasicAWSCredentials(accessKey, secretKey);
            }

            // 2. AWS_PROFILE / --profile from shared credentials file
            var profileName = profile
                ?? Environment.GetEnvironmentVariable("AWS_PROFILE")
                ?? "default";

            var credFile = new SharedCredentialsFile();
            if (credFile.TryGetProfile(profileName, out var credProfile) &&
                AWSCredentialsFactory.TryGetAWSCredentials(credProfile, credFile, out var profileCreds))
            {
                return profileCreds;
            }

            // 3. Environment variables
            var envAccess = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var envSecret = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            if (!string.IsNullOrEmpty(envAccess) && !string.IsNullOrEmpty(envSecret))
            {
                return new BasicAWSCredentials(envAccess, envSecret);
            }

            // 4. SDK default chain (IAM roles, instance profile, etc.)
            return FallbackCredentialsFactory.GetCredentials();
        }
    }
}
