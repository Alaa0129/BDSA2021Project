using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Core
{
    public record StudentDTO(string Id, string Name);

    public record StudentDetailsDTO(string Id, string Name, ProjectDTO Project, ICollection<RequestDTO> Requests);

    public record StudentCreateDTO
    {
        [Required]
        public string Id { get; init; }

        [StringLength(50)]
        public string Name { get; init; }
    }

    public record StudentUpdateDTO : StudentCreateDTO
    {

    }

}
