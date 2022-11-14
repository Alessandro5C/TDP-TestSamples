using System;
using System.IO.Compression;
using System.Threading.Tasks;

// To interact with Amazon S3.
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

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

	    // 100 requests in parallel - adjust as needed
      var config = new TransferUtilityConfig { ConcurrentServiceRequests = 100 };
      using var utility = new TransferUtility(s3Client, config);
      var request = new TransferUtilityDownloadDirectoryRequest
      {
        DownloadFilesConcurrently = true,
        BucketName = "alessandrosbucket", // your bucket name here
        LocalDirectory = @"something", // the path where to store the downloaded files
        // LocalDirectory = @"C:\Users\aldav\Downloads\Nueva carpeta6", // the path where to store the downloaded files
        // S3Directory = "miusuario/_copy/copy_requisitos/" // the bucket prefix (folder) for a month
        S3Directory = "miusuario/_copy/" // the bucket prefix (folder) for a month
      };

      await utility.DownloadDirectoryAsync(request);
      
      // var downloadPath = @"C:\Users\aldav\Downloads\" + @"Nueva Carpeta6\";
      // ZipFile.CreateFromDirectory(downloadPath, @"C:\Users\aldav\Downloads\" + @"Nueva Carpeta6" + ".zip");
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