using interships_Management.Data;
using interships_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace interships_Management.Controllers;
[ApiController]
[Route("[controller]")]
public class PostProjController : ControllerBase{
private readonly ContextDb _contextDb ;
public PostProjController(ContextDb contextDb)
{
    _contextDb=contextDb;
}
[HttpGet]
    public async Task<IActionResult> Get(){
        var postproj = await _contextDb.PostProjects.ToListAsync();
                
        return Ok(postproj);
 
    }

    
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid Id){
        var postproj = await _contextDb.PostProjects.FindAsync(Id);
        if(postproj == null){
            return NotFound($"Aucun projet avec id{Id}");
        }
        return Ok(postproj);
    }
   
    [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostProject postproj){
            postproj.id = Guid.NewGuid();
             if (!ModelState.IsValid)
        {
            return BadRequest();
        }
           await _contextDb.PostProjects.AddAsync(postproj);
           await _contextDb.SaveChangesAsync();
           return Ok(postproj);
        }


        [HttpPut]
       
        [Route("{id:Guid}")]
        public async Task<IActionResult> Put(Guid id,PostProject postproj){
          var  postprojs = await _contextDb.PostProjects.FindAsync(id);
            if(postprojs == null){
                return BadRequest("id n'existe pas");
            }else{
                postproj.id_user = postprojs.id_user;
                postproj.id_proj = postprojs.id_proj;
                
                
                await _contextDb.SaveChangesAsync();

                return Ok(postproj);
            }
        }
         [HttpDelete]
         
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete(Guid id ){
            var postproj = await _contextDb.PostProjects.FindAsync(id);
            if(postproj == null){
                return NotFound("id n'existe pas");
            }else{
                _contextDb.PostProjects.Remove(postproj);
                await _contextDb.SaveChangesAsync();
                return Ok();
            }
        }
        }