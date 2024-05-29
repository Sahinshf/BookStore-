using BookStore.Models.Common;
namespace BookStore.Models;

public class Book : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Image { get; set; } = null!;
    public int StockQuantity { get; set; }
    public int SoldQuantity { get; set; }
    public decimal Price { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
}
