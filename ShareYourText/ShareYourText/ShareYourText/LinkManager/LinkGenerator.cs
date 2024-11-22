using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ShareYourText.Interfaces.LinkManger;


namespace ShareYourText.LinkManager
{
    public sealed class LinkGenerator : IGenerateLink
    {
        [Url]
        [Required]
        private readonly string _url;

        public LinkGenerator(string url)
        {
            _url = url ?? throw new ArgumentNullException(nameof(_url));
        }

        public string GenerateSHA256Hash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(_url));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        public string GenerateBaseLink() => _url;
    }
}

