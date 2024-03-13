using Microsoft.AspNetCore.Mvc;

namespace Gestor_Projetos_Tarefas.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        [HttpGet("{userID}")]
        public async Task<IActionResult> ReturnProjects(Guid userID)
        {
            return Ok();
        }

        [HttpGet("{projectID}")]
        public async Task<IActionResult> ReturnTasks(Guid userID)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject()
        {
            return Ok();
        }

    }
}
