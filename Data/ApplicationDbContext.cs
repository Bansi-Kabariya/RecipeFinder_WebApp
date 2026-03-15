using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    public DbSet<Categories> Categories { get; set; }

    public DbSet<Recipes> Recipes { get; set; }

    public DbSet<Ingredients> Ingredients { get; set; }

    public DbSet<Reviews> Reviews { get; set; }

    public DbSet<Wishlist> Wishlist { get; set; }

    public DbSet<Contact> Contacts { get; set; }

    public DbSet<Save> SaveFolders { get; set; }
}