using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [StringLength(100)]
    public string Password { get; set; } // Не забудьте использовать хэширование паролей!
}

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public decimal Price { get; set; }

    // Навигационное свойство
    public ICollection<Comment> Comments { get; set; }
}

public class Comment
{
    public int Id { get; set; }

    public int ProductId { get; set; } // Внешний ключ

    [Required]
    public string Content { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; } // Оценка от 1 до 5

    // Навигационное свойство
    public Product Product { get; set; }
}