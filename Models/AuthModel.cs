namespace interships_Management.Models;

public class AuthModel {
    
    
    public string? Message { get; set; }

    public string? id { get; set; }

    public bool isAuthenticated { get; set; }

    public string? Email { get; set; }

    public List<string>? Roles { get; set; }

    public string? Token { get; set; }
    public DateTime ExpiresOn { get; set; }


} 