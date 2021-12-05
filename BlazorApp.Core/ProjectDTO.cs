using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BlazorApp.Core
{
    public record ProjectDTO(int Id, string Title, string Description, int SupervisorId);

    public record ProjectDetailsDTO(int Id, string Title, string Description, int SupervisorId, int MaxApplications, ICollection<string> Tags);

    public record ProjectCreateDTO 
    {
        [Required]
        [StringLength(50)]
        public string Title { get; init; }

        [Required]
        [StringLength(4400)]
        public string Description { get; init; }

        // [Required]
        public int MaxApplications {get; init; }
        
        [Required]
        public int SupervisorId {get; init; }

        public ICollection<string> Tags { get; set; }

    }

    public record ProjectUpdateDTO : ProjectCreateDTO
    {
        public int Id {get; init; }
    }


}
