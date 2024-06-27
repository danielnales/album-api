using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Net.Http.Json;
using Xunit;
using Moq;
using Xunit.Abstractions;
using Album.Api.Controllers;
using Album.Api.Services;
using Album.Api.Models;
using Album.Api.Data;

namespace Album.Api.Tests
{
   public class AlbumServiceTests : IClassFixture<InMemoryApplicationFactory>
   {
      private readonly HttpClient _client;
      private readonly ITestOutputHelper _output;

      public AlbumServiceTests(InMemoryApplicationFactory factory, ITestOutputHelper output)
      {
         _client = factory.CreateClient();
         _output = output;
      }

      [Fact]
      public async Task GetAlbums_ReturnsOkResult_WithListOfAlbums()
      {
         // Arrange
         var requestUri = "/api/Album/";
         var newAlbum = new AlbumModel
         {
               Id = 22,
               Name = "Test Album",
               Artist = "Test Artist",
               ImageUrl = "https://example.com/image.jpg"
         };

         var json = JsonConvert.SerializeObject(newAlbum);
         var content = new StringContent(json, Encoding.UTF8, "application/json");
         var createResponse = await _client.PostAsync("/api/Album/", content);

         // Act
         var response = await _client.GetAsync(requestUri);

         // Assert
         response.EnsureSuccessStatusCode();

         var albumsContent = await response.Content.ReadAsStringAsync();
         var albums = JsonConvert.DeserializeObject<List<AlbumModel>>(albumsContent);

         Assert.NotNull(albums);
         Assert.NotEmpty(albums);
      }

      [Fact]
      public async Task GetAlbumById_ReturnsOkResult_WithAlbum()
      {
         // Arrange
         var newAlbum = new AlbumModel
         {
               Id = 35,
               Name = "Test Album",
               Artist = "Test Artist",
               ImageUrl = "https://example.com/image.jpg"
         };

         var json = JsonConvert.SerializeObject(newAlbum);
         var content = new StringContent(json, Encoding.UTF8, "application/json");

         var createResponse = await _client.PostAsync("/api/Album/", content);
         createResponse.EnsureSuccessStatusCode();

         var createContent = await createResponse.Content.ReadAsStringAsync();
         var createdAlbum = JsonConvert.DeserializeObject<AlbumModel>(createContent);

         // Act
         var requestUri = $"/api/Album/{createdAlbum.Id}";

         var response = await _client.GetAsync(requestUri);
         response.EnsureSuccessStatusCode();

         var albumContent = await response.Content.ReadAsStringAsync();
         var album = JsonConvert.DeserializeObject<AlbumModel>(albumContent);

         // Assert
         Assert.NotNull(album);
         Assert.Equal(createdAlbum.Id, album.Id);
      }

      [Fact]
      public async Task GetAlbumById_ReturnsNotFound_WhenAlbumDoesNotExist()
      {
         // Arrange
         var requestUri = "/api/Album/100";

         // Act
         var response = await _client.GetAsync(requestUri);

         // Assert
         Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
      }

      [Fact]
      public async Task CreateAlbum_ReturnsCreatedAtAction_WithNewAlbum()
      {
         // Arrange
         var requestUri = "/api/Album/";
         var newAlbum = new AlbumModel
         {
               Id = 36,
               Name = "Test Album",
               Artist = "Test Artist",
               ImageUrl = "https://example.com/image.jpg"
         };

         var json = JsonConvert.SerializeObject(newAlbum);
         var content = new StringContent(json, Encoding.UTF8, "application/json");

         // Act
         var response = await _client.PostAsync(requestUri, content);
         response.EnsureSuccessStatusCode();

         var responseContent = await response.Content.ReadAsStringAsync();
         var album = JsonConvert.DeserializeObject<AlbumModel>(responseContent);

         // Assert
         Assert.NotNull(album);
         Assert.Equal(newAlbum.Name, album.Name);
         Assert.Equal(newAlbum.Artist, album.Artist);
         Assert.Equal(newAlbum.ImageUrl, album.ImageUrl);
      }

