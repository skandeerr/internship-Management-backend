using interships_Management.Data;
using interships_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace interships_Management.Controllers;
[ApiController]
[Route("[controller]")]


public class ProjectController : ControllerBase {

private readonly ContextDb _contextDb;
    public ProjectController(ContextDb contextDb)
    {
        _contextDb= contextDb;
    }
    [HttpGet]
    public async Task<IActionResult> Get(){
        var projects = await _contextDb.Projects.ToListAsync();
                
        return Ok(projects);
 
    }
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id){
        var projet = await _contextDb.Projects.FindAsync(id);
        
        if(projet == null){
            return NotFound($"Aucun projet avec id : {id}");
        }
        return Ok(projet);
    }
    [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectModel project){
            project.id = Guid.NewGuid();
             if (!ModelState.IsValid)
        {
            return BadRequest();
        }
           await _contextDb.Projects.AddAsync(project);
           await _contextDb.SaveChangesAsync();
           return Ok(project);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] ProjectModel project){
          var  projet = await _contextDb.Projects.FindAsync(id);
            if(projet == null){
                return NotFound("id n'existe pas");
            }else{
                projet.nom = project.nom;
                projet.description = project.description;
                projet.refe= project.refe;
                projet.technologies=project.technologies;
                projet.type=project.type;
                projet.periode=project.periode;
                projet.numberInter=project.numberInter;
                await _contextDb.SaveChangesAsync();

                return Ok(project);
            }
            


        }
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id ){
            var projet = await _contextDb.Projects.FindAsync(id);
            if(projet == null){
                return NotFound("id n'existe pas");
            }else{
                _contextDb.Projects.Remove(projet);
                await _contextDb.SaveChangesAsync();
                return Ok();
            }
        }
}