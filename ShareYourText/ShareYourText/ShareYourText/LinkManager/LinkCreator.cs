using ShareYourText.Interfaces.LinkManger;

namespace ShareYourText.LinkManager
{
    public sealed class LinkCreator : ICreateLink
    {
        private readonly IGenerateLink _generateLink;

        public LinkCreator(IGenerateLink generateLink)
        {
            _generateLink = generateLink ?? throw new ArgumentNullException(nameof(_generateLink));
        }

        public string CreateLink() => _generateLink.GenerateBaseLink();

        public string CreateLinkHash() => _generateLink.GenerateSHA256Hash();
    }
}

