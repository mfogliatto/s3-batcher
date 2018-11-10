using System.Collections.Generic;

namespace S3Batcher.Arguments
{
    interface IProvider<T>
    {
        T GetFrom(IList<Argument> args);
    }
}