using System;

namespace S3Batcher.CommandLine.Arguments
{
    sealed class ArgumentMissingException : Exception
    {
        private string _argKey;

        public ArgumentMissingException(string argKey)
        {
            _argKey = argKey;
        }

        public override string ToString()
        {
            return $"Missing required [{_argKey}] argument.";
        }
    }
}