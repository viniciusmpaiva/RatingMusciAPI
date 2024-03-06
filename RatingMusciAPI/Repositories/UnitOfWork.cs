using RatingMusciAPI.Context;
using RatingMusciAPI.Interfaces;

namespace RatingMusciAPI.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IArtistsRepository _artistRepository;
    private readonly IAlbumsRepository _albumRepository;
    private readonly ISongsRepository _songRepository;
    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IArtistsRepository ArtistsRepository
    {
        get
        {
            return _artistRepository ?? new ArtistRepository(_context);
        }
    }

    public IAlbumsRepository AlbumsRepository
    {
        get
        {
            return _albumRepository ?? new AlbumRepository(_context);
        }
    }

    public ISongsRepository SongsRepository
    {
        get
        {
            return _songRepository ?? new SongRepository(_context);
        }
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
