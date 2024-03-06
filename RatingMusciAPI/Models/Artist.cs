using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RatingMusciAPI.Models;


[Table("Artists")]
public class Artist
{
    [Key]
    public int ArtistId { get; set; }

    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Description { get; set; }

    [Required]
    [StringLength(300)]
    public string ImageUrl { get; set; }

    [Required]
    [Range(1,5)]
    public double Rating { get; set; }
    [Required]
    public int TotalListeners { get; set; }

    [JsonIgnore]
    public ICollection<Album>? Albums { get; set; }

    [JsonIgnore]
    public ICollection<Song>? Songs { get; set; }


}
