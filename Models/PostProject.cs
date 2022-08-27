using System.ComponentModel.DataAnnotations;

namespace interships_Management.Models;
public class PostProject {
    [Required]
    public Guid id { get; set; }
    [Required]
    public Guid id_proj { get; set; }
    [Required]
    public Guid id_user { get; set; }
    [Required]
    public string? cv { get; set; }
    [Required]
    public string? description { get; set; }
}