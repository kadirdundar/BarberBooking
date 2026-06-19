using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BerberApp1.Services;

public class FirebaseStorageService : IFileStorageService
{
    private readonly string _bucketName;
    private readonly StorageClient _storageClient;

    public FirebaseStorageService(IConfiguration configuration)
    {
        var firebaseSection = configuration.GetSection("Firebase");
        _bucketName = firebaseSection["BucketName"] ?? throw new ArgumentNullException("Firebase:BucketName configuration is missing.");
        var credentialPath = firebaseSection["CredentialFilePath"] ?? throw new ArgumentNullException("Firebase:CredentialFilePath configuration is missing.");

        // Resolve absolute credential path
        var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), credentialPath);
        if (!File.Exists(absolutePath))
        {
            throw new FileNotFoundException($"Firebase credential file not found at: {absolutePath}");
        }

        using var credentialStream = File.OpenRead(absolutePath);
        var credential = GoogleCredential.FromStream(credentialStream);
        _storageClient = StorageClient.Create(credential);
    }

    public async Task<string> UploadFileAsync(byte[] fileBytes, string fileName, string contentType)
    {
        using var memoryStream = new MemoryStream(fileBytes);
        
        // Upload the object with public read permission so it can be viewed by anyone
        var storageObject = await _storageClient.UploadObjectAsync(
            _bucketName,
            fileName,
            contentType,
            memoryStream,
            new UploadObjectOptions
            {
                PredefinedAcl = PredefinedObjectAcl.PublicRead
            });

        // The public URL for public-read Google Cloud Storage objects
        return $"https://storage.googleapis.com/{_bucketName}/{fileName}";
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl)) return;

        try
        {
            // Extract the object name from the URL
            // URL format: https://storage.googleapis.com/{bucket}/{objectName}
            var prefix = $"https://storage.googleapis.com/{_bucketName}/";
            if (fileUrl.StartsWith(prefix))
            {
                var objectName = fileUrl.Substring(prefix.Length);
                objectName = Uri.UnescapeDataString(objectName);
                
                await _storageClient.DeleteObjectAsync(_bucketName, objectName);
            }
        }
        catch (Exception ex)
        {
            // Log error or ignore if object doesn't exist
            Console.WriteLine($"Error deleting file {fileUrl} from Firebase Storage: {ex.Message}");
        }
    }
}
