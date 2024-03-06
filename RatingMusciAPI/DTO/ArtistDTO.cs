using RatingMusciAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RatingMusciAPI.DTO;

public class ArtistDTO
{
    public int ArtistId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
    public int TotalListeners { get; set; }

}
