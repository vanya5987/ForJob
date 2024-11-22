using Google.Apis.Drive.v3;


namespace ShareYourText.Interfaces.GoogleServices
{
    public interface IGetDriveService
    {
        Task<DriveService> InitializeDriveServiceAsync();
    }
}

