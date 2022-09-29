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
      // var listResponse = await Move(s3Client, "carpeta_ejemplo/otro_ejemplo.txt", "carpeta_ejemplo/nested_dir/");
      // var listResponse = await Delete(s3Client, "carpeta_ejemplo/nested_dir/otro_ejemplo.txt");
      // var listResponse2 = await Move(s3Client, "carpeta_ejemplo/nested_dir/carpeta_ejemplo/otro_ejemplo.txt", "carpeta_ejemplo/");
      // var listResponse2 = await Copy(s3Client, "carpeta_ejemplo/nested_dir/", "carpeta_ejemplo/");
      var listResponse2 = await Copy(s3Client, "something/ejemplo.txt", "carpeta_ejemplo/");
      
    }

    // Async method to get a list of Amazon S3 buckets.
    private static async Task<string> Move(IAmazonS3 s3Client, string key, string new_path)
    {
      var copyResponse = await Copy(s3Client, key, new_path);
      var deleteResponse = await Delete(s3Client, key);
      return "ok";
    }

    // Async method to get a list of Amazon S3 buckets.
    private static async Task<CopyObjectResponse> Copy(IAmazonS3 s3Client, string key, string new_path)
    {
      Console.WriteLine(key.Split("/").Length);
      // var destKey = key.Split("/").Take(1).ToList();
      // destKey.Add("copy_" + key.Split("/").Last());
      // Console.WriteLine(destKey.Count);

      var splitted = key.Split("/");
      var fileName = "copy_" + splitted.Last();
      Console.WriteLine("adaadadad");
      Console.WriteLine(splitted.Length == 1 ? fileName : string.Join("/", splitted.Take(1).Append(fileName)) );

      return await s3Client.CopyObjectAsync(new CopyObjectRequest()
      {
          // SourceBucket = "alessandrosbucket",
          SourceKey = key,
          DestinationBucket = "alessandrosbucket",
          DestinationKey = splitted.Length == 1 ? fileName : splitted.Take(1).ToString() + "/" + fileName
      });

      // return await s3Client.CopyObjectAsync(request);
    }

    public static async Task<DeleteObjectResponse> Delete(IAmazonS3 s3Client, string key)
    {
      var request = new DeleteObjectRequest() {
        BucketName = "alessandrosbucket",
        Key = key
      };

      return await s3Client.DeleteObjectAsync(request);
    }

  }
}