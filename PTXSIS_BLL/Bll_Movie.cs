using MYTEST_BusinessObjects;
using MYTEST_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTEST_BLL
{
    public partial class Bll
    {
        public async Task<List<MovieDTO>> GetMoviesAsync()
        {
            var movieFromRepo = await _movieRepository.GetAllAsync();
            return _mapper.Map<List<MovieDTO>>(movieFromRepo);
        }

        public async Task<MovieDTO> GetMovieByIdAsync(int id)
        {
            var movieById = _movieRepository.Find(x => x.Id == id).FirstOrDefault();
            if(movieById != null)
            {
                var mapMovie = _mapper.Map<MovieDTO>(movieById);
                return mapMovie;
            }
            else
            {
                throw new KeyNotFoundException($"Error on getting Movie By Id = {id}");
            }

        }
        
        public async Task<MovieDTO> UpdateMovie(MovieUpdateDTO movieUpdate)
        {
            var movieById = _movieRepository.Find(x => x.Id == movieUpdate.Id);
            if(movieById != null)
            {
                var mapMovie = _mapper.Map<Movie>(movieUpdate);
                _movieRepository.Update(mapMovie);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<MovieDTO>(mapMovie);
            }
            else
            {
                throw new KeyNotFoundException($"Update movie tried on unexisting id {movieUpdate.Id}");
            }
        }

        public async Task<MovieDTO> CreateMovie(MovieCreateDTO movieCreate)
        {
            if (movieCreate != null)
            {
                var mapMovie = _mapper.Map<Movie>(movieCreate);
                _movieRepository.Add(mapMovie);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<MovieDTO>(mapMovie);
            }
            else
            {
                throw new KeyNotFoundException($"Create movie tried on error parameter {movieCreate}");
            }
        }

        public async Task DeleteMovie(int id)
        {
            if(id > 0)
            {
                var movie = _movieRepository.Find(x => x.Id == id).FirstOrDefault();
                if(movie != null)
                {
                    _movieRepository.Delete(movie);
                }
                else
                {
                    throw new ArgumentNullException($"Delete movie tried on unexisting movie is {movie}");
                }
            }
            else
            {
                throw new KeyNotFoundException($"Delete movie tried on error {id}");
            }
        }
    }
}
