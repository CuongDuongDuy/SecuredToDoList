using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SecuredToDoList.Api.Models
{
    public class TodoItemEditModel
    {
        [Required]
        [MaxLength(2000)]
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public bool IsPublic { get; set; }
        [MaxLength(2000)]
        [DataType(DataType.EmailAddress)]
        public string AttendeeEmail { get; set; }
    }
}