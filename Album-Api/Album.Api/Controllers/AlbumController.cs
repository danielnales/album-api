using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Album.Api.Models;
using Album.Api.Services;

namespace Album.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        // GET: api/Albums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumModel>>> GetAllAlbums()
        {
            var albums = await _albumService.GetAllAlbums();
            return Ok(albums);
        }

        // GET: api/Albums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlbumModel>> GetAlbumById(int id)
        {
            var album = await _albumService.GetAlbumById(id);
            if (album == null)
            {
                return NotFound();
            }
            return Ok(album);
        }

        // POST: api/Albums
        [HttpPost]
        public async Task<ActionResult<AlbumModel>> CreateAlbum(AlbumModel newAlbum)
        {
            if (newAlbum == null)
            {
                return BadRequest("Album is null.");
            }

            var createdAlbum = await _albumService.CreateAlbum(newAlbum);
            return CreatedAtAction(nameof(GetAlbumById), new { id = createdAlbum.Id }, createdAlbum);
        }

        // PUT: api/Albums/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlbum(int id, AlbumModel updatedAlbum)
        {
            if (id != updatedAlbum.Id)
            {
                return BadRequest("Album ID mismatch.");
            }

            if (!_albumService.AlbumExists(id))
            {
                return NotFound();
            }

            await _albumService.UpdateAlbum(updatedAlbum);
            return NoContent();
        }

        // DELETE: api/Albums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            if (!_albumService.AlbumExists(id))
            {
                return NotFound();
            }

            await _albumService.DeleteAlbum(id);
            return NoContent();
        }
    }
}
