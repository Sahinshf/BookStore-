using BookStore.Areas.Admin.ViewModels.Book;
using BookStore.Context;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Utils;
using BookStore.Areas.Admin.ViewModels.Author;

namespace BookStore.Areas.Admin.Controllers;

[Area("Admin")]
public class BookController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public BookController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        List<Book> book = await _context.Books.Where(b => !b.IsDeleted).ToListAsync();

        return View(book);
    }

    public async Task<IActionResult> Create()
    {
        var viewModel = new BookViewModel
        {
            Authors = await _context.Authors.ToListAsync() // Fetch authors from your repository or database
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookViewModel bookViewModel)
    {

        if (!ModelState.IsValid) return View();

        if (bookViewModel.Image == null) { ModelState.AddModelError("Image", "Select Image"); return View(); }
        if (!bookViewModel.Image.CheckFileType("image"))
        {
            ModelState.AddModelError("Image", "File Type Must be Image.");
            return View();
        }

        string filename = $"{Guid.NewGuid()}-{bookViewModel.Image.FileName}";
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "book", filename);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            await bookViewModel.Image.CopyToAsync(stream);
        }

        Book book = new()
        {
            Title = bookViewModel.Title,
            Image = filename,
            Description = bookViewModel.Description,
            StockQuantity = bookViewModel.StockQuantity,
            Price = bookViewModel.Price,
            SoldQuantity = 0,
            AuthorId = bookViewModel.SelectedAuthorId,
            IsDeleted = false
        };

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }



    public async Task<IActionResult> Update(int id)
    {
        Book? book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
        if (book == null) return BadRequest();

        BookViewModel bookViewModel = new()
        {
            Title = book.Title,
            Description = book.Description,
            StockQuantity = book.StockQuantity,
            Price = book.Price,
            SelectedAuthorId = book.AuthorId,
            Authors = await _context.Authors.ToListAsync(),
            ImageString = book.Image
        };

        return View(bookViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, BookViewModel bookViewModel)
    {
        Book? book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
        if (book == null) return BadRequest();
        BookViewModel bookViewModelIsValid = new()
        {
            Title = book.Title,
            Description = book.Description,
            StockQuantity = book.StockQuantity,
            Price = book.Price,
            SelectedAuthorId = book.AuthorId,
            Authors = await _context.Authors.ToListAsync(),
            ImageString = book.Image
        };
        if (!ModelState.IsValid) return View(bookViewModelIsValid);

        if (bookViewModel.Image != null)
        {
            if (!bookViewModel.Image.CheckFileType("image"))
            {
                ModelState.AddModelError("Image", "File Type Must be Image.");
                return View();
            }
            var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "book", book.Image);
            FileService.DeleteFile(oldPath);

            var filename = $"{Guid.NewGuid()} - {bookViewModel.Image.FileName}";
            var newPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "book", filename);

            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await bookViewModel.Image.CopyToAsync(stream);
            }

            book.Image = filename;
        }

        book.Title = bookViewModel.Title;
        book.Description = bookViewModel.Description;
        book.Price = bookViewModel.Price;
        book.StockQuantity = bookViewModel.StockQuantity;
        book.AuthorId = bookViewModel.SelectedAuthorId;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }



    public async Task<IActionResult> Details(int id)
    {
        Book? book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(a => a.Id == id);
        if (book is null) return BadRequest();

        return View(book);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        Book? book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
        if (book is null) return BadRequest();

        book.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
