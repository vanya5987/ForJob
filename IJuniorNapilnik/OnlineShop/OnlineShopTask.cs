namespace ulearn_1

{
    internal class Program
    {
        public static void Main()
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Deliver(iPhone12, 10);
            warehouse.Deliver(iPhone11, 1);

            // Вывод всех товаров на складе с их остатком
            Console.WriteLine("Товары на складе:");

            foreach (var item in warehouse.GetItems())
            {
                Console.WriteLine($"{item.Key._productName}: {item.Value}");
            }

            Cart cart = shop.Cart();

            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); // Возникает ошибка, так как нет нужного количества товара на складе

            // Вывод всех товаров в корзине
            Console.WriteLine("\nТовары в корзине:");
            foreach (var item in cart.Order().GetItems())
            {
                Console.WriteLine($"{item.Key._productName}: {item.Value}");
            }

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9); // Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    public class Good
    {
        public string _productName { get; private set; }

        public Good(string name)
        {
            _productName = name;
        }
    }

    public class Warehouse : IGetProduct, IAccessToWarehouse
    {
        private readonly Dictionary<Good, int> _items = new Dictionary<Good, int>();

        public void Deliver(Good product, int productCount)
        {
            if (product == null || productCount < 0)
                throw new Exception("Данные введены некоректно!");

            if (_items.ContainsKey(product) == false)
                _items[product] = 0;
            _items[product] += productCount;
        }

        public bool CheckAvailability(Good product, int productCount)
        {
            if (product == null || productCount < 0)
                throw new Exception("Данные введены некоректно!");

            return (_items.ContainsKey(product) && _items[product] >= productCount);
        }

        public void Remove(Good product, int productCount)
        {
            if (product == null || productCount < 0)
                throw new Exception("Данные введены некоректно!");

            if (_items.ContainsKey(product))
                _items[product] -= productCount;
        }

        public Dictionary<Good, int> GetItems()
        {
            return new Dictionary<Good, int>(_items);
        }
    }

    public class Cart
    {
        private readonly Dictionary<Good, int> _items = new Dictionary<Good, int>();
        private readonly IAccessToWarehouse AccessToWarehouse;

        public Cart(IAccessToWarehouse accessToWarehouse)
        {
            AccessToWarehouse = accessToWarehouse;
        }

        public void Add(Good product, int productCount)
        {
            if (product == null || productCount < 0)
                throw new Exception("Данные введены некоректно!");

            if (!AccessToWarehouse.CheckAvailability(product, productCount))
                throw new Exception($"Ошибка! Недостаточно товара \"{product._productName}\" на складе.");

            if (AccessToWarehouse.CheckAvailability(product, productCount))
            {
                if (_items.ContainsKey(product))
                    _items[product] += productCount;
                else
                    _items.Add(product, productCount);
            }
        }

        public Order Order()
        {
            foreach (var item in _items)
                AccessToWarehouse.Remove(item.Key, item.Value);

            return new Order(_items);
        }
    }

    public class Order : IGetProduct
    {
        private readonly Dictionary<Good, int> _items;

        public string Paylink;

        public Order(Dictionary<Good, int> items)
        {
            _items = items;
            Paylink = GeneratePaylink();
        }

        private string GeneratePaylink()
        {
            return "Ссылка для оплаты";
        }

        public Dictionary<Good, int> GetItems()
        {
            return new Dictionary<Good, int>(_items);
        }
    }

    public class Shop
    {
        private Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            return new Cart(_warehouse);
        }
    }

    public interface IAccessToWarehouse
    {
        public bool CheckAvailability(Good product, int productCount);

        public void Remove(Good product, int productCount);
    }

    interface IGetProduct
    {
        public Dictionary<Good, int> GetItems();
    }
}