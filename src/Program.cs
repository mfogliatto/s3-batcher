using System;
using System.Linq;
using Amazon.S3;
using S3Batcher.Arguments;
using S3Batcher.Operations;

namespace S3Batcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var parsedArgs = args.Select(_ => new Argument(_)).ToList();

            {
                var authParser = new AwsConnectionParser();
                var connArg = authParser.GetFrom(parsedArgs);

                var client = new Authenticator().Authenticate(connArg.AccessKey, connArg.SecretKey, connArg.Region);
                Container.Register(typeof(AmazonS3Client), client);
            }

            {
                var opParser = new OperationParser();
                var opArgument = opParser.GetFrom(parsedArgs);
                var opInstance = (IOperation)Container.Resolve(opArgument.Type);

                opInstance.Execute(opArgument.Options);
            }
        }
    }
}
