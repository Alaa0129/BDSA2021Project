using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Core
{
    public record SupervisorDTO(string Id, string Name);

    public record SupervisorDetailsDTO(string Id, string Name, ICollection<ProjectDTO> Projects, ICollection<RequestDTO> Requests);

    public record SupervisorCreateDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        [MinLength(10)]
        public string Name { get; set; }
    }

    public record SupervisorUpdateDTO : SupervisorCreateDTO
    {

    }

}
