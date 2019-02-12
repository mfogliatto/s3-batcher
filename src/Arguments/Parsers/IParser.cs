using System.Collections.Generic;

namespace S3Batcher.Arguments
{
    interface IParser<out T>
    {
        T GetFrom(IEnumerable<Argument> args);
    }
}