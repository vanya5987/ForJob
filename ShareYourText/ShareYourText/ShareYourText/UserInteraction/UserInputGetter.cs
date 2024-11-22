using ShareYourText.Interfaces.LinkManger;
using ShareYourText.Interfaces.Services;
using ShareYourText.Interfaces.UserInteraction;
using ShareYourText.LinkManager;
using ShareYourText.Singleton;

namespace ShareYourText.UserInteraction
{
    public sealed class UserInputGetter : IUserInputGetter
    {
        public async Task<string> GetUserInputLinkAsync(IHashFinder hashFinder)
        {
            ShareText form = FormCreater.GetForm();
            IGenerateLink generateLink = new LinkGenerator(form.UserInputLink.Text);

            return await hashFinder.HashLinkFinderAsync(generateLink);
        }
    }
}

