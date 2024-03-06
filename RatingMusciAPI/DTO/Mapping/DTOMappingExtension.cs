using AutoMapper;
using RatingMusciAPI.Models;

namespace RatingMusciAPI.DTO.Mapping;

public class DTOMappingExtension:Profile
{
    public DTOMappingExtension()
    {
        CreateMap<SongDTO,Song>().ReverseMap();
        CreateMap<AlbumDTO,Album>().ReverseMap();
        CreateMap<ArtistDTO,Artist>().ReverseMap();
    }
}
