using RatingMusciAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace RatingMusciAPI.DTO;

public class SongDTO
{
    public int SongId { get; set; }
    public string Name { get; set; }
    public string Credits { get; set; }
    public double Rating { get; set; }
    public int Streams { get; set; }

}
