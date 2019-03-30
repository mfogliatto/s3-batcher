using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime;

namespace S3Batcher.CommandLine.Arguments
{
    sealed class AwsConnectionParser : IParser<AwsConnectionArgument>
    {
        private static ArgumentDefinition AccessArg = ArgumentDefinition.Required("access-key", "Your AWS Access Key ID.");
        private static ArgumentDefinition SecretArg = ArgumentDefinition.Required("secret-key", "Your AWS Secret Access Key.");
        private static ArgumentDefinition RegionArg = ArgumentDefinition.Required("region", "The region name where your S3 instance is (e.g.: us-west-1, us-west-2, etc.).");
        private static ArgumentDefinition[] _argDefinitions = new ArgumentDefinition[] { AccessArg, SecretArg, RegionArg };

        public static IEnumerable<ArgumentDefinition> GetArgumentDefinitions()
        {
            return _argDefinitions;
        }

        public AwsConnectionArgument GetFrom(IEnumerable<Argument> args)
        {
            var argValues = new Dictionary<string, string>();
            foreach (var arg in _argDefinitions)
            {
                var found = args.FirstOrDefault(_ => _.Matches(arg.Name));
                if (found == default(Argument))
                {
                    throw new ArgumentMissingException($"An --[{arg}] argument is required.");
                }

                argValues.Add(found.Name, found.Value);
            }

            var accessKey = argValues[AccessArg.Name];
            var secretKey = argValues[SecretArg.Name];
            var region = argValues[RegionArg.Name];
            return new AwsConnectionArgument(accessKey, secretKey, region);
        }
    }
}