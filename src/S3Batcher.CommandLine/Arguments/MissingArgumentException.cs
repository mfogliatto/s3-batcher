using System;

namespace S3Batcher.CommandLine.Arguments
{
    sealed class MissingArgumentException : Exception
    {
        public MissingArgumentException(string argKey) : base($"Missing required '{argKey}' argument.")
        {
        }
    }
}