using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RatingMusciAPI.Models;

[Table("Songs")]
public class Song
{
    [Key]
    public int SongId { get; set; }

    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    [Required]
    public string Credits { get; set; }

    public DateOnly Release { get; set; }
    public string? Lyrics { get; set; }

    public int Streams { get; set; }

    public int ArtistId { get; set; }

    [JsonIgnore]
    public Artist? Artist { get; set; }

    public int AlbumId { get; set; }
    [JsonIgnore]
    public Album? Album { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    [Required]
    [Range(1,5)]
    public double Rating { get; set; }
}
