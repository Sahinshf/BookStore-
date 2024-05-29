using System.ComponentModel.DataAnnotations;
namespace BookStore.Areas.Admin.ViewModels.Book;

public class BookViewModel
{
    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    public IFormFile? Image { get; set; } = null!;

    [Required]
    public int StockQuantity { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int SelectedAuthorId { get; set; }

    public string ImageString { get; set; } = string.Empty;

    public List<BookStore.Models.Author> Authors { get; set; } = null!;
    public BookViewModel()
    {
        Authors = new List<BookStore.Models.Author>();
    }
}
