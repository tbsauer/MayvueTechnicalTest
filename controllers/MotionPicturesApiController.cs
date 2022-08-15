
using Microsoft.AspNetCore.Mvc;
using MotionPicturesCore.Interfaces;
using MotionPicturesCore.Models;
using System.Data.SqlClient;

namespace MotionPicturesCore.Controllers
{
	[Route("api/motionpictures")]
	[ApiController]
	public class MotionPicturesApiController : ControllerBase
	{
		private readonly IMotionPictureService _movieService;

		public MotionPicturesApiController(IMotionPictureService movieService)
		{
			_movieService = movieService;
		}

		[HttpGet]
		public async Task<IActionResult> GetMotionPictures()
		{
			try
			{
				var movies = await _movieService.GetMotionPictures();
				return Ok(movies);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetMotionPicture(int id)
		{
			try
			{
				var movie = await _movieService.GetMotionPicture(id);
				if (movie == null)
					return NotFound();

				return Ok(movie);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateMotionPicture(MotionPictureAddRequest movie)
		{
			try
			{
				var createdMovie = await _movieService.CreateMotionPicture(movie);
				return CreatedAtAction("CreateMotionPicture", new { id = createdMovie.Id }, createdMovie);
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateMotionPicture(int id, MotionPictureUpdateRequest movie)
		{
			try
			{
				var dbMovie = await _movieService.GetMotionPicture(id);
				if (dbMovie == null)
					return NotFound();

				await _movieService.UpdateMotionPicture(id, movie);
				return NoContent();
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMotionPicture(int id)
		{
			try
			{

				await _movieService.DeleteMotionPicture(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				//log error
				return StatusCode(500, ex.Message);
			}
		}
	}
}

