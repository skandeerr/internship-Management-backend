using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using interships_Management.Helpers;
using interships_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace interships_Management.Services;
public class AuthUserService : IAuthUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Jwt _jwt;
    public AuthUserService(UserManager<ApplicationUser> userManager,IOptions<Jwt> jwt)
    {
        _userManager=userManager;
        _jwt= jwt.Value;
                 

    }

    public async Task<AuthModel> RegistreUser(RegistreUser model)
    {
        if(await _userManager.FindByEmailAsync(model.Email) != null){
            return new AuthModel  { Message= "Email is already registered!"};
        }
        var user = new ApplicationUser{
            
            Email = model.Email,
            FullName = model.FullName,
            Ecole = model.Ecole,
            PhoneNumber = model.tel,
            Niveau_Specialite= model.Niveau_Specialite,
            UserName=model.Email,
            lienGithub=model.lienGithub,
            lienLinkedln=model.lienLinkedln,
            validate=0
        };
        var resultat = await _userManager.CreateAsync(user,model.Password);
        if(!resultat.Succeeded){
            var errors = string.Empty;
            foreach(var error in resultat.Errors){
                errors += $"{error.Description},";
            }
            return new AuthModel {Message=errors};
        }
        await _userManager.AddToRoleAsync(user,"User");
        
        var idU=user.Id.ToString();

        var jwtSecurityToke = await CreateJwtToken(user);
        return new AuthModel
        {
            Email = user.Email,
            ExpiresOn = jwtSecurityToke.ValidTo,
            id = idU,
            isAuthenticated = true,
            Roles = new List<string> { "User" },
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToke),
        } ;

    }
    public async Task<AuthModel> Login (LoginModel model){
        var authModel = new AuthModel();
        var user = await _userManager.FindByEmailAsync(model.Email);
        if(user == null || !await _userManager.CheckPasswordAsync(user,model.Password) ){
            authModel.Message="Email or Password is incorrect";
            return authModel;
        }
        var jwtSecurityToke = await CreateJwtToken(user);
        var roleList = await _userManager.GetRolesAsync(user);
        
        var userId = user.Id.ToString();
        authModel.id=userId;
        authModel.isAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToke);
        authModel.Email = user.Email;
        authModel.ExpiresOn = jwtSecurityToke.ValidTo;
        authModel.Roles = roleList.ToList();

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