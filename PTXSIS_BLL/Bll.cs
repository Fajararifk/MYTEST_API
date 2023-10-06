using AutoMapper;
using Microsoft.Extensions.Logging;
using MYTEST_BusinessObjects;
using MYTEST_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTEST_BLL
{
    public partial class Bll : IBll
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Movie> _movieRepository;
        private readonly ILogger<Bll> _logger;
        private readonly IMapper _mapper;

        public Bll(IUnitOfWork unitOfWork, ILogger<Bll> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _movieRepository = unitOfWork.GetGenericRepository<Movie>();
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
    }
}
