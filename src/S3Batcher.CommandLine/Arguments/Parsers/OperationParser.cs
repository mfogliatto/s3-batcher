using System;
using System.Collections.Generic;
using System.Linq;
using S3Batcher.Operations;

namespace S3Batcher.CommandLine.Arguments
{
    sealed class OperationParser : IParser<OperationArgument>
    {
        private static Dictionary<string, Type> Mappings = new Dictionary<string, Type>
            {
                {"delete-versions", typeof(DeleteVersions)},
                {"restore-objects", typeof(RestoreObjects)}
            };

        private static ArgumentDefinition OperationArg = ArgumentDefinition.Required("operation", $"The S3 operation that you want to perform in batch. Possible values: {string.Join(",", Mappings.Keys)}.");
        private static ArgumentDefinition BucketArg = ArgumentDefinition.Required("bucket", "The S3 bucket that the selected operation will target.");
        private static ArgumentDefinition PrefixArg = ArgumentDefinition.Optional("prefix", "The prefix that will be used to match the corresponding S3 objects. If not specified, all objects will be matched.", "");
        private static ArgumentDefinition DryRunArg = ArgumentDefinition.Optional("dryrun", "If set to true, it will fetch and list all potentially affected objects but will not perform any mutations on source.", "false");
        private static ArgumentDefinition[] _usedArguments = new ArgumentDefinition[] { OperationArg, BucketArg, PrefixArg, DryRunArg };

        public static IEnumerable<ArgumentDefinition> GetArgumentDefinitions()
        {
            return _usedArguments;
        }

        public OperationArgument GetFrom(IEnumerable<Argument> args)
        {
            var argValues = ArgumentDefinition.GetValues(_usedArguments, args);

            var isValid = Mappings.TryGetValue(argValues[OperationArg.Name], out Type type);
            if (!isValid)
            {
                throw new InvalidOperationException($"Unsupported operation: '{argValues[OperationArg.Name]}'.");
            }

            var bucket = argValues[BucketArg.Name];
            var prefix = argValues[PrefixArg.Name];
            var dryRun = bool.Parse(argValues[DryRunArg.Name]);

            return new OperationArgument(type, new OperationOptions(bucket, prefix, dryRun));
        }
    }
}