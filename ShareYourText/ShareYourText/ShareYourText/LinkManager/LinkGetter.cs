using ShareYourText.Interfaces.LinkManger;

namespace ShareYourText.LinkManager
{
    public sealed class LinkGetter
    {
        private static ICreateLink _baseLinkCreator;
        private static ICreateLink _hashLinkCreator;

        public LinkGetter(ICreateLink baseLinkCreator, ICreateLink hashLinkCreator)
        {
            _baseLinkCreator = baseLinkCreator ?? throw new ArgumentNullException(nameof(_baseLinkCreator));
            _hashLinkCreator = hashLinkCreator ?? throw new ArgumentNullException(nameof(_hashLinkCreator));
        }

        public static string GetHashLink() => _hashLinkCreator.CreateLinkHash();

        public static string GetBaseLink() => _baseLinkCreator.CreateLink();
    }
}

