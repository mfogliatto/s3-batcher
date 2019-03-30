using System;

namespace S3Batcher.CommandLine.Arguments
{
    sealed class Argument
    {
        private string _arg;

        public string Name => _arg.Substring(ArgumentDefinition.PREFIX.Length, _arg.IndexOf(ArgumentDefinition.VALUE_DELIMITER) - ArgumentDefinition.PREFIX.Length);
        public string Value => _arg.Substring(_arg.IndexOf(ArgumentDefinition.VALUE_DELIMITER) + 1);

        public Argument(string arg)
        {
            _arg = arg;
            Validate();
        }

        private void Validate()
        {
            if (!_arg.StartsWith(ArgumentDefinition.PREFIX)) throw new ArgumentException($"Invalid argument {_arg}.");
        }

        public bool Matches(string name)
        {
            return string.Equals(Name, name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}