using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        int id = 5555;
        int amount = 13000;

        string secretWord = "word";

        Order order = new Order(id, amount);

        IHashComputer hashSHA1 = new Sha1HashComputer();
        IHashComputer hashMD5 = new Md5HashComputer();

        ICreatePaylink paymentSystem1 = new PaymentSystem1(hashMD5);
        ICreatePaylink paymentSystem2 = new PaymentSystem2(hashMD5);
        ICreatePaylink paymentSystem3 = new PaymentSystem3(hashSHA1, secretWord);

        Console.WriteLine(paymentSystem1.CreatePaylink(order));
        Console.WriteLine(paymentSystem2.CreatePaylink(order));
        Console.WriteLine(paymentSystem3.CreatePaylink(order));
    }
}

public interface IHashComputer
{
    string ComputeHash(string input);
}

public interface ICreatePaylink
{
    public string CreatePaylink(Order order);
}

public class Order
{
    private readonly int _id;
    private readonly int _amount;

    public Order(int id, int amount)
    {
        _id = id;
        _amount = amount;
    }

    public string GetId()
    {
        return _id.ToString();
    }

    public string GetAmount()
    {
        return _amount.ToString();
    }
}

class PaymentSystem1 : ICreatePaylink
{
    private IHashComputer _hashComputer;

    public PaymentSystem1(IHashComputer hashComputer)
    {
        _hashComputer = hashComputer ?? throw new NullReferenceException(nameof(_hashComputer));
    }

    public string CreatePaylink(Order order)
    {
        string hash = _hashComputer.ComputeHash(order.GetId());

        return $"pay.system1.ru/order?amount={order.GetAmount()}RUB&hash={hash}";
    }
}

class PaymentSystem2 : ICreatePaylink
{
    private IHashComputer _hashComputer;

    public PaymentSystem2(IHashComputer hashComputer)
    {
        _hashComputer = hashComputer ?? throw new NullReferenceException(nameof(_hashComputer));
    }

    public string CreatePaylink(Order order)
    {
        string hash = _hashComputer.ComputeHash(string.Concat(order.GetId(), order.GetAmount()));

        return $"order.system2.ru/pay?hash={hash}";
    }
}

class PaymentSystem3 : ICreatePaylink
{
    private IHashComputer _hashComputer;
    private string _secretWord;

    public PaymentSystem3(IHashComputer hashComputer, string secretWord)
    {
        _hashComputer = hashComputer ?? throw new NullReferenceException(nameof(_hashComputer));
        _secretWord = secretWord ?? throw new NullReferenceException(nameof(secretWord));
    }

    public string CreatePaylink(Order order)
    {
        string hash = _hashComputer.ComputeHash(string.Concat(order.GetAmount(), order.GetId(), _secretWord));

        return $"system3.com/pay?amount={order.GetAmount()}&curency=RUB&hash={hash}";
    }
}

class Sha1HashComputer : IHashComputer
{
    public string ComputeHash(string value)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(value));
            StringBuilder hash = new StringBuilder();
            foreach (var bytes in data)
                hash.Append(bytes.ToString("x2"));
            return hash.ToString();
        }
    }
}

class Md5HashComputer : IHashComputer
{
    public string ComputeHash(string value)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            StringBuilder hash = new StringBuilder();
            foreach (var bytes in data)
                hash.Append(bytes.ToString("x2"));
            return hash.ToString();
        }
    }
}