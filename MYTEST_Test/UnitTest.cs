using AutoMapper;
using Microsoft.Extensions.Logging;
using MYTEST.Mapper;
using MYTEST_BLL;
using MYTEST_BusinessObjects;
using MYTEST_Contracts;
using MYTEST_DTO;
using NSubstitute;
using Xunit.Sdk;

namespace MYTEST_Test
{
    [TestClass]
    public class UnitTest
    {
        private IBll bll;
        private IUnitOfWork unitOfWork;
        private IGenericRepository<Movie> movieRepository;
        private IMapper mapper;
        private ILogger<Bll> logger;

        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWork = Substitute.For<IUnitOfWork>();
            this.movieRepository = Substitute.For<IGenericRepository<Movie>>();
            this.logger = Substitute.For<ILogger<Bll>>();
            this.unitOfWork.GetGenericRepository<Movie>().Returns(this.movieRepository);

            MapperConfiguration mapperConfig = new (
            cfg => 
            {
                cfg.AddProfile(new MovieProfile());
            });

            this.mapper = new Mapper(mapperConfig);
            this.mapper.ConfigurationProvider.AssertConfigurationIsValid();
            this.bll = new Bll(unitOfWork, logger, mapper);
        }

        [TestMethod]
        public async Task GetMovieTest()
        {
            var movieDTO = new MovieDTO()
            {
                Id = 1,
                Description = "Description Movie",
                Image = "",
                Rating = 7,
                Title = "Judul Movie"
            };
            bll.GetMovieByIdAsync(1).Returns(movieDTO);

            var result = await bll.GetMovieByIdAsync(1);

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Description Movie", result.Description);
            Assert.AreEqual("", result.Image);
            Assert.AreEqual(7, result.Rating);
            Assert.AreEqual("Judul Movie", result.Title);
        }

    }
}