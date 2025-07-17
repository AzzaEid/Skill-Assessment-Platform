using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.Abstract;

namespace SkillAssessmentPlatform.Infrastructure.Services.ExternalServices
{
    public class S3FileService : IFileService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<S3FileService> _logger;
        private readonly string _bucketName;
        private readonly string _region;

        public S3FileService(IAmazonS3 s3Client, ILogger<S3FileService> logger, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _logger = logger;
            _bucketName = configuration["AWS:BucketName"] ?? "ratify-files";
            _region = configuration["AWS:Region"] ?? "us-east-1";
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided");

            try
            {
                // unique filename
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var key = $"{folderPath}/{fileName}";

                //  upload request
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                // Upload to S3
                var response = await _s3Client.PutObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Return the public URL
                    var fileUrl = $"https://{_bucketName}.s3.{_region}.amazonaws.com/{key}";
                    _logger.LogInformation($"File uploaded successfully: {fileUrl}");
                    return fileUrl;
                }
                else
                {
                    throw new Exception($"Failed to upload file to S3. Status: {response.HttpStatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file to S3");
                throw;
            }
        }

        public async Task DeleteFileAsync(string filePath)
        {
            try
            {
                // Extract key from URL if it's a full URL
                string key = ExtractKeyFromUrl(filePath);

                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                await _s3Client.DeleteObjectAsync(request);
                _logger.LogInformation($"File deleted successfully: {key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting file from S3: {filePath}");
                throw;
            }
        }

        public bool ValidateFileSize(IFormFile file, long maxSizeInBytes)
        {
            return file != null && file.Length <= maxSizeInBytes;
        }

        public bool ValidateFileType(IFormFile file, string[] allowedFileTypes)
        {
            if (file == null) return false;
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedFileTypes.Contains(fileExtension);
        }

        private string ExtractKeyFromUrl(string urlOrPath)
        {
            // If it's already a key (not a full URL), return as is
            if (!urlOrPath.StartsWith("http"))
                return urlOrPath;

            // Extract key from S3 URL
            var uri = new Uri(urlOrPath);
            return uri.AbsolutePath.TrimStart('/');
        }
    }



}
