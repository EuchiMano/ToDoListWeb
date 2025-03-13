using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoListWebApi.Dtos;
using ToDoListWebApi.Repositories;

namespace ToDoListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        public TaskController(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var task = _mapper.Map<Models.Task>(createTaskDto);

            if (task.Status == Enums.TaskStatus.Overdue)
            {
                return BadRequest("Cannot manually set status to 'Overdue'. It will be updated automatically based on DueDate.");
            }

            var newTask = await _taskRepository.AddTaskAsync(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingTask = await _taskRepository.GetTaskByIdAsync(id);
            if (existingTask == null) return NotFound();

            if (updateTaskDto.Status == Enums.TaskStatus.Overdue)
            {
                return BadRequest("Cannot manually set status to 'Overdue'. It will be updated automatically based on DueDate.");
            }

            _mapper.Map(updateTaskDto, existingTask);

            bool updated = await _taskRepository.UpdateTaskAsync(existingTask);
            if (!updated) return StatusCode(500, "Error updating task.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var deleted = await _taskRepository.DeleteTaskAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
