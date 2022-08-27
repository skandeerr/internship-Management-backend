using interships_Management.Models;
using interships_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace interships_Management.Controllers;


[ApiController]
[Route("[controller]")]


public class AdminController : ControllerBase {
    private readonly IAuthAdminService _IauthService;
    private readonly UserManager<ApplicationUser> _userManager;

public AdminController(IAuthAdminService IauthService,UserManager<ApplicationUser> userManager)
    {
        _IauthService = IauthService;
        _userManager=userManager;

    }


[HttpPost("registre")]
[Authorize(Roles ="Super Admin")]
public async Task<IActionResult> RegistreUser(RegistreAdmin model){
    if(!ModelState.IsValid){
        return BadRequest(ModelState);
    }
    var resultat = await _IauthService.RegistreAdmin(model);
    if(!resultat.isAuthenticated){
        return BadRequest(resultat.Message);
    }
    return Ok(resultat);
}

[HttpGet]
[Route("all")]

public async Task<IActionResult> GetAllAdmin(){
    var users =  _userManager.Users;

    var stagiares = new List<ApplicationUser>();

    foreach(var user in users){
        if(await _userManager.IsInRoleAsync(user,"Admin")){
               stagiares.Add(user) ;
        }
    }
    return Ok(stagiares);
}


[HttpPost("login")]

public async Task<IActionResult> LoginController(LoginModel model){
    if(!ModelState.IsValid){
        return BadRequest(ModelState);
    }
    var resultat = await _IauthService.Login(model);
    if(!resultat.isAuthenticated){
        return BadRequest(resultat.Message);
    }
    return Ok(resultat);
}
[HttpGet]
[Route("{id:Guid}")]

public async Task<IActionResult> GetAdminById([FromRoute]Guid id){
    var admin =  await _userManager.FindByIdAsync(id.ToString());

    
    return Ok(admin);
}
[HttpPut]
[Route("validate/user/{id:Guid}")]
public async Task<IActionResult> ValidateUser([FromRoute]Guid id){
    var user = await _userManager.FindByIdAsync(id.ToString());
    user.validate = 1;
    await _userManager.UpdateAsync(user);
    return Ok(user);
}
[HttpPut]
[Route("edit/profil/{id:Guid}")]
public async Task<IActionResult> EditProfil([FromRoute]Guid id, RegistreAdmin profil){
    var admin = await _userManager.FindByIdAsync(id.ToString());
    admin.FullName = profil.FullName;
    admin.Email= profil.Email;
    admin.PhoneNumber= profil.tel;
    
    var result = await _userManager.ChangePasswordAsync(admin,admin.PasswordHash.GetHashCode().ToString(),profil.Password?.GetHashCode().ToString());
    if(!result.Succeeded){
            var errors = string.Empty;
            foreach(var error in result.Errors){
                errors += $"{error.Description},";
            }
            return BadRequest(errors);
        }
    await _userManager.UpdateAsync(admin);
    return Ok(admin);
}
[HttpDelete]
[Route("edit/profil/{id:Guid}")]
public async Task<IActionResult> DeleteAdmin([FromRoute]Guid id){
    var admin = await _userManager.FindByIdAsync(id.ToString());
    await _userManager.DeleteAsync(admin);
    return Ok();
}
}