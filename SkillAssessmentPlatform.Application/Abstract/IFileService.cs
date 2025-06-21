using Microsoft.AspNetCore.Http;

namespace SkillAssessmentPlatform.Application.Abstract
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderPath);
        Task DeleteFileAsync(string filePath);
        bool ValidateFileSize(IFormFile file, long maxSizeInBytes);
        bool ValidateFileType(IFormFile file, string[] allowedFileTypes);
    }
}
