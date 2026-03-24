using Microsoft.AspNetCore.Mvc;
using XClone.Application;

namespace XClone.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {

        //Crear
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest model)
        {
            return Ok($"Post subido: autor: {model.AutorId}, contenido: {model.Texto}");
        }

        //obtener todos los post
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPostRequest model)
        {
            return Ok($"Todos los usuarios: limit: {model.Limit}, outset: {model.Offset}, gitlabprofile: {model.GitlabProfile}");
        }

        //obtener un post
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok($"{id}");
        }

        //Actualizar
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromBody] UpdatePostRequest model)
        {
            return Ok($"usuario actualizado: Contenido: {model.Texto}");
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok("usuario eliminado");
        }
    }
}
