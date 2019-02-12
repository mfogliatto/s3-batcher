namespace S3Batcher.Operations
{
    class OperationOptions
    {
        public string Prefix { get; }

        public OperationOptions(string prefix)
        {
            Prefix = prefix;
        }
    }
}