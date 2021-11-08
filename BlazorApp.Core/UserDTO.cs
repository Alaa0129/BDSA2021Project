using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Core
{
    public record UserDTO(int Id, string Firstname, string Lastname);

    public record UserDetailsDTO(int Id, string Firstname, string Lastname);

    public record UserCreateDTO
    {
        [StringLength(50)]
        public string Firstname { get; init; }

        [StringLength(50)]
        public string Lastname { get; init; }
    }
}
