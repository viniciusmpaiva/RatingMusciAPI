using RatingMusciAPI.Models;
using RatingMusciAPI.Pagination;
using X.PagedList;

namespace RatingMusciAPI.Interfaces;

public interface IAlbumsRepository:IRepository<Album>   
{
    public Task<IPagedList<Album>> GetAlbumsArtistPaged(int id,AlbumsParam albumsParam);
    public Task<IPagedList<Album>> GetAlbumsPaged(AlbumsParam albumsParam);
}
