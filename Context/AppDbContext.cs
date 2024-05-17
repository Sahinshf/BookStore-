using BookStore.Models;
using LoginRegisterProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoginRegisterProject.Context;

public class AppDbContext : IdentityDbContext <AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options ) : base( options ) {    }

     public DbSet<Book> Books { get; set; }
     public DbSet<Author> Authors { get; set; }
     public DbSet<Contact> Contacts { get; set; }
     public DbSet<About> About { get; set; }
}
