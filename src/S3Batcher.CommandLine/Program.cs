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
        private static string CmdName = "s3batcher-cli";
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

            try
            {
                var parsedArgs = args.Select(_ => new Argument(_)).ToList();
                Validate(parsedArgs);

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
                Console.WriteLine($"{CmdName}: {e.Message}");
            }
        }

        private static void ShowHelp()
        {
            var arguments = GetArgumentDefinitions();

            Console.WriteLine($"{CmdName} v{typeof(Program).Assembly.GetName().Version.ToString(3)}");
            Console.WriteLine($"Usage: {CmdName} --arg=value");
            Console.WriteLine($"Arguments:\n{string.Join("\n", arguments)}");
        }

        private static IEnumerable<ArgumentDefinition> GetArgumentDefinitions()
        {
            return Enumerable.Concat(
                AwsConnectionParser.GetArgumentDefinitions(),
                OperationParser.GetArgumentDefinitions());
        }

        private static void Validate(IEnumerable<Argument> parsedArgs)
        {
            var allowed = GetArgumentDefinitions().Select(_ => _.Name);
            var provided = parsedArgs.Select(_ => _.Name);
            var unknown = provided.Except(allowed);
            if (unknown.Any())
            {
                throw new ArgumentException($"Unknown arguments: {ArgumentDefinition.PREFIX} {string.Join(", ", unknown)}.");
            }
        }
    }
}
