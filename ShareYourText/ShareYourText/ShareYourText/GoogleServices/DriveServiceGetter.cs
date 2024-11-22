using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using ShareYourText.Interfaces.GoogleServices;


namespace ShareYourText.GoogleServices
{
    public sealed class DriveServiceGetter : IGetDriveService
    {
        public async Task<DriveService> InitializeDriveServiceAsync()
        {
            string credentialPath = "C:\\Users\\admin\\Desktop\\ShareYourText\\ShareYourText\\appSettings.json";
            UserCredential credential;

            using (FileStream stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { DriveService.Scope.DriveFile },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("DriveCredentialStore"));
            }

            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Text Saver",
            });
        }
    }
}

