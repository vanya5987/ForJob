class Program
{
    static void Main(string[] args)
    {
        ILogger consoleLogger = new ConsoleLogger();
        ILogger fileLogger = new FileLogger("log.txt");
        ILogger fridayFileLogger = new SecureLogger(fileLogger, DayOfWeek.Friday);
        ILogger fridayConsoleLogger = new SecureLogger(consoleLogger, DayOfWeek.Friday);
        ILogger hybridLogger = new HybridLogger([consoleLogger, fridayConsoleLogger]);

        Pathfinder pathfinder1 = new Pathfinder(fileLogger);
        Pathfinder pathfinder2 = new Pathfinder(consoleLogger);
        Pathfinder pathfinder3 = new Pathfinder(fridayFileLogger);
        Pathfinder pathfinder4 = new Pathfinder(fridayConsoleLogger);
        Pathfinder pathfinder5 = new Pathfinder(hybridLogger);

        pathfinder1.Find();
        pathfinder2.Find();
        pathfinder3.Find();
        pathfinder4.Find();
        pathfinder5.Find();
    }
}

public interface ILogger
{
    void WriteLog(string message);
}

public class ConsoleLogger : ILogger
{
    public void WriteLog(string message)
    {
        Console.WriteLine(message);
    }
}

public class FileLogger : ILogger
{
    private string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void WriteLog(string message)
    {
        File.WriteAllText(_filePath, message);
    }
}

public class SecureLogger : ILogger
{
    private readonly ILogger _baseLogger;
    private readonly DayOfWeek _dayOfWeek;

    public SecureLogger(ILogger baseLogger, DayOfWeek dayOfWeek)
    {
        if (_baseLogger == null)
            throw new ArgumentNullException();

        _baseLogger = baseLogger ?? throw new ArgumentNullException(nameof(baseLogger), "—сылка на объект отсутствует!");
        _dayOfWeek = dayOfWeek;
    }

    public void WriteLog(string message)
    {
        if (DateTime.Now.DayOfWeek == _dayOfWeek)
        {
            _baseLogger.WriteLog(message);
        }
    }
}

public class HybridLogger : ILogger
{
    private readonly IEnumerable<ILogger> _loggers;

    public HybridLogger(IEnumerable<ILogger> loggers)
    {
        _loggers = loggers ?? throw new ArgumentNullException(nameof(loggers), "—сылка на объект отсутствует!");
    }

    public void WriteLog(string message)
    {
        foreach (var logger in _loggers)
            logger.WriteLog(message);
    }
}

public class Pathfinder
{
    private readonly ILogger _logger;

    public Pathfinder(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), "—сылка на объект отсутствует!");
    }

    public void Find()
    {
        _logger.WriteLog("ѕишу лог...");
    }
}