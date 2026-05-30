using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime;

namespace S3Batcher.CommandLine.Arguments
{
    sealed class AwsConnectionParser : IParser<AwsConnectionArgument>
    {
        private static ArgumentDefinition AccessArg = ArgumentDefinition.Optional("access-key", "Your AWS Access Key ID. Falls back to AWS_ACCESS_KEY_ID env var or ~/.aws/credentials.", null);
        private static ArgumentDefinition SecretArg = ArgumentDefinition.Optional("secret-key", "Your AWS Secret Access Key. Falls back to AWS_SECRET_ACCESS_KEY env var or ~/.aws/credentials.", null);
        private static ArgumentDefinition RegionArg = ArgumentDefinition.Required("region", "The region name where your S3 instance is (e.g.: us-west-1, us-west-2, etc.).");
        private static ArgumentDefinition ProfileArg = ArgumentDefinition.Optional("profile", "AWS profile name from ~/.aws/credentials. Falls back to AWS_PROFILE env var.", null);
        private static ArgumentDefinition[] _usedArguments = new ArgumentDefinition[] { AccessArg, SecretArg, RegionArg, ProfileArg };

        public static IEnumerable<ArgumentDefinition> GetArgumentDefinitions()
        {
            return _usedArguments;
        }

        public AwsConnectionArgument GetFrom(IEnumerable<Argument> args)
        {
            var argValues = ArgumentDefinition.GetValues(_usedArguments, args);

            var accessKey = argValues[AccessArg.Name];
            var secretKey = argValues[SecretArg.Name];
            var region = argValues[RegionArg.Name];
            var profile = argValues[ProfileArg.Name];
            return new AwsConnectionArgument(accessKey, secretKey, region, profile);
        }
    }
}
