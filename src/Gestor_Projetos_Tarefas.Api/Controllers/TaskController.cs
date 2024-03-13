using Microsoft.AspNetCore.Mvc;

namespace Gestor_Projetos_Tarefas.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        [HttpPost("{projectID}")]
        public async Task<IActionResult> CreateTask(Guid projectID)
        {
            return Ok();
        }

        [HttpPut("{taskID}")]
        public async Task<IActionResult> UpdateTask(Guid taskID)
        {
            return Ok();
        }

        [HttpDelete("{taskID}")]
        public async Task<IActionResult> DeleteTask(Guid taskID)
        {
            return Ok();
        }
    }
}
