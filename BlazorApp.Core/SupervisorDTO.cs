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
        public string Id { get; init; }

        [StringLength(50)]
        public string Name { get; init; }
    }

    public record SupervisorUpdateDTO : SupervisorCreateDTO
    {

    }

}
