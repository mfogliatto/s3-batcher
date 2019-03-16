using System;
using System.IO;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3Batcher.CommandLine
{
    class Playground
    {
        static void Entry(string[] args)
        {
            // parse arguments
            // authenticate
            // execute operation
            var credentials = new BasicAWSCredentials(args[0], args[1]);
            var region = RegionEndpoint.USEast2;
            var cancelToken = new System.Threading.CancellationToken();
            var s3 = new AmazonS3Client(credentials, region);

            var listRequest = new ListVersionsRequest
            {
                BucketName = args[2],
                Prefix = args[3],
                MaxKeys = int.MaxValue
            };

            var sb = new StringBuilder();
            var cont = true;
            var deleted = 0;
            Console.WriteLine("Are you sure you want to proceed with deletion?");
            var response = Console.ReadLine();
            try
            {
                while (cont)
                {
                    var list = s3.ListVersionsAsync(listRequest, cancelToken);
                    var result = list.Result;

                    result.Versions.ForEach((obj) =>
                    {
                        deleted++;
                        var str = $"{obj.Key},{obj.VersionId}";
                        Console.WriteLine(str);
                        sb.AppendLine(str);
                    });

                    if (response == "Y" && result.Versions.Any())
                    {
                        // undelete
                        var deleteRequest = new DeleteObjectsRequest
                        {
                            BucketName = listRequest.BucketName,
                            Objects = list.Result.Versions.Select(dm => new KeyVersion { Key = dm.Key, VersionId = dm.VersionId }).ToList()
                        };
                        s3.DeleteObjectsAsync(deleteRequest).Wait();
                        Console.WriteLine("Batch deletion complete!");
                    }

                    listRequest.KeyMarker = result.NextKeyMarker;
                    cont = !string.IsNullOrWhiteSpace(listRequest.KeyMarker);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error: {0}", e.ToString());
                sb.AppendLine($"Exception: {e.ToString()}");
            }

            Console.WriteLine("Deleted files: {0}", deleted);
            Console.WriteLine("Process completed!");
            File.WriteAllText($"deleted-result-{DateTime.UtcNow.Ticks}.csv", sb.ToString());
            Console.ReadKey();
        }
    }
}
