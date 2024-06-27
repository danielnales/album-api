using Album.Api.Models;

namespace Album.Api.Data
{
   public static class DBInitializer
   {
      public static void Initialize(AlbumContext context) 
      {
         context.Database.EnsureCreated();
         
         if (context.Albums.Any()) {
            return;
         }

         var albums = new AlbumModel[]
         {
            new AlbumModel { Id = 1, Name="Dog of Wisdom", Artist="Joe", ImageUrl="https://m.media-amazon.com/images/M/MV5BMTQwYmM2OWQtNWZmOS00OWZhLWE1YzYtMWQyYTk3MzhiOGYzXkEyXkFqcGdeQXVyMjQxNzM0MjI@._V1_FMjpg_UX1000_.jpg" },
            new AlbumModel { Id = 2, Name="Hond van Wijsheid", Artist="Johan", ImageUrl="https://cdn.britannica.com/13/234213-050-45F47984/dachshund-dog.jpg" }
         };

         foreach (AlbumModel album in albums) {
            context.Albums.Add(album);
         }

         context.SaveChanges();
      }
   }
}