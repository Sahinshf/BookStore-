using BookStore.Context;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginRegisterProject.Controllers;

public class AuthorController : Controller
{
    private readonly AppDbContext _context;

    public AuthorController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Author> authors = await _context.Authors.Where(a => !a.IsDeleted).ToListAsync();
        return View(authors);
    }

    public async Task<IActionResult> Details(int id)
    {
        Author? author = await _context.Authors.Include(a => a.Books.Where(b => !b.IsDeleted)).FirstOrDefaultAsync(a => a.Id == id);
        if (author is null) return BadRequest();

        return View(author);
    }
}