      [Fact]
      public async Task UpdateAlbum_ReturnsNoContent()
      {
         // Arrange
         var createUri = "/api/Album/";
         var newAlbum = new AlbumModel
         {
               Id = 23,
               Name = "Test Album",
               Artist = "Test Artist",
               ImageUrl = "https://example.com/image.jpg"
         };

         var createJson = JsonConvert.SerializeObject(newAlbum);
         var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");

         var createResponse = await _client.PostAsync(createUri, createContent);
         createResponse.EnsureSuccessStatusCode();

         // Act
         var updateUri = $"/api/Album/{newAlbum.Id}";
         var updatedAlbum = new AlbumModel
         {
               Id = newAlbum.Id,
               Name = "Updated Album",
               Artist = "Updated Artist",
               ImageUrl = "https://example.com/updated.jpg"
         };

         var updateJson = JsonConvert.SerializeObject(updatedAlbum);
         var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

         var updateResponse = await _client.PutAsync(updateUri, updateContent);
         updateResponse.EnsureSuccessStatusCode();

         // Assert
         Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);
      }

      [Fact]
      public async Task UpdateAlbum_ReturnsBadRequest_WhenIdDoesNotMatch()
      {
         // Arrange
         var requestUri = "/api/Album/2";
         var updatedAlbum = new AlbumModel
         {
               Id = 1,
               Name = "Updated Album",
               Artist = "Updated Artist",
               ImageUrl = "https://example.com/updated.jpg"
         };

         var json = JsonConvert.SerializeObject(updatedAlbum);
         var content = new StringContent(json, Encoding.UTF8, "application/json");

         // Act
         var response = await _client.PutAsync(requestUri, content);

         // Assert
         Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);            
      }

      [Fact]
      public async Task DeleteAlbum_ReturnsNoContent()
      {
         // Arrange
         var requestUri = "/api/Album/5";
         var albumToDelete = new AlbumModel
         {
               Id = 5,
               Name = "Album to Delete",
               Artist = "Artist to Delete",
               ImageUrl = "https://example.com/delete.jpg"
         };

         await _client.PostAsJsonAsync("/api/Album/", albumToDelete);

         // Act
         var response = await _client.DeleteAsync(requestUri);
         response.EnsureSuccessStatusCode();

         // Assert
         Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
      }

      [Fact]
      public async Task DeleteAlbum_ReturnsNotFound_WhenAlbumDoesNotExist()
      {
         // Arrange
         var requestUri = "/api/Album/100";

         // Act
         var response = await _client.DeleteAsync(requestUri);

         // Assert
         Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
      }

      [Fact]
   public async Task GetAlbumById_ReturnsAlbum_WhenAlbumExists()
   {
      // Arrange
      var album = new AlbumModel
      {
            Id = 1,
            Name = "Test Album",
            Artist = "Test Artist",
            ImageUrl = "https://example.com/image.jpg"
      };

      var mockService = new Mock<IAlbumService>();

      mockService.Setup(service => service.GetAlbumById(1))
            .ReturnsAsync(album);

      var controller = new AlbumController(mockService.Object);

      // Act
      var result = await controller.GetAlbumById(1);

      // Assert
      var actionResult = Assert.IsType<ActionResult<AlbumModel>>(result);
      var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
      var model = Assert.IsAssignableFrom<AlbumModel>(okResult.Value);

      Assert.Equal(album.Id, model.Id);
   }

   [Fact]
   public async Task CreateAlbum_ReturnsAlbum_WhenAlbumIsValid()
   {
      // Arrange
      var newAlbum = new AlbumModel
      {
            Id = 95,
            Name = "Test Album",
            Artist = "Test Artist",
            ImageUrl = "https://example.com/image.jpg"
      };

      var mockService = new Mock<IAlbumService>();

      mockService.Setup(service => service.CreateAlbum(newAlbum))
            .ReturnsAsync(newAlbum);

      var controller = new AlbumController(mockService.Object);

      // Act
      var result = await controller.CreateAlbum(newAlbum);

      // Assert
      var actionResult = Assert.IsType<ActionResult<AlbumModel>>(result);
      var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
      var model = Assert.IsAssignableFrom<AlbumModel>(createdAtActionResult.Value);

      Assert.Equal(newAlbum.Id, model.Id);
   }
   }
}
