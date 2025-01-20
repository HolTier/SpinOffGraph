using Media.Api.Controllers;
using Media.Api.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Tests.Controllers
{
    public class MediaControllerTests
    {
        private readonly Mock<IMediaService> _mediaService;
        private readonly MediaController _mediaController;

        public MediaControllerTests()
        {
            _mediaService = new Mock<IMediaService>();
            _mediaController = new MediaController(_mediaService.Object);
        }


    }
}
