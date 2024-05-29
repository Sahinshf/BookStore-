using BookStore.Models;

namespace BookStore.ViewModels;

public class HomeViewModel
{
    public List<Author> Authors { get; set; }
    public List<Book> Books { get; set; }
    public int CartItemsCount { get; set; }

    public HomeViewModel()
    {
        Books = new List<Book>();
        Authors = new List<Author>();
    }

}
