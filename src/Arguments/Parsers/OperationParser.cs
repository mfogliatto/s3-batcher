using System;
using System.Collections.Generic;
using System.Linq;
using S3Batcher.Operations;

namespace S3Batcher.Arguments
{
    sealed class OperationParser : IParser<OperationArgument>
    {
        private const string OPERATION_KEY = "operation";
        private const string BUCKET_KEY = "bucket";
        private const string PREFIX_KEY = "prefix";
        private string[] _requiredArgs = new string[] { OPERATION_KEY, BUCKET_KEY, PREFIX_KEY };
        private Dictionary<string, Type> _mappings;

        public OperationParser()
        {
            _mappings = new Dictionary<string, Type>
            {
                {"restore", typeof(DeleteVersions)}
            };
        }

        public OperationArgument GetFrom(IEnumerable<Argument> args)
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

            var isValid = _mappings.TryGetValue(relevantArgs[OPERATION_KEY].Value, out Type type);
            if (!isValid)
            {
                throw new ArgumentMissingException($"Unsupported operation.");
            }

            var bucket = relevantArgs[BUCKET_KEY].Value;
            var prefix = relevantArgs[PREFIX_KEY].Value;
            return new OperationArgument(type, new OperationOptions(bucket, prefix));
        }
    }
}