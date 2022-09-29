using System;
using System.Threading.Tasks;

// To interact with Amazon S3.
using Amazon.S3;
using Amazon.S3.Model;

namespace S3CreateAndList
{
  class Program
  {
    // Main method
    static async Task Main(string[] args)
    {

        // Create an S3 client object.
      var s3Client = new AmazonS3Client();


      // List the buckets owned by the user.
      // Call a class method that calls the API method.
      Console.WriteLine("\nGetting a list of your objects in...");
      var listResponse = await MyListObjectsAsync(s3Client);

      Console.WriteLine($"Number of folders: {listResponse.CommonPrefixes.Count}");
      foreach(String s in listResponse.CommonPrefixes)
      {
        Console.WriteLine(s);
      }

      Console.WriteLine($"Number of files: {listResponse.S3Objects.Count}");
      foreach(S3Object o in listResponse.S3Objects)
      {
        Console.WriteLine(o.Key);
      }

    }

    // Async method to get a list of Amazon S3 Objects.
    private static async Task<ListObjectsV2Response> MyListObjectsAsync(IAmazonS3 s3Client)
    {
      var request = new ListObjectsV2Request()
      {
          BucketName = "alessandrosbucket",
          Delimiter = "/",
          // Prefix = "carpeta_ejemplo/"
          Prefix = "carpeta_ejemplo/nested_dir/"
      };

      return await s3Client.ListObjectsV2Async(request);
    }

    // Async method to get a list of Amazon S3 buckets.
    private static async Task<ListBucketsResponse> MyListBucketsAsync(IAmazonS3 s3Client)
    {
      return await s3Client.ListBucketsAsync();
    }

  }
}