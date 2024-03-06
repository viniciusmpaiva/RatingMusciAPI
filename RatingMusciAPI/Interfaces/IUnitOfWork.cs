namespace RatingMusciAPI.Interfaces;

public interface IUnitOfWork
{
    IArtistsRepository ArtistsRepository { get; }
    IAlbumsRepository AlbumsRepository { get; }
    ISongsRepository SongsRepository { get; }
    Task CommitAsync();
}
