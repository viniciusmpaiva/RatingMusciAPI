using Microsoft.AspNetCore.Http.HttpResults;
using RatingMusciAPI.Context;
using RatingMusciAPI.Interfaces;
using RatingMusciAPI.Models;
using RatingMusciAPI.Pagination;
using X.PagedList;

namespace RatingMusciAPI.Repositories;

public class ArtistRepository:Repository<Artist>, IArtistsRepository
{
    public ArtistRepository(AppDbContext context):base(context)
    {
    }

    public async  Task<IPagedList<Artist>> GetArtistPaged(ArtistsParams artistsParams)
    {
        var artists = await GetAllAsync();
        var artistsSorted = artists.OrderByDescending(a => a.TotalListeners).AsQueryable();

        var result = await artistsSorted.ToPagedListAsync(artistsParams.PageNumber, artistsParams.PageSize);
        return result;
    }
}
