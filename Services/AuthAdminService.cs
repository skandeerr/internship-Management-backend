using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using interships_Management.Helpers;
using interships_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace interships_Management.Services;
public class AuthAdminService : IAuthAdminService {
private readonly UserManager<ApplicationUser> _userManager;
    private readonly Jwt _jwt;
    public AuthAdminService(UserManager<ApplicationUser> userManager,IOptions<Jwt> jwt)
    {
        _userManager=userManager;
        _jwt= jwt.Value;        
    }
    public async Task<AuthModel> RegistreAdmin (RegistreAdmin model){
         if(await _userManager.FindByEmailAsync(model.Email) != null){
            return new AuthModel  { Message= "Email is already registered!"};
        }
        var user = new ApplicationUser{
            Email = model.Email,
            FullName = model.FullName,
            PhoneNumber = model.tel,
            UserName=model.Email,
            Ecole= "finlogik",
            Niveau_Specialite="RH" ,
            
        };
        var resultat = await _userManager.CreateAsync(user,model.Password);
        if(!resultat.Succeeded){
            var errors = string.Empty;
            foreach(var error in resultat.Errors){
                errors += $"{error.Description},";
            }
            return new AuthModel {Message=errors};
        }
        await _userManager.AddToRoleAsync(user,"Admin");
         var idU=user.Id.ToString();
        var jwtSecurityToke = await CreateJwtToken(user);
        return new AuthModel {
            Email = user.Email,
            ExpiresOn = jwtSecurityToke.ValidTo,
            isAuthenticated = true,
            Roles = new List<string> { "Admin"},
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToke),
            id=idU,
        } ;
    }

    
    public async Task<AuthModel> Login (LoginModel model){
        var authModel = new AuthModel();
        var admin = await _userManager.FindByEmailAsync(model.Email);
        if(admin == null || !await _userManager.CheckPasswordAsync(admin,model.Password) ){
            authModel.Message="Email or Password is incorrect";
            return authModel;
        }
        var idU=admin.Id.ToString();
        var jwtSecurityToke = await CreateJwtToken(admin);
        var roleList = await _userManager.GetRolesAsync(admin);
        authModel.isAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToke);
        authModel.Email = admin.Email;
        authModel.ExpiresOn = jwtSecurityToke.ValidTo;
        authModel.Roles = roleList.ToList();
        authModel.id = idU;

        return authModel;
    }

    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
}