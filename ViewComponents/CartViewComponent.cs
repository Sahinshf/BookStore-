using BookStore.Context;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public CartViewComponent(UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var cartItemsCount = 0;

            if (user is not null)
            {
                var userId = user.Id;

                var cart = await _appDbContext.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                cartItemsCount = cart?.CartItems.Count() ?? 0;
            }


            return View(cartItemsCount);
        }
    }
}
