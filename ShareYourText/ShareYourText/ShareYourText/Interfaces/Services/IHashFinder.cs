using ShareYourText.Interfaces.LinkManger;

namespace ShareYourText.Interfaces.Services
{
    public interface IHashFinder
    {
        Task<string> HashLinkFinderAsync(IGenerateLink generateLink);
    }
}

