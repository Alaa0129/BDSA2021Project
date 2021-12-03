using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BlazorApp.Core
{
    public record TagDTO(int Id, string Name, ICollection<ProjectDTO> Projects);


    public record TagDetailsDTO(int Id, string Name, ICollection<ProjectDetailsDTO> Projects);

    public record TagCreateDTO
    {

        [Required]
        public string Name { get; init; }

        public ICollection<ProjectCreateDTO> Projects { get; init; }

    }

    public record TagUpdateDTO : TagCreateDTO
    {
        public int Id { get; init; }
    }


}
