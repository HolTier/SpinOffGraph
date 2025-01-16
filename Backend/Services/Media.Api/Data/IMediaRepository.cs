﻿using Media.Api.Models;
using Shared.DataAccess;

namespace Media.Api.Data
{
    public interface IMediaRepository : IGenericRepository<MediaItem>
    {
    }
}
