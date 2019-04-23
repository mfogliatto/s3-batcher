using Xunit;
using S3Batcher.CommandLine.Arguments;

namespace S3Batcher.CommandLine.Tests
{
    public class ArgumentTests
    {
        [Fact]
        public void Argument_With_Value_Is_Valid()
        {
            const string argName = "argName";
            const string argValue = "argValue";
            var arg = new Argument($"{ArgumentDefinition.PREFIX}{argName}{ArgumentDefinition.VALUE_DELIMITER}{argValue}");

            Assert.Matches(argName, arg.Name);
            Assert.Matches(argValue, arg.Value);
        }
    }
}