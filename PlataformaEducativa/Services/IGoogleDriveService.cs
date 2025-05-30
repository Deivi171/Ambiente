using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PlataformaEducativa.Services
{
    public interface IGoogleDriveService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
        Task<string> GetFileLinkAsync(string fileId);
        Task DeleteFileAsync(string fileId);
    }
}