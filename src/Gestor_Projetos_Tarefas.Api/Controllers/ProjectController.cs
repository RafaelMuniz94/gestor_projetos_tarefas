using Gestor_Projetos_Tarefas.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gestor_Projetos_Tarefas.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private List<Project> projects = new List<Project>();
        private List<User> users = new List<User>();

        [HttpGet("{userID}")]
        public async Task<IActionResult> ReturnProjects(Guid userID)
        {
            if(userID == Guid.Empty)
                return BadRequest("The user ID must be valid!");
            
            User user = users.FirstOrDefault(user => user.ID == userID);

            return Ok(user.Projects);
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
