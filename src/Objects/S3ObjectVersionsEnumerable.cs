using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3Batcher.Objects
{
    public class S3ObjectVersionsEnumerable : IEnumerable<S3ObjectVersion>
    {
        private readonly IAmazonS3 _s3Client;
        private readonly ListVersionsRequest _request;
        private ListVersionsResponse _response;

        public S3ObjectVersionsEnumerable(IAmazonS3 s3Client, ListVersionsRequest request)
        {
            _s3Client = s3Client;
            _request = request;
        }

        public IEnumerator<S3ObjectVersion> GetEnumerator()
        {
            var cont = true;
            while (cont)
            {
                _response = _s3Client.ListVersionsAsync(_request).Result;
                foreach (var o in _response.Versions)
                {
                    yield return o;
                }

                _request.KeyMarker = _response.NextKeyMarker;
                cont = !string.IsNullOrWhiteSpace(_request.KeyMarker);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}