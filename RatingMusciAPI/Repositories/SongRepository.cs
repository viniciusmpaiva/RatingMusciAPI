using RatingMusciAPI.Context;
using RatingMusciAPI.Interfaces;
using RatingMusciAPI.Models;
using RatingMusciAPI.Pagination;
using X.PagedList;

namespace RatingMusciAPI.Repositories;

public class SongRepository: Repository<Song>, ISongsRepository
{
    public SongRepository(AppDbContext context):base(context)
    {
    }

    public async Task<IPagedList<Song>> GetSongsPaged(SongsParam songsParam){
        var songs = await GetAllAsync();
        var songsSorted = songs.OrderByDescending(s => s.Rating).AsQueryable();

        var result = await songsSorted.ToPagedListAsync(songsParam.PageNumber, songsParam.PageSize);
        return result;
    }

    public async Task<IPagedList<Song>> GetSongsAlbumPaged(int id,SongsParam songsParam)
    {
        var songs = _context.Songs.Where(s => s.AlbumId == id);
        var songsSorted = songs.OrderByDescending(s => s.Rating);
        var result = await songsSorted.ToPagedListAsync(songsParam.PageNumber, songsParam.PageSize);
        return result;
    }

    public async Task<IPagedList<Song>> GetSongsArtistPaged(int id, SongsParam songsParam)
    {
        var songs = _context.Songs.Where(s => s.ArtistId == id).AsQueryable();
        var songsSorted = songs.OrderByDescending(s => s.Rating).AsQueryable();
        var result = await songsSorted.ToPagedListAsync(songsParam.PageNumber, songsParam.PageSize);
        return result;
    }

}
