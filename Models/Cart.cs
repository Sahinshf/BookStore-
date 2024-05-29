using BookStore.Models.Common;

namespace BookStore.Models;

public class Cart : BaseEntity
{
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public ICollection<CartItem> CartItems { get; set; }
    public Cart()
    {
        CartItems = new List<CartItem>();
    }
}
