using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BlazorApp.Core
{
    public record ProjectDTO(int Id, string Title, string Description, string SupervisorId);


    // student reference?
    public record ProjectDetailsDTO(int Id, string Title, string Description, SupervisorDTO Supervisor, ICollection<StudentDTO> AppliedStudents, ICollection<string> Tags);

    public record ProjectCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string Title { get; init; }

        public string Description { get; set; }

        [Required]
        public string SupervisorId { get; set; }

        [Required]
        public ICollection<string> Tags { get; set; } = new List<string>();
    }

    public record ProjectUpdateDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }


}
