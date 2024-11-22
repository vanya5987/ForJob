namespace ShareYourText.Interfaces.Services
{
    public interface ILinkAdder
    {
         Task AddHashLinkAsync();
         Task AddBaseLinkAsync();
    }
}

