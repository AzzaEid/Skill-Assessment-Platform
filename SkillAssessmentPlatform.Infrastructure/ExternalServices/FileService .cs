using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.Abstract;

namespace SkillAssessmentPlatform.Infrastructure.ExternalServices
{

    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileService> _logger;

        public FileService(IWebHostEnvironment environment, ILogger<FileService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided");
            _logger.LogInformation("WebRootPath: " + _environment.WebRootPath);
            if (string.IsNullOrEmpty(_environment.WebRootPath))
                throw new InvalidOperationException("WebRootPath is not set. Ensure wwwroot folder exists in the project.");

            try
            {
                // Create the directory if it doesn't exist
                var uploadFolder = Path.Combine(_environment.WebRootPath, folderPath);
                Directory.CreateDirectory(uploadFolder);

                // Generate a unique filename
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadFolder, fileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Return the relative path
                //return Path.Combine(folderPath, fileName);
                return Path.Combine(folderPath, fileName).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                throw;
            }
        }

        public async Task DeleteFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_environment.WebRootPath, filePath);
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting file: {filePath}");
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
    }

}
