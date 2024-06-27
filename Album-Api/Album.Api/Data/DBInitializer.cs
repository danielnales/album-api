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
            new AlbumModel { Id = 1, Name="Dog of Wisdom", Artist="Joe", ImageUrl="https://www.google.com/imgres?q=dog%20of%20wisdom&imgurl=https%3A%2F%2Fm.media-amazon.com%2Fimages%2FM%2FMV5BMTQwYmM2OWQtNWZmOS00OWZhLWE1YzYtMWQyYTk3MzhiOGYzXkEyXkFqcGdeQXVyMjQxNzM0MjI%40._V1_.jpg&imgrefurl=https%3A%2F%2Fwww.imdb.com%2Ftitle%2Ftt10208550%2F&docid=tSkrLs0QVJ0GiM&tbnid=suPM4q5IS7Wa_M&vet=12ahUKEwi41fn88vuGAxWMgP0HHfDcCLAQM3oECBUQAA..i&w=1280&h=721&hcb=2&ved=2ahUKEwi41fn88vuGAxWMgP0HHfDcCLAQM3oECBUQAA" },
            new AlbumModel { Id = 2, Name="Hond van Wijsheid", Artist="Johan", ImageUrl="https://www.google.com/imgres?q=dog%20of%20wisdom&imgurl=https%3A%2F%2Fi.ytimg.com%2Fvi%2FHKCWMq-Poxo%2Fsddefault.jpg&imgrefurl=https%3A%2F%2Fm.youtube.com%2Fwatch%3Fv%3DHKCWMq-Poxo&docid=v3q6xLAICNK8ZM&tbnid=8VGeTdWiIIWbOM&vet=12ahUKEwi41fn88vuGAxWMgP0HHfDcCLAQM3oECGIQAA..i&w=640&h=480&hcb=2&ved=2ahUKEwi41fn88vuGAxWMgP0HHfDcCLAQM3oECGIQAA" }
         };

         foreach (AlbumModel album in albums) {
            context.Albums.Add(album);
         }

         context.SaveChanges();
      }
   }
}