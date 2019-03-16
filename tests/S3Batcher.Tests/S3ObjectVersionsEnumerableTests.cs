using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using FakeItEasy;
using S3Batcher.Objects;
using Xunit;

namespace S3Batcher.Tests
{
    public class S3ObjectVersionsEnumerableTests
    {
        [Fact]
        public void Enumerable_Returns_All_Versions_In_A_Single_Page()
        {
            var expectedVersions = new System.Collections.Generic.List<S3ObjectVersion>
                {
                    new S3ObjectVersion { },
                    new S3ObjectVersion { }
                };
            var s3client = A.Fake<IAmazonS3>();
            A.CallTo(() => s3client.ListVersionsAsync(default(ListVersionsRequest), default(CancellationToken)))
                .WithAnyArguments()
                .Returns(new ListVersionsResponse
                {
                    Versions = expectedVersions
                });
            var enumerable = new S3ObjectVersionsEnumerable(s3client, A.Fake<ListVersionsRequest>());

            Assert.True(enumerable.ToList().Count == expectedVersions.Count);
        }

        [Fact]
        public void Enumerable_Returns_All_Items_In_Multiple_Pages()
        {
            var expectedVersions = new System.Collections.Generic.List<S3ObjectVersion>
                {
                    new S3ObjectVersion { },
                    new S3ObjectVersion { }
                };
            var s3client = A.Fake<IAmazonS3>();
            A.CallTo(() => s3client.ListVersionsAsync(default(ListVersionsRequest), default(CancellationToken)))
                .WithAnyArguments()
                .ReturnsNextFromSequence(
                    new ListVersionsResponse
                    {
                        NextKeyMarker = "nextKeyMarker",
                        Versions = expectedVersions
                    },
                    new ListVersionsResponse
                    {
                        Versions = expectedVersions
                    });
            var enumerable = new S3ObjectVersionsEnumerable(s3client, A.Fake<ListVersionsRequest>());

            Assert.True(enumerable.Count() == expectedVersions.Count * 2);
        }

        [Fact]
        public void Enumerable_Returns_No_Items_When_Empty()
        {
            var enumerable = new S3ObjectVersionsEnumerable(A.Fake<IAmazonS3>(), A.Fake<ListVersionsRequest>());

            Assert.True(enumerable.Count() == 0);
        }
    }
}