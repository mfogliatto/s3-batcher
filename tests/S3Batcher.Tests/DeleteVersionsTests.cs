using System.Collections.Generic;
using System.Threading;
using Amazon.S3;
using Amazon.S3.Model;
using FakeItEasy;
using S3Batcher.Operations;
using Xunit;

namespace S3Batcher.Tests
{
    public class DeleteVersionsTests
    {
        [Fact]
        public void DryRun_Does_Not_Perform_Any_Delete_On_Source()
        {
            var bucketName = "bucket";
            var s3Client = A.Fake<IAmazonS3>();
            A.CallTo(() => s3Client.ListVersionsAsync(bucketName, A<CancellationToken>.Ignored)).Returns(new ListVersionsResponse
            {
                Versions = new List<S3ObjectVersion>
                {
                    new S3ObjectVersion(),
                    new S3ObjectVersion(),
                }
            });

            var restoreObjects = new RestoreObjects(s3Client);
            restoreObjects.Execute(new OperationOptions(bucketName, "prefix", true));

            A.CallTo(() => s3Client.DeleteObjectsAsync(A<DeleteObjectsRequest>.Ignored, A<CancellationToken>.Ignored)).MustNotHaveHappened();
        }
    }
}