public class AuthService
{
    private readonly ShopDbContext _context;
    private int _loginAttempts;

    public AuthService(ShopDbContext context)
    {
        _context = context;
        _loginAttempts = 0;
    }

    public bool Register(string username, string password)
    {
        if (_context.Users.Any(u => u.Username == username))
            throw new Exception("Пользователь с таким именем уже существует");

        var user = new User { Username = username, Password = password };
        _context.Users.Add(user);
        _context.SaveChanges();
        return true;
    }

    public bool Login(string username, string password)
    {
        if (_loginAttempts >= 3)
            throw new Exception("Превышено количество попыток входа. Попробуйте позже.");

        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            _loginAttempts++;
            throw new Exception("Неверное имя пользователя или пароль");
        }

        _loginAttempts = 0;
        return true;
    }
}