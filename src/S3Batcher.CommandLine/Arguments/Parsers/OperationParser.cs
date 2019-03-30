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
        private static ArgumentDefinition[] _requiredArgs = new ArgumentDefinition[] { OperationArg, BucketArg, PrefixArg };

        public static IEnumerable<ArgumentDefinition> GetArgumentDefinitions()
        {
            return _requiredArgs;
        }

        public OperationArgument GetFrom(IEnumerable<Argument> args)
        {
            var argValues = new Dictionary<string, string>();
            foreach (var arg in _requiredArgs)
            {
                var found = args.FirstOrDefault(_ => _.Matches(arg.Name));
                if (found == default(Argument) && !arg.IsOptional)
                {
                    throw new ArgumentMissingException($"An --[{arg}] argument is required.");
                }

                if (found != default(Argument))
                {
                    argValues.Add(found.Name, found.Value);
                }
                else
                {
                    argValues.Add(arg.Name, arg.DefaultValue);
                }
            }

            var isValid = Mappings.TryGetValue(argValues[OperationArg.Name], out Type type);
            if (!isValid)
            {
                throw new ArgumentMissingException($"Unsupported operation.");
            }

            var bucket = argValues[BucketArg.Name];
            var prefix = argValues[PrefixArg.Name];
            return new OperationArgument(type, new OperationOptions(bucket, prefix));
        }
    }
}