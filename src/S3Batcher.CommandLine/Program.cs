using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.S3;
using S3Batcher.CommandLine.Arguments;
using S3Batcher.Operations;

namespace S3Batcher.CommandLine
{
    static class Program
    {
        static void Main(string[] args)
        {
            {
                var showHelp = !args.Any() || args.Contains("--help");
                if (showHelp)
                {
                    ShowHelp();
                    return;
                }
            }

            var parsedArgs = args.Select(_ => new Argument(_)).ToList();

            try
            {
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ShowHelp()
        {
            var arguments = Enumerable.Concat(
                AwsConnectionParser.GetArgumentDefinitions(),
                OperationParser.GetArgumentDefinitions());

            Console.WriteLine($"s3-batcher v{typeof(Program).Assembly.GetName().Version.ToString(2)}");
            Console.WriteLine($"Usage: s3-batcher --arg=value");
            Console.WriteLine($"Arguments:\n{string.Join("\n", arguments)}");
        }
    }
}
