using BookStore.Models.Common;
namespace BookStore.Models;

public class Author : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Image { get; set; } = null!;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
