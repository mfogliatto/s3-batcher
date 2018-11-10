using System;

namespace S3Batcher.Arguments
{
    sealed class Argument
    {
        private const string PREFIX = "--";
        private const char VALUE_DELIMITER = '=';

        private string _arg;

        public string Key => _arg.Substring(PREFIX.Length);
        public string Value => IsBoolean ? "" : _arg.Substring(VALUE_DELIMITER);
        public bool IsBoolean => _arg.IndexOf('=') == -1;

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