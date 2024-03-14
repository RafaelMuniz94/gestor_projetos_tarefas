using AutoMapper;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gestor_Projetos_Tarefas.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IProjectsRepository projectsRepository;
        private readonly ITasksRepository tasksRepository;
        private readonly IUsersRepository usersRepository;
        private readonly IMapper mapper;

        public UserController(IProjectsRepository _projectsRepository, ITasksRepository _tasksRepository, IUsersRepository _usersRepository, IMapper _mapper)
        {
            this.projectsRepository = _projectsRepository;
            this.tasksRepository = _tasksRepository;
            this.usersRepository = _usersRepository;
            this.mapper = _mapper;
        }

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
