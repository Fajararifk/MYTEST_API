using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MYTEST_BusinessObjects;
using MYTEST_Contracts;
using MYTEST_DTO;

namespace MYTEST.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IBll _bll;
        public MovieController(IBll bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            try
            {
                var movie = await _bll.GetMoviesAsync();
                return Ok(movie);
            } 
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            try
            {
                if(id > 0)
                {
                    var movieById = await _bll.GetMovieByIdAsync(id);
                    return Ok(movieById);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovie(MovieUpdateDTO movieUpdateDTO)
        {
            try
            {
                if(movieUpdateDTO.Id > 0)
                {
                    var updateMovie = await _bll.UpdateMovie(movieUpdateDTO);
                    return Ok(updateMovie);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateDTO movieCreateDTO)
        {
            try
            {
                if (movieCreateDTO != null)
                {
                    var createMovie = await _bll.CreateMovie(movieCreateDTO);
                    return Ok(createMovie);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                if (id > 0)
                {
                    await _bll.DeleteMovie(id);
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
