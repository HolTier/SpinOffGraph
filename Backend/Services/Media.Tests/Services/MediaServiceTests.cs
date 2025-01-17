using Media.Api.Data;
using Media.Api.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Tests.Services
{
    public class MediaServiceTests
    {
        private readonly Mock<IMediaRepository> _mediaRepositoryMock;
        private readonly Mock<IMediaTypeRepository> _mediaTypeRepositoryMock;
        private readonly MediaService _mediaService;

        public MediaServiceTests()
        {
            _mediaRepositoryMock = new Mock<IMediaRepository>();
            _mediaTypeRepositoryMock = new Mock<IMediaTypeRepository>();
            _mediaService = new MediaService(_mediaRepositoryMock.Object, _mediaTypeRepositoryMock.Object);
        }


    }
}
