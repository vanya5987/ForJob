using ShareYourText.Interfaces.GoogleServices;

namespace ShareYourText.Interfaces.UIManager
{
    public interface IShowFileText
    {
        Task ShowFileTextAsync(TextBox textBox, IDriveExtractFile extarctText, IDriveTextShowing textShowing);
    }
}

