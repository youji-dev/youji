using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Represents the abstract controller base class where all CRUD operations are defined.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class CRUDController() : ControllerBase
    {
        /// <summary>
        /// Abstract endpoint that gets one entity.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<IActionResult> Get();

        /// <summary>
        /// Abstract endpoint that gets many entity.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<IActionResult> GetMany();

        /// <summary>
        /// Abstract endpoint that adds one entity.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<IActionResult> Post();

        /// <summary>
        /// Abstract endpoint that updates one entity.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<IActionResult> Put();

        /// <summary>
        /// Abstract endpoint that deletes one entity.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<IActionResult> Delete();
    }
}
