using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime;

namespace S3Batcher.Arguments
{
    sealed class AwsCredentialsProvider : IProvider<BasicAWSCredentials>
    {
        private const string ACCESS_KEY = "access-key";
        private const string SECRET_KEY = "secret-key";

        public BasicAWSCredentials GetFrom(IEnumerable<Argument> args)
        {
            var argsList = args.ToList();
            var accessKeyArg = args.FirstOrDefault(_ => _.Matches(ACCESS_KEY));
            var secretKeyArg = args.FirstOrDefault(_ => _.Matches(SECRET_KEY));

            if (accessKeyArg == default(Argument) || secretKeyArg == default(Argument))
            {
                throw new ArgumentMissingException($"Credentials missing. Forgot {ACCESS_KEY} or {SECRET_KEY}?");
            }

            return new BasicAWSCredentials(accessKeyArg.Value, secretKeyArg.Value);
        }
    }
}