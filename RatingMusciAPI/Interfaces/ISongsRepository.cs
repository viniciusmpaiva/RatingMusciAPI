using RatingMusciAPI.Models;
using RatingMusciAPI.Pagination;
using X.PagedList;

namespace RatingMusciAPI.Interfaces;

public interface ISongsRepository: IRepository<Song>
{
    public Task<IPagedList<Song>> GetSongsArtistPaged(int id,SongsParam songsParam);
    public Task<IPagedList<Song>> GetSongsAlbumPaged(int id,SongsParam songsParam);

    public Task<IPagedList<Song>> GetSongsPaged(SongsParam songsParam);
}
