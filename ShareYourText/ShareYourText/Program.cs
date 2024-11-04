using System.Security.Cryptography;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;


namespace ShareYourText
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(FormCreater.GetForm());
        }
    }
    public interface IGenerateLink
    {
        string GenerateSHA256Hash();
        string GenerateBaseLink();
    }

    public interface ICreateLink
    {
        string CreateLinkHash();
        string CreateLink();
    }

    public interface ILinkHandler
    {
        void AddHashLink();
        void AddBaseLink();
        void RemoveExpiredBaseLinks();
        void RemoveExpiredHashLinks();
        void InitializeDatabase();
        void LikePost();
        void DislikePost();
        void ShowTextForLink(TextBox textBox);
    }

    public interface ICombinedLinkManager
    {
        void ManageLink();
    }

    public interface IShowUI
    {
        void ShowPopularityLinksList(ListView listView);
    }

    public interface IGetId
    {
        string GetFileId();
    }

    public interface IDriveService
    {
        string SaveText(string content, string fileName);
        string ShowText(string fileId);
        string ExtractFileId(string url);
    }

    public sealed class LinkGenerator : IGenerateLink
    {
        private readonly string _linkForm;

        public LinkGenerator(string linkForm)
        {
            _linkForm = linkForm ?? throw new ArgumentNullException(nameof(_linkForm));
        }

        public string GenerateSHA256Hash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(_linkForm));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        public string GenerateBaseLink() => _linkForm;
    }

    public sealed class LinkCreator : ICreateLink
    {
        private readonly IGenerateLink _generateLink;
        private readonly string _id;

        public LinkCreator(string id)
        {
            _id = id ?? throw new ArgumentNullException(nameof(id));
            _generateLink = new LinkGenerator($"https://drive.google.com/file/d/{_id}/view") ?? throw new ArgumentNullException(nameof(_generateLink));
        }

        public string CreateLink() => _generateLink.GenerateBaseLink();

        public string CreateLinkHash() => _generateLink.GenerateSHA256Hash();
    }

    public sealed class GoogleDriveUpload : IDriveService
    {
        private readonly DriveService _driveService;

        public GoogleDriveUpload(string credentialsPath)
        {
            _driveService = InitializeDriveService(credentialsPath) ?? throw new ArgumentNullException(nameof(_driveService));
        }

        private DriveService InitializeDriveService(string credentialsPath)
        {
            UserCredential credential;

            using (FileStream stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { DriveService.Scope.DriveFile },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("DriveCredentialStore")).Result;
            }

            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Text Saver",
            });
        }

        public string SaveText(string fileName, string content)
        {
            Google.Apis.Drive.v3.Data.File fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = fileName,
                MimeType = "text/plain"
            };

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                FilesResource.CreateMediaUpload request = _driveService.Files.Create(fileMetadata, stream, "text/plain");
                request.Fields = "id";

                IUploadProgress file = request.Upload();

                if (file.Status != Google.Apis.Upload.UploadStatus.Completed)
                    throw new Exception("Не удалось сохранить файл в Google Drive.");

                string fileId = request.ResponseBody.Id;

                SetFilePublic(fileId);

                return fileId;
            }
        }

        private void SetFilePublic(string fileId)
        {
            Google.Apis.Drive.v3.Data.Permission permission = new Google.Apis.Drive.v3.Data.Permission()
            {
                Type = "anyone",
                Role = "reader"
            };

            _driveService.Permissions.Create(permission, fileId).Execute();
        }

        public string ShowText(string fileId)
        {
            FilesResource.GetRequest request = _driveService.Files.Get(fileId);
            MemoryStream stream = new MemoryStream();

            request.Download(stream);
            stream.Position = 0;

            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public string ExtractFileId(string url)
        {
            Regex regex = new Regex(@"(?:/d/|id=)([^/]+)");
            Match match = regex.Match(url);

            if (match.Success)
                return match.Groups[1].Value;

            throw new ArgumentException("Некорректная ссылка на файл Google Drive.", nameof(url));
        }
    }

    public sealed class FileCreater : IGetId
    {
        private readonly string _id;

        public FileCreater(string id)
        {
            _id = id ?? throw new ArgumentNullException(nameof(_id));
        }

        public string GetFileId() => _id;
    }

    public sealed class LinkGetter
    {
        private static ICreateLink _baseLinkCreator;
        private static ICreateLink _hashLinkCreator;

        public LinkGetter(IGetId fileCreater)
        {
            _baseLinkCreator = new LinkCreator(fileCreater.GetFileId()) ?? throw new ArgumentNullException(nameof(_baseLinkCreator));
            _hashLinkCreator = new LinkCreator(fileCreater.GetFileId()) ?? throw new ArgumentNullException(nameof(_hashLinkCreator));
        }

        public static string GetHashLink() => _hashLinkCreator.CreateLinkHash();

        public static string GetBaseLink() => _baseLinkCreator.CreateLink();
    }

    public sealed class LinkManager : ICombinedLinkManager
    {
        private readonly ILinkHandler _linkHandler;

        public LinkManager(ILinkHandler linkHandler)
        {
            _linkHandler = linkHandler ?? throw new ArgumentNullException(nameof(_linkHandler));
        }

        public void ManageLink()
        {
            _linkHandler.AddBaseLink();
            _linkHandler.AddHashLink();
            _linkHandler.RemoveExpiredBaseLinks();
            _linkHandler.RemoveExpiredHashLinks();
        }
    }

    public abstract class LinkDbForm
    {
        public DateTime ExpirationDate { get; private set; }

        public LinkDbForm()
        {
            ExpirationDate = DateTime.UtcNow.AddDays(7);
        }
    }

    public class BaseLinkEntity : LinkDbForm
    {
        public string LinkFormat { get; private set; }
        public int PopularityPoint { get; set; }

        public BaseLinkEntity() : base()
        {
            LinkFormat = LinkGetter.GetBaseLink() ?? throw new ArgumentNullException(nameof(LinkFormat));
            PopularityPoint = 0;
        }
    }

    public class HashLinkEntity : LinkDbForm
    {
        public string LinkFormat { get; private set; }
        public HashLinkEntity() : base()
        {
            LinkFormat = LinkGetter.GetHashLink() ?? throw new ArgumentNullException(nameof(LinkFormat));
        }
    }

    public sealed class LinkBaseDbContext : DbContext
    {
        public DbSet<BaseLinkEntity> BaseLinks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dataDirectory = Path.Combine(projectDirectory, "Data");

            if (!Directory.Exists(dataDirectory))
                Directory.CreateDirectory(dataDirectory);

            string dbPath = Path.Combine(dataDirectory, "baseLinks.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseLinkEntity>()
                .HasKey(b => b.LinkFormat);

            base.OnModelCreating(modelBuilder);
        }
    }

    public sealed class LinkHashDbContext : DbContext
    {
        public DbSet<HashLinkEntity> HashLinks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dataDirectory = Path.Combine(projectDirectory, "Data");

            if (!Directory.Exists(dataDirectory))
                Directory.CreateDirectory(dataDirectory);

            string dbPath = Path.Combine(dataDirectory, "hashLinks.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HashLinkEntity>()
                .HasKey(b => b.LinkFormat);

            base.OnModelCreating(modelBuilder);
        }
    }

    public sealed class LinkHandler : ILinkHandler
    {
        private readonly IDriveService _driveService;

        public LinkHandler(IDriveService driveService)
        {
            _driveService = driveService ?? throw new ArgumentNullException(nameof(_driveService));
        }

        public void AddBaseLink()
        {
            using (LinkBaseDbContext context = new LinkBaseDbContext())
            {
                BaseLinkEntity linkEntity = new BaseLinkEntity();
                context.BaseLinks.Add(linkEntity);
                context.SaveChanges();
            }
        }

        public void RemoveExpiredBaseLinks()
        {
            using (LinkBaseDbContext context = new LinkBaseDbContext())
            {
                DateTime now = DateTime.UtcNow;
                List<BaseLinkEntity> expiredLinks = context.BaseLinks.Where(link => link.ExpirationDate < now).ToList();

                context.BaseLinks.RemoveRange(expiredLinks);
                context.SaveChanges();
            }
        }

        public void AddHashLink()
        {
            using (LinkHashDbContext context = new LinkHashDbContext())
            {
                HashLinkEntity hashLinkEntity = new HashLinkEntity();
                context.HashLinks.Add(hashLinkEntity);
                context.SaveChanges();
            }
        }

        public void RemoveExpiredHashLinks()
        {
            using (LinkHashDbContext context = new LinkHashDbContext())
            {
                DateTime now = DateTime.UtcNow;
                List<HashLinkEntity> expiredLinks = context.HashLinks.Where(link => link.ExpirationDate < now).ToList();

                context.HashLinks.RemoveRange(expiredLinks);
                context.SaveChanges();
            }
        }

        public void InitializeDatabase()
        {
            using (LinkBaseDbContext context = new LinkBaseDbContext())
            {
                context.Database.EnsureCreated();
            }

            using (LinkHashDbContext context = new LinkHashDbContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public void LikePost()
        {
            AddPoint(+1);
        }

        public void DislikePost()
        {
            AddPoint(-1);
        }

        private void AddPoint(int point)
        {
            using (LinkBaseDbContext context = new LinkBaseDbContext())
            {
                var linkEntity = context.BaseLinks
                            .SingleOrDefault(link => link.LinkFormat == GetUserInputLink());

                if (linkEntity == null)
                    throw new Exception("Ссылка отсутствует или была удалена...");

                linkEntity.PopularityPoint += point;
                context.SaveChanges();
            }
        }

        private string HashLinkFinder(IGenerateLink generateLink)
        {
            using (LinkHashDbContext context = new LinkHashDbContext())
            {
                string hashLink = generateLink.GenerateSHA256Hash();

                HashLinkEntity hashLinkEntity = context.HashLinks.SingleOrDefault(link => link.LinkFormat == hashLink);

                if (hashLinkEntity != null)
                    return generateLink.GenerateBaseLink();

                return null;
            }
        }

        private string GetUserInputLink()
        {
            ShareText form = FormCreater.GetForm();
            IGenerateLink generateLink = new LinkGenerator(form.UserInputLink.Text);

            return HashLinkFinder(generateLink);
        }

        public void ShowTextForLink(TextBox textBox)
        {
            using (LinkBaseDbContext context = new LinkBaseDbContext())
            {
                var linkEntity = context.BaseLinks
                            .SingleOrDefault(link => link.LinkFormat == GetUserInputLink());

                if (linkEntity == null)
                    throw new Exception("Ссылка отсутствует или была удалена...");

                string userFileId = _driveService.ExtractFileId(linkEntity.LinkFormat);

                textBox.Text = _driveService.ShowText(userFileId);
            }
        }
    }

    public sealed class FormCreater
    {
        private static ShareText _form;

        private FormCreater() { }

        public static ShareText GetForm()
        {
            if (_form == null)
                _form = new ShareText();

            return _form;
        }
    }

    public sealed class UI : IShowUI
    {
        public void ShowPopularityLinksList(ListView listView)
        {
            using (LinkBaseDbContext context = new LinkBaseDbContext())
            {
                var baseLinks = context.BaseLinks
                    .OrderByDescending(link => link.PopularityPoint)
                    .Take(10)
                    .Select(link => new
                    {
                        LinkFormat = link.LinkFormat,
                        ExpirationDate = link.ExpirationDate,
                        PopularityPoint = link.PopularityPoint
                    })
                    .ToList();

                listView.Items.Clear();

                foreach (var link in baseLinks)
                {
                    ListViewItem item = new ListViewItem(link.LinkFormat);

                    item.SubItems.Add(link.ExpirationDate.ToString("g"));
                    item.SubItems.Add(link.PopularityPoint.ToString());
                    listView.Items.Add(item);
                }
            }
        }
    }
}

