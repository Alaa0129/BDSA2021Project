using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Core
{
    public record RequestDTO(int Id, string Title, string Description, int StudentId);
    
    public record RequestDetailsDTO(int Id, string Title, string Description, int StudentId);

    public record RequestCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string Title { get; init; }

        [Required]
        public string Description { get; init; }

        [Required]
        public int StudentId {get; init; }

    }
    public record RequestUpdateDTO : RequestCreateDTO
    {
        public int Id {get; init; }
    }
}