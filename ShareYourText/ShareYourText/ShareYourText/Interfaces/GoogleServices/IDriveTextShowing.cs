namespace ShareYourText.Interfaces.GoogleServices
{
    public interface IDriveTextShowing
    {
         Task<string> ShowTextAsync(string fileId);
    }
}

