using interships_Management.Data;
using interships_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace interships_Management.Controllers;
[ApiController]
[Route("[controller]")]


public class EntretienController : ControllerBase {
    private readonly ContextDb _contextDb ;
    public EntretienController(ContextDb contextDb)
    {
        _contextDb=contextDb;
    }
    [HttpGet]
    public async Task<IActionResult> Get(){
        var entretiens = await _contextDb.Entretiens.ToListAsync();
                
        return Ok(entretiens);
 
    }
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid Id){
        var entretien = await _contextDb.Entretiens.FindAsync(Id);
        if(entretien == null){
            return NotFound($"Aucun projet avec id{Id}");
        }
        return Ok(entretien);
    }
    [HttpPost]
        public async Task<IActionResult> Post([FromBody] EntretienModel entretien){
            entretien.id = Guid.NewGuid();
             if (!ModelState.IsValid)
        {
            return BadRequest();
        }
           await _contextDb.Entretiens.AddAsync(entretien);
           await _contextDb.SaveChangesAsync();
           return Ok(entretien);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Put(Guid id,EntretienModel entretiens){
          var  entretien = await _contextDb.Entretiens.FindAsync(id);
            if(entretien == null){
                return BadRequest("id n'existe pas");
            }else{
                entretien.id_user = entretiens.id_user;
                entretien.Date_entretien = entretiens.Date_entretien;
                entretien.validate= entretiens.validate;
                entretien.id_proj= entretiens.id_proj;
                
                await _contextDb.SaveChangesAsync();

                return Ok(entretien);
            }
            


        }
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete(Guid id ){
            var entretien = await _contextDb.Entretiens.FindAsync(id);
            if(entretien == null){
                return NotFound("id n'existe pas");
            }else{
                _contextDb.Entretiens.Remove(entretien);
                await _contextDb.SaveChangesAsync();
                return Ok();
            }
        }
      [HttpPut]
        [Route("validate/{id:Guid}")]
        public async Task<IActionResult> Put(Guid id){
          var  entretien = await _contextDb.Entretiens.FindAsync(id);
            if(entretien == null){
                return BadRequest("id n'existe pas");
            }else{
                entretien.validate= 1;
               
                
                await _contextDb.SaveChangesAsync();

                return Ok(entretien);
            }
       
}
        [HttpPut]
        [Route("accept/{id:Guid}")]
        public async Task<IActionResult> AcceptInter(Guid id){
          var  entretien = await _contextDb.Entretiens.FindAsync(id);
            if(entretien == null){
                return BadRequest("id n'existe pas");
            }else{
                entretien.accept= "oui";
               
                
                await _contextDb.SaveChangesAsync();

                return Ok(entretien);
            }

        }

        [HttpPut]
        [Route("refuse/{id:Guid}")]
        public async Task<IActionResult> RefuseInter(Guid id){
          var  entretien = await _contextDb.Entretiens.FindAsync(id);
            if(entretien == null){
                return BadRequest("id n'existe pas");
            }else{
                entretien.accept= "non";
               
                
                await _contextDb.SaveChangesAsync();

                return Ok(entretien);
            }

        }


}