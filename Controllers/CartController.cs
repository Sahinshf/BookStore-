using BookStore.Context;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginRegisterProject.Controllers;

public class CartController : Controller
{

    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public CartController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var cart = await GetCartAsync(user.Id);
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int bookId, int quantity)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var cart = await GetCartAsync(user.Id);
        var cartItem = cart.CartItems.FirstOrDefault(i => i.BookId == bookId);

        if (cartItem == null)
        {
            cartItem = new CartItem { BookId = bookId, Quantity = quantity };
            cart.CartItems.Add(cartItem);
        }
        else
        {
            cartItem.Quantity += quantity;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Cart");
    }


    [HttpPost]
    public async Task<IActionResult> UpdateCartItem(int bookId, int quantity)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var cart = await GetCartAsync(user.Id);
        var cartItem = cart.CartItems.FirstOrDefault(i => i.BookId == bookId);
        if (cartItem != null)
        {
            // Check if the quantity is being incremented or decremented
            if (quantity > 0)
            {
                cartItem.Quantity = quantity;
            }
            else if (cartItem.Quantity > 1) // Ensure quantity doesn't go below 1
            {
                cartItem.Quantity = quantity;
            }
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Cart");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int bookId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var cart = await GetCartAsync(user.Id);
        var cartItem = cart.CartItems.FirstOrDefault(i => i.BookId == bookId);
        if (cartItem != null)
        {
            cart.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Cart");
    }

    public async Task<IActionResult> ConfirmCheckout()
    {
        AppUser user = await _userManager.GetUserAsync(User);
        if (user == null) return BadRequest();

        Cart cart = await GetCartAsync(user.Id);
        if (cart != null)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        return Redirect(nameof(CheckoutConfirmation));
    }

    public IActionResult CheckoutConfirmation()
    {
        return View();
    }

    private async Task<Cart> GetCartAsync(string userId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(i => i.Book)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null )
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }
}
