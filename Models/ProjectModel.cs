using System.ComponentModel.DataAnnotations;

namespace interships_Management.Models;
public class ProjectModel {
    [Required]
    public Guid id { get; set; }
    [Required]
    public string? refe { get; set; }
    [Required]
    public string? nom { get; set; }
    [Required]
    public string? description { get; set; }
    [Required]
    public string? technologies { get; set; }
    [Required]
    public string? type { get; set; }

    public int periode { get; set; }

    public int numberInter { get; set; }
}