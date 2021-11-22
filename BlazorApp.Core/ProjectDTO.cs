using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BlazorApp.Core
{
    public record ProjectDTO(int Id, string Title, string Description, int SupervisorId);


    // student reference?
    public record ProjectDetailsDTO(int Id, string Title, string Description, int SupervisorId, int MaxApplications);

    public record ProjectCreateDTO
    {
        [StringLength(50)]
        public string Title { get; init; }

        public string Description { get; init; }

        [Required]
        public int MaxApplications {get; init; }
        
        [Required]
        public int SupervisorId {get; init; } 
    }

    public record ProjectUpdateDTO : ProjectCreateDTO
    {
        public int Id {get; init; }
    }


}
