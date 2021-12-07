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
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        // [MinLength(10)]
        public string Name { get; set; }
    }

    public record StudentUpdateDTO : StudentCreateDTO
    {

    }

}
