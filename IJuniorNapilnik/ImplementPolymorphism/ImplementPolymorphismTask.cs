namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            PaymentSystemFactory paymentSystemFactory = new PaymentSystemFactory();
            IEnumerable<string> availableSystems = paymentSystemFactory.GetAvailableSystems;

            OrderForm orderForm = new OrderForm(availableSystems);
            string systemId = orderForm.ShowForm();

            IPaymentSystemFactory iPaymentSystemFactory = paymentSystemFactory.GetFactory(systemId);
            PaymentHandler paymentHandler = new PaymentHandler(iPaymentSystemFactory);

            paymentHandler.HandlePayment();
            paymentHandler.ShowPaymentResult(systemId);
        }
    }

    public interface IPaymentSystem
    {
        void Redirect();
        void HandlePayment();
        void ShowPaymentResult(string systemId);
    }

    public class OrderForm
    {
        private readonly IEnumerable<string> _availableSystems;

        public OrderForm(IEnumerable<string> availableSystems)
        {
            _availableSystems = availableSystems;
        }

        public string ShowForm()
        {
            Console.WriteLine("Мы принимаем: " + string.Join(", ", _availableSystems));
            Console.WriteLine("Какой системой вы хотите совершить оплату?");
            return Console.ReadLine();
        }
    }

    public class PaymentHandler
    {
        private readonly IPaymentSystem _paymentSystem;

        public PaymentHandler(IPaymentSystemFactory factory)
        {
            _paymentSystem = factory.Create();
        }

        public void HandlePayment()
        {
            _paymentSystem.Redirect();
            _paymentSystem.HandlePayment();
        }

        public void ShowPaymentResult(string systemId)
        {
            _paymentSystem.ShowPaymentResult(systemId);
        }
    }

    public class QIWI : IPaymentSystem
    {
        public void Redirect()
        {
            Console.WriteLine("Перевод на страницу QIWI...");
        }

        public void HandlePayment()
        {
            Console.WriteLine("Проверка платежа через QIWI...");
        }

        public void ShowPaymentResult(string systemId)
        {
            Console.WriteLine($"Вы оплатили с помощью {systemId}");
            Console.WriteLine("Оплата прошла успешно!");
        }
    }

    public class WebMoney : IPaymentSystem
    {
        public void Redirect()
        {
            Console.WriteLine("Вызов API WebMoney...");
        }

        public void HandlePayment()
        {
            Console.WriteLine("Проверка платежа через WebMoney...");
        }

        public void ShowPaymentResult(string systemId)
        {
            Console.WriteLine($"Вы оплатили с помощью {systemId}");
            Console.WriteLine("Оплата прошла успешно!");
        }
    }

    public class Card : IPaymentSystem
    {
        public void Redirect()
        {
            Console.WriteLine("Вызов API банка эмитера карты...");
        }

        public void HandlePayment()
        {
            Console.WriteLine("Проверка платежа через Card...");
        }

        public void ShowPaymentResult(string systemId)
        {
            Console.WriteLine($"Вы оплатили с помощью {systemId}");
            Console.WriteLine("Оплата прошла успешно!");
        }
    }

    public interface IPaymentSystemFactory
    {
        IPaymentSystem Create();
    }

    public class QIWIFactory : IPaymentSystemFactory
    {
        public IPaymentSystem Create()
        {
            return new QIWI();
        }
    }

    public class WebMoneyFactory : IPaymentSystemFactory
    {
        public IPaymentSystem Create()
        {
            return new WebMoney();
        }
    }

    public class CardFactory : IPaymentSystemFactory
    {
        public IPaymentSystem Create()
        {
            return new Card();
        }
    }

    public class PaymentSystemFactory
    {
        private readonly Dictionary<string, IPaymentSystemFactory> _factories;

        public PaymentSystemFactory()
        {
            _factories = new Dictionary<string, IPaymentSystemFactory>
            {
                { "QIWI", new QIWIFactory() },
                { "WebMoney", new WebMoneyFactory() },
                { "Card", new CardFactory() }
            };
        }

        public IPaymentSystemFactory GetFactory(string systemId)
        {
            if (_factories.TryGetValue(systemId, out IPaymentSystemFactory factory))
                return factory;

            throw new ArgumentException("Неизвестная платёжная система");
        }

        public IEnumerable<string> GetAvailableSystems => _factories.Keys;
    }
}