namespace S3Batcher
{
    public sealed class CommandLineArgument
    {
        public string Name { get; }
        public string Value { get; }

        public CommandLineArgument(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}