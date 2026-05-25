using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3Batcher.Objects
{
    sealed class S3ObjectVersionsEnumerable
    {
        private readonly IAmazonS3 _s3Client;
        private readonly ListVersionsRequest _request;

        public S3ObjectVersionsEnumerable(IAmazonS3 s3Client, ListVersionsRequest request)
        {
            _s3Client = s3Client;
            _request = request;
        }

        public async Task<List<S3ObjectVersion>> GetAllAsync()
        {
            var results = new List<S3ObjectVersion>();
            var cont = true;
            while (cont)
            {
                var response = await _s3Client.ListVersionsAsync(_request);
                results.AddRange(response.Versions);

                _request.KeyMarker = response.NextKeyMarker;
                cont = !string.IsNullOrWhiteSpace(_request.KeyMarker);
            }
            return results;
        }
    }
}