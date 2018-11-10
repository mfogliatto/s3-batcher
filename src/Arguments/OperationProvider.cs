using System;
using System.Collections.Generic;
using System.Linq;
using S3Batcher.Operations;

namespace S3Batcher.Arguments
{
    sealed class OperationProvider : IProvider<Type>
    {
        private const string OPERATION_KEY = "operation";
        private Dictionary<string, Type> _mappings;

        public OperationProvider()
        {
            _mappings = new Dictionary<string, Type>
            {
                {"restore", typeof(RestoreObjects)}
            };
        }

        public Type GetFrom(IEnumerable<Argument> args)
        {
            var operationArg = args.FirstOrDefault(_ => _.Matches(OPERATION_KEY));

            if (operationArg == default(Argument))
            {
                throw new ArgumentMissingException($"An operation must to be provided with [{OPERATION_KEY}] argument.");
            }

            return _mappings[operationArg.Value];
        }
    }
}