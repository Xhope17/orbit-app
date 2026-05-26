using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XClone.Application.Helpers;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.Requets.Post;
using XClone.Shared.Constants;

namespace XClone.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController(IPostService postService) : ControllerBase
    {

        //Crear
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest model)
        {
            if (model.AuthorId == Guid.Empty)
            {
                return BadRequest(ResponseHelper.Create<string>(null, null, null, ValidationConstants.AUTHOR_ID_REQUIRED));
            }

            var rsp = await postService.Create(model);

            return Ok(rsp);
        }

        //obtener todos los post
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterPostRequest model)
        {
            var rsp = postService.Get(model);

            return Ok(rsp);
        }

        //obtener un post por id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var rsp = await postService.Get(id);
            return Ok(rsp);
        }

        //Actualizar falta
        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdatePostRequest model, Guid id)
        {
            var rsp = await postService.Update(id, model);

            return Ok(rsp);
        }


        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var rsp = await postService.Delete(id);
            return Ok(rsp);
        }
    }
}
