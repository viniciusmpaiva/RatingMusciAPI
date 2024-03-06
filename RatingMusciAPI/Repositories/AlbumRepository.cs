using RatingMusciAPI.Context;
using RatingMusciAPI.Interfaces;
using RatingMusciAPI.Models;
using RatingMusciAPI.Pagination;
using X.PagedList;

namespace RatingMusciAPI.Repositories;

public class AlbumRepository:Repository<Album>, IAlbumsRepository
{
    public AlbumRepository(AppDbContext context):base(context)
    {
    }

    public async Task<IPagedList<Album>> GetAlbumsArtistPaged(int id,AlbumsParam albumsParam)
    {
        var albums = await GetAllAsync();
        var albumsArtist = albums.Where(a => a.ArtistId == id).OrderByDescending(a=>a.Streams);
        var result = await albumsArtist.ToPagedListAsync(albumsParam.PageNumber,albumsParam.PageSize);
        return result;
    }

    public async Task<IPagedList<Album>> GetAlbumsPaged(AlbumsParam albumsParam)
    {
        var albums = await GetAllAsync();
        var alumsSorted = albums.OrderByDescending(a=>a.Streams);
        var result = await alumsSorted.ToPagedListAsync(albumsParam.PageNumber,albumsParam.PageSize);
        return result;
    }

    public async Task<IEnumerable<Song>> GetSongsAlbum(string name)
    {
        var album = await GetAsync(a => a.Name == name);
        if(album == null)
        {
            return null;
        }
        return album.Songs;
    }
}
