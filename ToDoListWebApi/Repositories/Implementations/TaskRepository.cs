using Microsoft.EntityFrameworkCore;
using ToDoListWebApi.Data;

namespace ToDoListWebApi.Repositories.Implementations;

public class TaskRepository : ITaskRepository
{
  private readonly ToDoContext _context;
  public TaskRepository(ToDoContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
  {
    var tasks = await _context.Tasks.ToListAsync();

    foreach (var task in tasks)
    {
      UpdateOverdueStatus(task);
    }

    return tasks;
  }

  public async Task<Models.Task> GetTaskByIdAsync(int id)
  {
    var task = await _context.Tasks.FindAsync(id);
    if (task != null)
    {
      UpdateOverdueStatus(task);
    }
    return task;
  }

  public async Task<Models.Task> AddTaskAsync(Models.Task task)
  {
    UpdateOverdueStatus(task);
    _context.Tasks.Add(task);
    await _context.SaveChangesAsync();
    return task;
  }

  public async Task<bool> UpdateTaskAsync(Models.Task task)
  {
    UpdateOverdueStatus(task);
    _context.Tasks.Update(task);
    return await _context.SaveChangesAsync() > 0;
  }

  public async Task<bool> DeleteTaskAsync(int id)
  {
    var task = await _context.Tasks.FindAsync(id);
    if (task == null) return false;

    _context.Tasks.Remove(task);
    return await _context.SaveChangesAsync() > 0;
  }

  private void UpdateOverdueStatus(Models.Task task)
  {
    if (task.DueDate.HasValue && task.DueDate.Value < DateTime.Now && task.Status != Enums.TaskStatus.Overdue)
    {
      task.Status = Enums.TaskStatus.Overdue;
    }
  }
}
