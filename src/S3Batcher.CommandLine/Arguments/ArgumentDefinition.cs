namespace S3Batcher.CommandLine.Arguments
{
    sealed class ArgumentDefinition
    {
        internal const string PREFIX = "--";
        internal const char VALUE_DELIMITER = '=';

        public string Name { get; }
        public string Description { get; }
        public string DefaultValue { get; }

        public bool IsOptional => DefaultValue != null;

        private ArgumentDefinition(string name, string description, string defaultValue)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
        }

        public static ArgumentDefinition Required(string name, string description)
        {
            return new ArgumentDefinition(name, description, null);
        }

        public static ArgumentDefinition Optional(string name, string description, string defaultValue)
        {
            return new ArgumentDefinition(name, description, defaultValue);
        }

        public override string ToString()
        {
            return $"{(IsOptional ? "[OPTIONAL]" : string.Empty)} {PREFIX}{Name}: {Description} {(IsOptional ? "Default Value: " + DefaultValue : string.Empty)}";
        }
    }
}