using Microsoft.EntityFrameworkCore;
using RatingMusciAPI.Models;

namespace RatingMusciAPI.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Song>? Songs { get; set; }
    public DbSet<Album>? Albums { get; set; }
    public DbSet<Artist>? Artists { get; set; }
}
