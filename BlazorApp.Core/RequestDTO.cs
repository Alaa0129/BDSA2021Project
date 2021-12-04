using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Core
{
    public record RequestDTO(int Id, string Title, string Description, string StudentId, string SupervisorId);
    
    public record RequestDetailsDTO(int Id, string Title, string Description, StudentDTO Student, SupervisorDTO Supervisor);

    public record RequestCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string Title { get; init; }

        [Required]
        [StringLength(4400)]
        public string Description { get; init; }

        [Required]
        public string StudentId {get; init; }

        [Required]
        public string SupervisorId {get; init; }

    }
    public record RequestUpdateDTO : RequestCreateDTO
    {
        public int Id {get; init; }
    }
}