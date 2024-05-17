using LoginRegisterProject.Models.Common;

namespace BookStore.Models;

public class About : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}
