namespace S3Batcher.Operations
{
    interface IOperation
    {
        void Execute(OperationOptions options);
    }
}