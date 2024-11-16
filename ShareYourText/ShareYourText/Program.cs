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

    public interface IPostController
    {
        void LikePost();
        void DislikePost();
    }

    public interface IDatabaseLinkAdder
    {
        void AddHashLink();
        void AddBaseLink();
    }

    public interface IDatabaseLinkRemover
    {
        void RemoveExpiredBaseLinks();
        void RemoveExpiredHashLinks();
    }

    public interface IDatabaseInitializator
    {
        void InitializeDatabase();
    }

    public interface ILinkRepositoryManager
    {
        void AddLink();
        void RemoveLink();
    }

    public interface IShowUI
    {
        void ShowPopularityLinks(ListView listView);
    }

    public interface IDriveTextSaver
    {
        string SaveText(string content, string fileName);
    }

    public interface IDriveTextShowing
    {
        string ShowText(string fileId);
    }

    public interface IDriveExtractFile
    {
        string ExtractFileId(string url);
    }

    public interface IGetDriveService
    {
        DriveService InitializeDriveService(string credentialsPath);
    }

    public interface IHashFinder
    {
        string HashLinkFinder(IGenerateLink generateLink);
    }

    public interface IUserInputGetter
    {
        string GetUserInputLink(IHashFinder hashFinder);
    }

    public interface IShowFileText
    {
        void ShowFileText(TextBox textBox, IDriveExtractFile extarctText, IDriveTextShowing textShowing);
    }

    public sealed class LinkGenerator : IGenerateLink
    {
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

    public sealed class GoogleDriveUpload : IDriveTextSaver, IDriveTextShowing, IDriveExtractFile
    {
        private readonly DriveService _driveService;

        public GoogleDriveUpload(DriveService driveService)
        {
            _driveService = driveService ?? throw new ArgumentNullException(nameof(_driveService));
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

    public sealed class DriveServiceGetter : IGetDriveService
    {
        public DriveService InitializeDriveService(string credentialsPath)
        {
            UserCredential credential;

            using (FileStream stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
#pragma warning disable CS0618
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { DriveService.Scope.DriveFile },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("DriveCredentialStore")).Result;
#pragma warning restore CS0618
            }

            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Text Saver",
            });
        }
    }

    public sealed class LinkGetter
    {
#pragma warning disable CS8618
        private static ICreateLink _baseLinkCreator;
        private static ICreateLink _hashLinkCreator;
#pragma warning restore CS8618 

        public LinkGetter(ICreateLink baseLinkCreator, ICreateLink hashLinkCreator)
        {
            _baseLinkCreator = baseLinkCreator ?? throw new ArgumentNullException(nameof(_baseLinkCreator));
            _hashLinkCreator = hashLinkCreator ?? throw new ArgumentNullException(nameof(_hashLinkCreator));
        }

        public static string GetHashLink() => _hashLinkCreator.CreateLinkHash();

        public static string GetBaseLink() => _baseLinkCreator.CreateLink();
    }

    public sealed class LinkRepositoryManager : ILinkRepositoryManager
    {
        private readonly IDatabaseLinkAdder _linkAdder;
        private readonly IDatabaseLinkRemover _linkRemover;

        public LinkRepositoryManager(IDatabaseLinkAdder linkAdder, IDatabaseLinkRemover linkRemover)
        {
            _linkAdder = linkAdder ?? throw new ArgumentNullException(nameof(_linkAdder));
            _linkRemover = linkRemover ?? throw new ArgumentNullException(nameof(_linkRemover));
        }

        public void AddLink()
        {
            _linkAdder.AddBaseLink();
            _linkAdder.AddHashLink();
        }

        public void RemoveLink()
        {
            _linkRemover.RemoveExpiredBaseLinks();
            _linkRemover.RemoveExpiredHashLinks();
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

    public sealed class PostController : IPostController
    {
        private readonly IHashFinder _hashFinder;
        private readonly IUserInputGetter _userInput;

        public PostController(IHashFinder hashFinder, IUserInputGetter userInput)
        {
            _hashFinder = hashFinder ?? throw new ArgumentNullException(nameof(_hashFinder));
            _userInput = userInput ?? throw new ArgumentNullException(nameof(_userInput));
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
                            .SingleOrDefault(link => link.LinkFormat == _userInput.GetUserInputLink(_hashFinder));

                if (linkEntity == null)
                    throw new Exception("Ссылка отсутствует или была удалена...");

                linkEntity.PopularityPoint += point;
                context.SaveChanges();
            }
        }
    }

    public sealed class FileTextShowing : IShowFileText
    {
        private readonly IHashFinder _hashFinder;
        private readonly IUserInputGetter _userInput;

        public FileTextShowing(IHashFinder hashFinder, IUserInputGetter userInput)
        {
            _hashFinder = hashFinder ?? throw new ArgumentNullException(nameof(_hashFinder));
            _userInput = userInput ?? throw new ArgumentNullException(nameof(_userInput));
        }

        public void ShowFileText(TextBox textBox, IDriveExtractFile extarctText, IDriveTextShowing textShowing)
        {
            using (LinkBaseDbContext context = new LinkBaseDbContext())
            {
                var linkEntity = context.BaseLinks
                            .SingleOrDefault(link => link.LinkFormat == _userInput.GetUserInputLink(_hashFinder));

                if (linkEntity == null)
                    throw new Exception("Ссылка отсутствует или была удалена...");

                string userFileId = extarctText.ExtractFileId(linkEntity.LinkFormat);

                textBox.Text = textShowing.ShowText(userFileId);
            }
        }
    }

    public sealed class UserInputGetter : IUserInputGetter
    {
        public string GetUserInputLink(IHashFinder hashFinder)
        {
            ShareText form = FormCreater.GetForm();
            IGenerateLink generateLink = new LinkGenerator(form.UserInputLink.Text);

            return hashFinder.HashLinkFinder(generateLink);
        }
    }

    public sealed class HashFinder : IHashFinder
    {
        public string HashLinkFinder(IGenerateLink generateLink)
        {
            using (LinkHashDbContext context = new LinkHashDbContext())
            {
                string hashLink = generateLink.GenerateSHA256Hash();

                HashLinkEntity hashLinkEntity = context.HashLinks
                    .SingleOrDefault(link => link.LinkFormat == hashLink) ?? throw new Exception("Ссылка не найдена...");

                if (hashLinkEntity != null)
                    return generateLink.GenerateBaseLink();

#pragma warning disable CS8603 
                return null;
#pragma warning restore CS8603 
            }
        }
    }

    public sealed class DatabaseLinkAdder : IDatabaseLinkAdder
    {
        public void AddBaseLink()
        {
            using (LinkBaseDbContext context = new LinkBaseDbContext())
            {
                BaseLinkEntity linkEntity = new BaseLinkEntity();
                context.BaseLinks.Add(linkEntity);
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
    }

    public sealed class DatabaseLinkRemover : IDatabaseLinkRemover
    {
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
    }

    public sealed class DatabaseInitializator : IDatabaseInitializator
    {
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
    }

    public sealed class FormCreater
    {
#pragma warning disable CS8618 
        private static ShareText _form;
#pragma warning restore CS8618

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
        public void ShowPopularityLinks(ListView listView)
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

