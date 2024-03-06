using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RatingMusciAPI.Models;

public class Album
{
    [Key]
    public int AlbumId { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [Required]
    [StringLength(100)]
    public string Description { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    [Required]
    [Range(1,5)]
    public double Rating { get; set; }
    [Required]
    public string Credits { get; set; }

    [Required]
    public int Streams { get; set; }
    public DateOnly Release { get; set; }

    public int ArtistId { get; set; }
    [JsonIgnore]
    public Artist? Artist { get; set; } 

    [JsonIgnore]
    public ICollection<Song>? Songs { get; set; }

}
