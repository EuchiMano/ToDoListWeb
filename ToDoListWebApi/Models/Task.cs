using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApi.Models;

public class Task
{
  [Key]
  public int Id { get; set; }

  [Required]
  [MaxLength(250)]
  public string Description { get; set; }

  public DateTime? DueDate { get; set; }

  public Enums.TaskStatus Status { get; set; }
}
