namespace BerberApp1.Services;

public interface IFileStorageService
{
    /// <summary>
    /// Uploads a file to Firebase Storage and returns the public accessibility URL.
    /// </summary>
    /// <param name="fileBytes">The binary data of the file.</param>
    /// <param name="fileName">The target name of the file in storage.</param>
    /// <param name="contentType">The MIME type of the file (e.g. image/jpeg).</param>
    /// <returns>The public URL of the uploaded file.</returns>
    Task<string> UploadFileAsync(byte[] fileBytes, string fileName, string contentType);

    /// <summary>
    /// Deletes a file from Firebase Storage.
    /// </summary>
    /// <param name="fileUrl">The full public URL of the file to delete.</param>
    Task DeleteFileAsync(string fileUrl);
}
