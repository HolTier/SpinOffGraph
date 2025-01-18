using Media.Api.Models;
using Media.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Media.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        
        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMediaItems()
        {
            try
            {                 
                var media = await _mediaService.GetMediaItemsAsync();
                if (media == null)
                {
                    return NotFound();
                }
                return Ok(media);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
           
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMediaItem(int id)
        {
            try
            {
                var media = await _mediaService.GetMediaItemByIdAsync(id);
                if (media == null)
                {
                    return NotFound();
                }
                return Ok(media);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetMediaItemsWithTitle(string title)
        {
            try
            {
                var media = await _mediaService.GetMediaItemsWithTitleAsync(title);
                if (media == null)
                {
                    return NotFound();
                }
                return Ok(media);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("mediaType/{mediaTypeId}")]
        public async Task<IActionResult> GetMediaItemsByMediaType(int mediaTypeId)
        {
            try
            {
                var media = await _mediaService.GetMediaItemsByMediaTypeAsync(mediaTypeId);
                if (media == null)
                {
                    return NotFound();
                }
                return Ok(media);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMediaItem([FromBody] MediaItem mediaItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {                 
                await _mediaService.AddMediaItemAsync(mediaItem);
                return CreatedAtAction(nameof(GetMediaItem), new { id = mediaItem.Id }, mediaItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMediaItem(int id, [FromBody] MediaItem mediaItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                mediaItem.Id = id;
                await _mediaService.UpdateMediaItemAsync(mediaItem);
                return Ok(mediaItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
