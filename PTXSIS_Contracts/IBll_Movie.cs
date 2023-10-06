using MYTEST_BusinessObjects;
using MYTEST_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTEST_Contracts
{
    public partial interface IBll
    {
        Task<List<MovieDTO>> GetMoviesAsync();
        Task<MovieDTO> GetMovieByIdAsync(int id);
        Task<MovieDTO> UpdateMovie(MovieUpdateDTO movieUpdateDTO);
        Task<MovieDTO> CreateMovie(MovieCreateDTO movieCreateDTO);
        Task DeleteMovie(int id);
    }
}
