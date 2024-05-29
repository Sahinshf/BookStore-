using BookStore.Areas.Admin.ViewModels.Author;
using BookStore.Models;
using BookStore.Utils;
using BookStore.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers;

[Area("Admin")]
public class AuthorController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AuthorController(IWebHostEnvironment webHostEnvironment, AppDbContext context)
    {
        _webHostEnvironment = webHostEnvironment;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var authors = await _context.Authors.Where(a => !a.IsDeleted).ToListAsync();

        return View(authors);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AuthorViewModel authorViewModel)
    {

        if (!ModelState.IsValid) return View();

        if (authorViewModel.Image == null) { ModelState.AddModelError("Image", "Select Image"); return View(); }
        if (!authorViewModel.Image.CheckFileType("image"))
        {
            ModelState.AddModelError("Image", "File Type Must be Image.");
            return View();
        }


        string filename = $"{Guid.NewGuid()}-{authorViewModel.Image.FileName}";
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "author", filename);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            await authorViewModel.Image.CopyToAsync(stream);
        }

        Author author = new()
        {
            Name = authorViewModel.Name,
            Image = filename,
            Description = authorViewModel.Description,
            IsDeleted = false
        };

        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        Author ?author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
        if (author == null) return BadRequest();

        AuthorViewModel authorViewModel = new()
        {
            AuthorId = author.Id,
            Name = author.Name,
            Description = author.Description,
            ImageString = author.Image
        };

        return View(authorViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update (int id, AuthorViewModel authorViewModel )
    {
        if (!ModelState.IsValid) return View();

        Author ?author = await _context.Authors.FirstOrDefaultAsync( x => x.Id == id);
        if (author == null) return BadRequest();

        if (authorViewModel.Image != null)
        {
            if (!authorViewModel.Image.CheckFileType("image"))
            {
                ModelState.AddModelError("Image", "File Type Must be Image.");
                return View();
            }
            var oldPath = Path.Combine(_webHostEnvironment.WebRootPath,"assets", "img", "author", author.Image);
            FileService.DeleteFile(oldPath);

            var filename = $"{Guid.NewGuid()} - {authorViewModel.Image.FileName}";
            var newPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "author", filename);

            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await authorViewModel.Image.CopyToAsync(stream);
            }

            author.Image = filename;
        }

        author.Name = authorViewModel.Name;
        author.Description = authorViewModel.Description;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        Author ?author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
        if (author is null) return BadRequest();

        author.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        Author ?author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
        if (author is null) return BadRequest();

        return View(author);
    }
}
