using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDo.API.DTOs;
using ToDo.API.Models;
using ToDo.API.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ToDo.API.Context;
using AutoMapper;
using ToDo.API.Error;
using System.Threading.Tasks;
namespace ToDo.API.Controllers
{

    
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IGenericRepository<DoTask> _taskRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public TasksController(IGenericRepository<DoTask> taskRepo, IHttpContextAccessor httpContextAccessor,
            AppDbContext dbContext,
            IMapper mapper)
        {
            _taskRepo = taskRepo;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        //add task
        
        [HttpPost]
        public async Task<ActionResult<TaskDTO>> AddTask(CreateTaskDTO model)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Actor) ?? "0";
            if (userId == "0")
            {
                return NotFound(new ApiResponse(401,"un authorized"));
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return NotFound(new ApiResponse(401));
            }
            var task = new DoTask
            {

                Title = model.Title,
                Description = model.Description,
                StartAt = DateTime.UtcNow,
                EndAt = DateTime.UtcNow,
                UserId = userId,
                User = user

            };

            await _taskRepo.AddAsync(task);
            var mappedTask = _mapper.Map<DoTask, TaskDTO>(task);

            return Ok(new
            {
                Message = "Task added successfully!",
                Task = mappedTask
            });
        }
        [HttpPut("changeStatus/{taskId}")]
        public async Task<IActionResult> ChangeTaskStatus(int taskId, [FromQuery] int status)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Actor) ?? "0";
            if (userId == "0")
            {
                return NotFound(new ApiResponse(401));
            }
            var Task = await _taskRepo.GetByIdAsync(taskId);

            if (Task == null)
            {
                return NotFound(new ApiResponse(404)); ;
            }

            if (!Enum.IsDefined(typeof(TasksStatus), status))
            {
                return NotFound(new ApiResponse(400,"Invalid Status")); 
            }


            Task.Status = (TasksStatus)status;


            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = "An error occurred while updating the task status.",
                    Details = ex.InnerException?.Message ?? ex.Message
                });
            }

            return Ok(new
            {
                Message = "Task status updated successfully!",
                TaskId = taskId,
                NewStatus = Task.Status
            });
        }

        //Get Tasks
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Actor) ?? "0";
            if (userId == "0")
            {
                return Unauthorized(new ApiResponse(401));
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return NotFound(new ApiResponse(401));
            }
            var Tasks=await _taskRepo.GetAllAsync(userId);
            if (Tasks == null || !Tasks.Any())
            {
                return NotFound(new ApiResponse(404,"Tasks Not Found"));
            }
            
            var mappedTas = _mapper.Map<IEnumerable<DoTask>, IEnumerable<TaskDTO>>(Tasks);
            return Ok(mappedTas);

        }
        ////GET task by id
        [HttpGet("{id}")]
        public async Task<ActionResult<CreateTaskDTO>> GetById(int id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Actor) ?? "0";
            if (userId == "0")
            {
                return NotFound(new ApiResponse(401));
            }
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null || task.UserId != userId)
            {
                return NotFound(new ApiResponse(404, "task not found"));
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return NotFound(new ApiResponse(401));
            }
           var mappedTask= _mapper.Map<DoTask, ReturnedTaskFromUpdateDTO>(task);
            return Ok(mappedTask);
        }

        //update task
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(int id, [FromBody] UpdatedTaskDTO  model)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Actor) ?? "0";
            if (userId == "0")
            {
                return NotFound(new ApiResponse(401));
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return NotFound(new ApiResponse(401));
            }
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null || task.UserId != userId)
            {
                return NotFound(new ApiResponse(404,"task not found"));
            }
            if (!Enum.IsDefined(typeof(TasksStatus), model.Status))
            {
                return BadRequest(new ApiResponse(400, "Invalid status value."));
            }
            task.Title = model.Title;
            task.Description = model.Description;
            task.StartAt=model.StartAt;
            task.EndAt=model.EndAt;
            task.Status=model.Status;
            
            _taskRepo.Update(task);
            var mappedTask=_mapper.Map<DoTask,ReturnedTaskFromUpdateDTO>(task);

            return Ok(new
            {
                Message = "Task Updated successfully!",
                Task = mappedTask
            });
        }
        ////delete task
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteTask(int id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Actor) ?? "0";
            if (userId == "0")
            {
                return NotFound(new ApiResponse(401,"un auhorized"));
            }
            var task = await _taskRepo.GetByIdAsync(id);

            if (task == null || task.UserId != userId)
            {
                return BadRequest(new ApiResponse(400, "Task Not Found"));
            }

            var Result = _taskRepo.Delete(task);
            if (Result)
                return Ok(new { message = "Deleted successfully" });
            else
                return BadRequest(new { message = "An error occurred while deleting the task." });

        }


    }
}
