using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RatingMusciAPI.DTO;
using RatingMusciAPI.Interfaces;
using RatingMusciAPI.Models;
using RatingMusciAPI.Pagination;

namespace RatingMusciAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AlbumsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AlbumsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlbumDTO>>> GetAlbums([FromQuery]AlbumsParam albumsParam)
    {
        var albums = await _unitOfWork.AlbumsRepository.GetAlbumsPaged(albumsParam);
        var metadata = new
        {
            albums.Count,
            albums.PageSize,
            albums.PageCount,
            albums.TotalItemCount,
            albums.HasNextPage,
            albums.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var albumsDTO = _mapper.Map<IEnumerable<AlbumDTO>>(albums);
        return Ok(albumsDTO);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlbumDTO>> GetAlbum(int id)
    {
        var album = await _unitOfWork.AlbumsRepository.GetAsync(a => a.ArtistId == id);
        if (album == null)
        {
            return NotFound();
        }
        var albumDTO = _mapper.Map<AlbumDTO>(album);
        return Ok(albumDTO);
    }

    [HttpGet("{id}/songs")]
    public async Task<ActionResult<IEnumerable<SongDTO>>> GetAlbumSongs(int id,[FromQuery]SongsParam songsParam)
    {
        var album = await _unitOfWork.AlbumsRepository.GetAsync(a => a.AlbumId == id);
        if (album == null)
        {
            return NotFound("Sorry! The album is not in our system :( ");
        }

        var songs = await _unitOfWork.SongsRepository.GetSongsAlbumPaged(id,songsParam);
        if (songs == null)
        {
            return Ok("The album doesn't have any songs");
        }

        var metadata = new
        {
            songs.Count,
            songs.PageSize,
            songs.PageCount,
            songs.TotalItemCount,
            songs.HasNextPage,
            songs.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var songsDTO = _mapper.Map<IEnumerable<SongDTO>>(songs);

        return Ok(songsDTO);
    }

    [HttpPost]
    [Authorize(Policy ="ModOnly")]
    public async Task<ActionResult<AlbumDTO>> Post(Album album)
    {
        _unitOfWork.AlbumsRepository.Create(album);
        await _unitOfWork.CommitAsync();
        var albumDTO = _mapper.Map<AlbumDTO>(album);
        return new CreatedAtRouteResult("GetAlbum", new { name = albumDTO.Name });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "ModOnly")]

    public async Task<ActionResult<Album>> Put(Album album, int id)
    {
        if(id != album.AlbumId)
        {
            return BadRequest();
        }
        _unitOfWork.AlbumsRepository.Update(album);
        await _unitOfWork.CommitAsync();
        var albumDTO = _mapper.Map<AlbumDTO>(album);
        return Ok(albumDTO);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy ="AdminOnly")]
    public async Task<ActionResult<Album>> Delete(int id)
    {
        var album = await _unitOfWork.AlbumsRepository.GetAsync(a => a.AlbumId == id);
        if(album == null)
        {
            return NotFound();
        }
        _unitOfWork.AlbumsRepository.Delete(album);
        await _unitOfWork.CommitAsync();
        var albumDTO = _mapper.Map<AlbumDTO>(album);
        return Ok(albumDTO);
    }
}
