using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetTopProductsAsync(int count);
    Task AddCommentAsync(Comment comment);
}

public class ProductRepository : IProductRepository
{
    private readonly ShopDbContext _context;

    public ProductRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetTopProductsAsync(int count)
    {
        return await _context.Products
            .Include(p => p.Comments)
            .OrderByDescending(p => p.Comments.Average(c => c.Rating)) // Топ по средней оценке
            .Take(count)
            .ToListAsync();
    }

    public async Task AddCommentAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }
}