using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PlataformaEducativa.Services
{
    public class GoogleDriveService : IGoogleDriveService
    {
        private readonly IConfiguration _configuration;
        private readonly DriveService _driveService;

        public GoogleDriveService(IConfiguration configuration)
        {
            _configuration = configuration;
            _driveService = GetDriveService().Result;
        }

        private async Task<DriveService> GetDriveService()
        {
            string[] scopes = { DriveService.Scope.Drive };
            string applicationName = _configuration["GoogleDriveSettings:ApplicationName"];
            string credPath = "token.json";

            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }

            // Crear el servicio Drive API
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });

            return service;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = file.FileName,
                Parents = new[] { await GetOrCreateFolderAsync(folderName) }
            };

            using (var stream = file.OpenReadStream())
            {
                var request = _driveService.Files.Create(fileMetadata, stream, GetMimeType(file.FileName));
                request.Fields = "id";
                var response = await request.UploadAsync();

                if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                    throw new Exception($"Error uploading file: {response.Exception.Message}");

                return request.ResponseBody.Id;
            }
        }

        public async Task<string> GetFileLinkAsync(string fileId)
        {
            try
            {
                // Configurar permisos para que cualquiera con el enlace pueda ver
                var permission = new Google.Apis.Drive.v3.Data.Permission
                {
                    Type = "anyone",
                    Role = "reader"
                };

                await _driveService.Permissions.Create(permission, fileId).ExecuteAsync();

                // Obtener el enlace para compartir
                var request = _driveService.Files.Get(fileId);
                request.Fields = "webViewLink";
                var file = await request.ExecuteAsync();

                return file.WebViewLink;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting file link: {ex.Message}");
            }
        }

        public async Task DeleteFileAsync(string fileId)
        {
            try
            {
                await _driveService.Files.Delete(fileId).ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting file: {ex.Message}");
            }
        }

        private async Task<string> GetOrCreateFolderAsync(string folderName)
        {
            // Buscar si la carpeta ya existe
            var listRequest = _driveService.Files.List();
            listRequest.Q = $"mimeType='application/vnd.google-apps.folder' and name='{folderName}' and trashed=false";
            listRequest.Fields = "files(id, name)";
            var folders = await listRequest.ExecuteAsync();

            // Si la carpeta existe, devolver su ID
            if (folders.Files.Count > 0)
                return folders.Files[0].Id;

            // Si no existe, crear la carpeta
            var folderMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder"
            };

            var request = _driveService.Files.Create(folderMetadata);
            request.Fields = "id";
            var folder = await request.ExecuteAsync();

            return folder.Id;
        }

        private string GetMimeType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            switch (extension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".ppt":
                case ".pptx":
                    return "application/vnd.ms-powerpoint";
                case ".doc":
                case ".docx":
                    return "application/msword";
                case ".xls":
                case ".xlsx":
                    return "application/vnd.ms-excel";
                case ".txt":
                    return "text/plain";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".mp4":
                    return "video/mp4";
                default:
                    return "application/octet-stream";
            }
        }
    }
}