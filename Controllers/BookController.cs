using BookStore.Context;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

public class BookController : Controller
{
    private readonly AppDbContext _context;

    public BookController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Book> books = await _context.Books.Where(b => !b.IsDeleted).ToListAsync();
        return View(books);
    }

    public async Task<IActionResult> Details (int id)
    {
        Book? book =await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book is null) return NotFound();

        return View(book);
    }
}
