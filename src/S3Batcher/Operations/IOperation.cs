using System.Threading.Tasks;

namespace S3Batcher.Operations
{
    public interface IOperation
    {
        Task ExecuteAsync(OperationOptions options);
    }
}