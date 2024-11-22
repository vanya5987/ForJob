using Google.Apis.Drive.v3;
using Google.Apis.Upload;
using ShareYourText.Interfaces.GoogleServices;
using System.Text;
using System.Text.RegularExpressions;


namespace ShareYourText.GoogleServices
{
    public sealed class GoogleDriveUpload : IDriveTextSaver, IDriveTextShowing, IDriveExtractFile
    {
        private readonly DriveService _driveService;

        public GoogleDriveUpload(DriveService driveService)
        {
            _driveService = driveService ?? throw new ArgumentNullException(nameof(_driveService));
        }

        public string SaveText(string fileName, string content)
        {
            Google.Apis.Drive.v3.Data.File fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = fileName,
                MimeType = "text/plain"
            };

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                FilesResource.CreateMediaUpload request = _driveService.Files.Create(fileMetadata, stream, "text/plain");
                request.Fields = "id";

                IUploadProgress file = request.Upload();

                if (file.Status != UploadStatus.Completed)
                    throw new Exception("Не удалось сохранить файл в Google Drive.");

                string fileId = request.ResponseBody.Id;

                SetFilePublic(fileId);

                return fileId;
            }
        }

        private void SetFilePublic(string fileId)
        {
            Google.Apis.Drive.v3.Data.Permission permission = new Google.Apis.Drive.v3.Data.Permission()
            {
                Type = "anyone",
                Role = "reader"
            };

            _driveService.Permissions.Create(permission, fileId).Execute();
        }

        public async Task<string> ShowTextAsync(string fileId)
        {
            FilesResource.GetRequest request = _driveService.Files.Get(fileId);
            MemoryStream stream = new MemoryStream();

            await request.DownloadAsync(stream);
            stream.Position = 0;

            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public string ExtractFileId(string url)
        {
            Regex regex = new Regex(@"(?:/d/|id=)([^/]+)");
            Match match = regex.Match(url);

            if (match.Success)
                return match.Groups[1].Value;

            throw new ArgumentException("Некорректная ссылка на файл Google Drive.", nameof(url));
        }
    }
}

