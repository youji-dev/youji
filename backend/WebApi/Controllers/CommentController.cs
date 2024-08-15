using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Represents the controller that provides operations for ticket comments.
    /// </summary>
    [ApiController]
    public class CommentController : CRUDController
    {
        [HttpGet]
        public override Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public override Task<IActionResult> GetMany()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public override Task<IActionResult> Post()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public override Task<IActionResult> Put()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public override Task<IActionResult> Delete()
        {
            throw new NotImplementedException();
        }
    }
}
