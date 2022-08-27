using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace interships_Management.Models;
public class ApplicationUser : IdentityUser{
    
    public string? FullName { get; set; }
   
    public string? Ecole  { get; set; }
    
    public string? Niveau_Specialite { get; set; }

    public string? lienGithub { get; set; }

    public string? lienLinkedln { get; set; }

    public int? validate { get; set; }
    
    

}