using System.ComponentModel.DataAnnotations;

namespace BookStore.Areas.Admin.ViewModels.Author;

public class AuthorViewModel
{
    public int AuthorId { get; set; }
    public string ImageString { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }

    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }

}
