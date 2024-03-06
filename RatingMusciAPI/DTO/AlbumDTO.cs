using System.ComponentModel.DataAnnotations;

namespace RatingMusciAPI.DTO;

public class AlbumDTO
{
    public int AlbumId { get; set; }
    public string Name { get; set; }
    public string Credits { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
    public double Streams { get; set; }
}
