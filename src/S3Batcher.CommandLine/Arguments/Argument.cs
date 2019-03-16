using System;

namespace S3Batcher.CommandLine.Arguments
{
    sealed class Argument
    {
        private const string PREFIX = "--";
        private const char VALUE_DELIMITER = '=';

        private string _arg;

        public string Key => _arg.Substring(PREFIX.Length, _arg.IndexOf(VALUE_DELIMITER) - PREFIX.Length);
        public string Value => _arg.Substring(_arg.IndexOf(VALUE_DELIMITER) + 1);

        public Argument(string arg)
        {
            _arg = arg;
            Validate();
        }

        private void Validate()
        {
            if (!_arg.StartsWith(PREFIX)) throw new ArgumentException($"Invalid argument {_arg}.");
        }

        public bool Matches(string key)
        {
            return string.Equals(Key, key, StringComparison.InvariantCulture);
        }
    }
}