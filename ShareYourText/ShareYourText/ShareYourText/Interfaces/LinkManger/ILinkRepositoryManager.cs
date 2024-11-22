namespace ShareYourText.Interfaces.LinkManger
{
    public interface ILinkRepositoryManager
    {
        Task AddLinkAsync();
        Task RemoveLinkAsync();
    }
}

