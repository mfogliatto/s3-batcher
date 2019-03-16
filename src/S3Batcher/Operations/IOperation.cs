namespace S3Batcher.Operations
{
    public interface IOperation
    {
        void Execute(OperationOptions options);
    }
}