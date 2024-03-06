using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RatingMusciAPI.DTO;
using RatingMusciAPI.Interfaces;
using RatingMusciAPI.Models;
using RatingMusciAPI.Pagination;

namespace RatingMusciAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ArtistsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ArtistsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArtistDTO>>> GetArtists([FromQuery]ArtistsParams artistsParams)
    {
        var artists = await _unitOfWork.ArtistsRepository.GetArtistPaged(artistsParams);
        var metadata = new {
            artists.Count,
            artists.PageSize,
            artists.PageCount,
            artists.TotalItemCount,
            artists.HasNextPage,
            artists.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination",JsonConvert.SerializeObject(metadata));
        var artistsDTO = _mapper.Map<IEnumerable<ArtistDTO>>(artists);
        return Ok(artistsDTO);
    }

    [HttpGet("{id}")]
    
    public async Task<ActionResult<Artist>> GetArtist(int id)
    {
        var artist = await _unitOfWork.ArtistsRepository.GetAsync(a => a.ArtistId == id);
        if (artist == null)
        {
            return NotFound();
        }
        var artistDTO = _mapper.Map<ArtistDTO>(artist);
        return Ok(artistDTO);
    }

    [HttpGet("{id}/songs")]
    public async Task<ActionResult<IEnumerable<SongDTO>>> GetArtistSongs([FromQuery]SongsParam songsParam,int id)
    {
        var artist = await _unitOfWork.ArtistsRepository.GetAsync(a => a.ArtistId == id);
        if (artist == null)
        {
            return NotFound("Sorry! The artist is not in our system :( ");
        }

        var songs = await _unitOfWork.SongsRepository.GetSongsArtistPaged(artist.ArtistId,songsParam);
        if (songs == null)
        {
            return Ok("The artist doesn't have any songs");
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


    [HttpGet("{id}/albums")]
    public async Task<ActionResult<IEnumerable<AlbumDTO>>> GetArtistAlbums(int id, [FromQuery] AlbumsParam albumsParam)
    {
        var artist = await _unitOfWork.ArtistsRepository.GetAsync(a => a.ArtistId == id);
        if (artist == null)
        {
            return NotFound("Sorry! The artist is not in our system :( ");
        }
        var albums = await _unitOfWork.AlbumsRepository.GetAlbumsArtistPaged(artist.ArtistId,albumsParam);
        if (albums == null)
        {
            return Ok("The artist doesn't have any albums");
        }
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


    [HttpPost]
    public async Task<ActionResult<Artist>> Post(Artist artist)
    { 
        if(artist == null)
        {
            return BadRequest();
        }
        _unitOfWork.ArtistsRepository.Create(artist);
        await _unitOfWork.CommitAsync();
        return new CreatedAtRouteResult("GetArtist", new { name = artist.Name });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Artist>> Put(int id, Artist artist)
    {
        if (id != artist.ArtistId)
        {
            return BadRequest();
        }
        _unitOfWork.ArtistsRepository.Update(artist);    
        await _unitOfWork.CommitAsync();
        var artistDTO = _mapper.Map<ArtistDTO>(artist);
        return Ok(artist);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Artist>> Delete(int id)
    {
        var artist = await _unitOfWork.ArtistsRepository.GetAsync(a => a.ArtistId == id);
        if (artist == null)
        {
            return NotFound();
        }
        _unitOfWork.ArtistsRepository.Delete(artist);
        await _unitOfWork.CommitAsync();
        var artistDTO = _mapper.Map<ArtistDTO>(artist);
        return Ok(artistDTO);
    }

}
