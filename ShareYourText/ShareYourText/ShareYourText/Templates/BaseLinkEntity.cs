using System.ComponentModel.DataAnnotations;
using ShareYourText.LinkManager;

namespace ShareYourText.Templates
{
    public class BaseLinkEntity : LinkDbForm
    {
        [Key]
        public string LinkFormat { get; private set; }
        public int PopularityPoint { get; set; }

        public BaseLinkEntity() : base()
        {
            LinkFormat = LinkGetter.GetBaseLink() ?? throw new ArgumentNullException(nameof(LinkFormat));
            PopularityPoint = 0;
        }
    }
}

