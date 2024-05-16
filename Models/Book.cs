using LoginRegisterProject.Models.Common;

namespace LoginRegisterProject.Models;

public class Book : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Image { get; set; } = null!;
}
