namespace OnlineShopModel
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var context = new ShopDbContext();
            context.Database.EnsureCreated(); 

            var authService = new AuthService(context);
            var productRepository = new ProductRepository(context);

            try
            {
                authService.Register("user1", "password1");
                Console.WriteLine("Пользователь зарегистрирован!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                authService.Login("user1", "password1");
                Console.WriteLine("Успешный вход!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var topProducts = await productRepository.GetTopProductsAsync(5);
            foreach (var product in topProducts)
            {
                Console.WriteLine($"Товар: {product.Name}");
            }
        }
    }
}