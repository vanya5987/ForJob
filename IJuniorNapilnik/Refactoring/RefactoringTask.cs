using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace WindowsFormsApp1
{
    public partial class Presenter : Form
    {
        private readonly PassportViewModel _viewModel;
        private readonly Label _textResult;
        public TextBox PassportTextbox { get; private set; }

        public Presenter()
        {
            InitializeComponent();
            _viewModel = new PassportViewModel();
        }

        private void CheckButtonClick(object sender, EventArgs e)
        {
            string passportNumber = PassportTextbox.Text;
            string resultMessage = _viewModel.CheckPassport(passportNumber);
            DisplayResultMessage(resultMessage);
        }

        private void DisplayResultMessage(string message)
        {
            _textResult.Text = message;
        }
    }

    public class PassportViewModel
    {
        private readonly PassportService _passportService;

        public PassportViewModel()
        {
            _passportService = new PassportService();
        }

        public string CheckPassport(string passportNumber)
        {
            try
            {
                Passport passport = new Passport(passportNumber);
                bool accessStatus = _passportService.CheckPassport(passport);
                return accessStatus
                    ? $"По паспорту «{passportNumber}» доступ к бюллетеню на дистанционном электронном голосовании ПРЕДОСТАВЛЕН"
                    : $"По паспорту «{passportNumber}» доступ к бюллетеню на дистанционном электронном голосовании НЕ ПРЕДОСТАВЛЯЛСЯ";
            }
            catch (PassportNotFoundException exception)
            {
                return exception.Message;
            }
            catch (FormatException exception)
            {
                return exception.Message;
            }
        }
    }

    public class DatabaseContext
    {
        private string GetConnectionString()
        {
            return $"Data Source={Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\db.sqlite";
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(GetConnectionString());
        }
    }
    public class Repository
    {
        private readonly DatabaseContext _databaseContext;

        public Repository()
        {
            _databaseContext = new DatabaseContext();
        }

        public DataTable GetPassportData(string passportNumber)
        {
            using (SQLiteConnection connection = _databaseContext.GetConnection())
            {
                connection.Open();
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(new SQLiteCommand(GetCommandText(passportNumber), connection)))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        private string GetCommandText(string passportNumber)
        {
            return $"SELECT * FROM passports WHERE num='{ComputeSha256Hash(passportNumber)}' LIMIT 1;";
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder hash = new StringBuilder();
                foreach (var bytes in data)
                    hash.Append(bytes.ToString("x2"));
                return hash.ToString();
            }
        }
    }

    public class PassportService
    {
        private readonly Repository _repository;

        public PassportService()
        {
            _repository = new Repository();
        }

        public bool CheckPassport(Passport passport)
        {
            DataTable result = _repository.GetPassportData(passport.Number);
            if (result.Rows.Count == 0)
            {
                throw new PassportNotFoundException(passport.Number);
            }

            return Convert.ToBoolean(result.Rows[0].ItemArray[1]);
        }
    }

    public class PassportNotFoundException : Exception
    {
        public PassportNotFoundException(string passportNumber)
            : base($"Паспорт «{passportNumber}» в списке участников дистанционного голосования НЕ НАЙДЕН")
        {
        }
    }

    public class Passport
    {
        private const int PassportDigitsCount = 10;

        public string Number { get; private set; }

        public Passport(string passportNumber)
        {
            if (passportNumber.Length < PassportDigitsCount)
                throw new FormatException("Неверный формат серии или номера паспорта!");

            if (string.IsNullOrWhiteSpace(passportNumber))
                throw new FormatException("Данные паспорта неверны!");

            Number = passportNumber.Trim().Replace(" ", string.Empty); ;
        }
    }
}
