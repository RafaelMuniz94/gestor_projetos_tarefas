using Microsoft.AspNetCore.Mvc;

namespace Gestor_Projetos_Tarefas.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpDelete("{userID}/{projectID}")]
        public async Task<IActionResult> RemoveProject(Guid userID, Guid projectID)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GeneratePerformanceReport(Guid userID)
        {
            return Ok();
        }
    }
}
