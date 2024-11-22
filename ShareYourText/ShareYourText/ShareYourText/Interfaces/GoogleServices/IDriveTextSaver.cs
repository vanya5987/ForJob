namespace ShareYourText.Interfaces.GoogleServices
{
    public interface IDriveTextSaver
    {
        string SaveText(string content, string fileName);
    }
}

