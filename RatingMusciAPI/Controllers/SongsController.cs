using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RatingMusciAPI.DTO;
using RatingMusciAPI.Interfaces;
using RatingMusciAPI.Models;
using RatingMusciAPI.Pagination;
using X.PagedList;

namespace RatingMusciAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SongsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SongsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<SongDTO>>> GetSongs([FromQuery] SongsParam songsParam)
    {
        var songs = await _unitOfWork.SongsRepository.GetSongsPaged(songsParam);
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

    [HttpGet("{id}")]
    public async Task<ActionResult<SongDTO>> GetSong(int id)
    {
        var song = await _unitOfWork.SongsRepository.GetAsync(s=>s.SongId == id);
        if (song == null)
        {
            return NotFound("This song is not avaliable in our system");
        }
        var songDTO = _mapper.Map<SongDTO>(song);
        return Ok(song);
    }

    [HttpPost]
    public async Task<ActionResult<SongDTO>> Post(Song song)
    {
        if(song is null)
        {
            return BadRequest("The song is null");
        }
        _unitOfWork.SongsRepository.Create(song);
        await _unitOfWork.CommitAsync();
        var result = _mapper.Map<SongDTO>(song);
        return new CreatedAtRouteResult("GetSong", new { id = result.SongId });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SongDTO>> Put(int id, SongDTO songDTO)
    {
        var findSong = _unitOfWork.SongsRepository.GetAsync(s=>s.SongId == id);
        if(findSong is null)
        {
            return NotFound("This song is not avaliable in our system");
        }
        var song = _mapper.Map<Song>(songDTO);
        _unitOfWork.SongsRepository.Update(song);
        await _unitOfWork.CommitAsync();
        var result = _mapper.Map<SongDTO>(song);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SongDTO>> Delete(int id)
    {
        var song = await _unitOfWork.SongsRepository.GetAsync(s=>s.SongId == id);
        if (song is null)
        {
            return NotFound("This song is not avaliable in our system");
        }
        _unitOfWork.SongsRepository.Delete(song);
        await _unitOfWork.CommitAsync();
        _mapper.Map<SongDTO>(song);
        return Ok(song);
    }
}
