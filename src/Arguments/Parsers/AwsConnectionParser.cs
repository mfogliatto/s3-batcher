using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime;

namespace S3Batcher.Arguments
{
    sealed class AwsConnectionParser : IParser<AwsConnectionArgument>
    {
        private const string ACCESS_KEY = "access-key";
        private const string SECRET_KEY = "secret-key";
        private const string REGION_KEY = "region";
        private string[] _requiredArgs = new string[] { ACCESS_KEY, SECRET_KEY, REGION_KEY };

        public AwsConnectionArgument GetFrom(IEnumerable<Argument> args)
        {
            var relevantArgs = new Dictionary<string, Argument>();
            foreach (var arg in _requiredArgs)
            {
                var found = args.FirstOrDefault(_ => _.Matches(arg));
                if (found == default(Argument))
                {
                    throw new ArgumentMissingException($"An --[{arg}] argument is required.");
                }

                relevantArgs.Add(found.Key, found);
            }

            var accessKey = relevantArgs[ACCESS_KEY].Value;
            var secretKey = relevantArgs[SECRET_KEY].Value;
            var region = relevantArgs[REGION_KEY].Value;
            return new AwsConnectionArgument(accessKey, secretKey, region);
        }
    }
}