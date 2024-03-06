using RatingMusciAPI.Models;
using RatingMusciAPI.Pagination;
using X.PagedList;

namespace RatingMusciAPI.Interfaces;

public interface IArtistsRepository:IRepository<Artist>
{
    public Task<IPagedList<Artist>> GetArtistPaged(ArtistsParams artistsParams);
}
