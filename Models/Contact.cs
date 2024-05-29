using BookStore.Models.Common;

namespace BookStore.Models;

public class Contact : BaseEntity
{
    public string Mail { get; set; } = null!;
    public string Number { get; set; } = null!;
    public string Address { get; set; } = null!;
}
