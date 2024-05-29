using BookStore.Context;
using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace LoginRegisterProject.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Author> authors = await _context.Authors.Where(a => !a.IsDeleted).Take(8).ToListAsync();
        List<Book> books = await _context.Books.Where(a => !a.IsDeleted).Take(4).ToListAsync();

        HomeViewModel viewModel = new()
        {
            Authors = authors,
            Books = books,
        };

        return View(viewModel);
    }

    public IActionResult Success()
    {
        return View();
    }
}
