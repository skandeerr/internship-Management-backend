using System.ComponentModel.DataAnnotations;

namespace interships_Management.Models ;
public class EntretienModel {
    
    public Guid id { get; set; }
   
    public DateTime Date_entretien { get; set; }
    
    public Guid id_user { get; set; }
    public Guid id_proj { get; set; }
    public int validate { get; set; }

    public string? accept { get; set; }

}