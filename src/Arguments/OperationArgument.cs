using System;
using System.Collections.Generic;
using S3Batcher.Operations;

namespace S3Batcher.Arguments
{
    class OperationArgument
    {
        public Type Type { get; }
        public OperationOptions Options { get; }

        public OperationArgument(Type type, OperationOptions options)
        {
            Type = type;
            Options = options;
        }
    }
}