using entity_library;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Person> Persons { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Coments { get; set; }
    public DbSet<Interaction> Interactions { get; set; }
    public DbSet<Following> Followings { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Image> Images { get; set; }

}