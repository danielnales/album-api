using Microsoft.EntityFrameworkCore;
using Album.Api.Models;
using Album.Api.Data;

namespace Album.Api.Services
{
   public interface IAlbumService
   {
      Task<IEnumerable<AlbumModel>> GetAllAlbums();
      bool AlbumExists(int id);
      Task<AlbumModel> GetAlbumById(int id);
      Task<AlbumModel> CreateAlbum(AlbumModel newAlbum);
      Task UpdateAlbum(AlbumModel updatedAlbum);
      Task DeleteAlbum(int id);
   }

   public class AlbumService : IAlbumService
   {
      private readonly AlbumContext _context;

      public AlbumService(AlbumContext context)
      {
         _context = context;
      }

      public async Task<IEnumerable<AlbumModel>> GetAllAlbums()
      {
         return await _context.Albums.ToListAsync();
      }

      public bool AlbumExists(int id)
      {
         return _context.Albums.Any(e => e.Id == id);
      }

      public async Task<AlbumModel> GetAlbumById(int id)
      {
         return await _context.Albums.FindAsync(id);
      }

      public async Task<AlbumModel> CreateAlbum(AlbumModel newAlbum)
      {
         if (newAlbum == null) throw new ArgumentNullException(nameof(newAlbum));

         _context.Albums.Add(newAlbum);
         await _context.SaveChangesAsync(); 
         return newAlbum;
      }

      public async Task UpdateAlbum(AlbumModel updatedAlbum)
      {
         _context.Entry(updatedAlbum).State = EntityState.Modified;
         await _context.SaveChangesAsync();
      }

      public async Task DeleteAlbum(int id)
      {
         var album = await _context.Albums.FindAsync(id);
         _context.Albums.Remove(album);
         await _context.SaveChangesAsync();
      }
   }
}