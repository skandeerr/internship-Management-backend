using System.Collections.Generic;
using System.Linq;
using interships_Management.Models;
using interships_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;

namespace interships_Management.Controllers;
[ApiController]
[Route("[controller]")]
public class userController : ControllerBase{
private readonly IAuthUserService _IauthService;
private readonly UserManager<ApplicationUser> _userManager;

public userController(IAuthUserService IauthService,UserManager<ApplicationUser> userManager)
{
    _IauthService = IauthService;
    _userManager=userManager;

}
[HttpPost("registre")]
public async Task<IActionResult> RegistreUser(RegistreUser model){
    if(!ModelState.IsValid){
        return BadRequest(ModelState);
    }
    var resultat = await _IauthService.RegistreUser(model);
    if(!resultat.isAuthenticated){
        return BadRequest(resultat.Message);
    }
    return Ok(resultat);
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
[Route("all")]
[Authorize(Roles ="Admin")]
public async Task<IActionResult> GetAllUsers(){
    var users =  _userManager.Users;

    var stagiares = new List<ApplicationUser>();

    foreach(var user in users){
        if(await _userManager.IsInRoleAsync(user,"User")){
               stagiares.Add(user) ;
        }
    }
    return Ok(stagiares);
}
[HttpGet]
[Route("{id:Guid}")]

public async Task<IActionResult> GetAdminById([FromRoute]Guid id){
    var users =  await _userManager.FindByIdAsync(id.ToString());

    
    return Ok(users);
}

}