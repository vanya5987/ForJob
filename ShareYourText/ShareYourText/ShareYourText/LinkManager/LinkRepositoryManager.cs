using ShareYourText.Interfaces.LinkManger;
using ShareYourText.Interfaces.Services;

namespace ShareYourText.LinkManager
{
    public sealed class LinkRepositoryManager : ILinkRepositoryManager
    {
        private readonly ILinkAdder _linkAdder;
        private readonly ILinkRemover _linkRemover;

        public LinkRepositoryManager(ILinkAdder linkAdder, ILinkRemover linkRemover)
        {
            _linkAdder = linkAdder ?? throw new ArgumentNullException(nameof(_linkAdder));
            _linkRemover = linkRemover ?? throw new ArgumentNullException(nameof(_linkRemover));
        }

        public async Task AddLinkAsync()
        {
            await _linkAdder.AddBaseLinkAsync();
            await _linkAdder.AddHashLinkAsync();
        }

        public async Task RemoveLinkAsync()
        {
           await _linkRemover.RemoveExpiredBaseLinksAsync();
           await _linkRemover.RemoveExpiredHashLinksAsync();
        }
    }
}

