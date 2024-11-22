using ShareYourText.Interfaces.Services;

namespace ShareYourText.Interfaces.UserInteraction
{
    public interface IUserInputGetter
    {
        Task<string> GetUserInputLinkAsync(IHashFinder hashFinder);
    }
}

