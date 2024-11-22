using System.ComponentModel.DataAnnotations;
using ShareYourText.LinkManager;


namespace ShareYourText.Templates
{
    public class HashLinkEntity : LinkDbForm
    {
        [Key]
        public string LinkFormat { get; private set; }
        public HashLinkEntity() : base()
        {
            LinkFormat = LinkGetter.GetHashLink() ?? throw new ArgumentNullException(nameof(LinkFormat));
        }
    }
}

