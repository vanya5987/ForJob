namespace ShareYourText.Interfaces.Services
{
    public interface ILinkRemover
    {
        Task RemoveExpiredBaseLinksAsync();
        Task RemoveExpiredHashLinksAsync();
    }
}

