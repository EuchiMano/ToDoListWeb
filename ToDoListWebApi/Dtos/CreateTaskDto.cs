using System.ComponentModel.DataAnnotations;
using ToDoListWebApi.Helpers;

namespace ToDoListWebApi.Dtos;

public class CreateTaskDto
{
  [Required]
  [MaxLength(250)]
  public string Description { get; set; }

  [FutureDate(ErrorMessage = "DueDate must be in the future.")]
  public DateTime? DueDate { get; set; }

  [EnumDataType(typeof(Enums.TaskStatus))]
  public Enums.TaskStatus Status { get; set; }
}
