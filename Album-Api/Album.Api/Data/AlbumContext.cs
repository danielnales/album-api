using Microsoft.EntityFrameworkCore;
using Album.Api.Models;

namespace Album.Api.Data
{
   public class AlbumContext : DbContext
   {
      public AlbumContext(DbContextOptions<AlbumContext> options) : base(options) { }
      public DbSet<AlbumModel> Albums { get; set; }
   }
}