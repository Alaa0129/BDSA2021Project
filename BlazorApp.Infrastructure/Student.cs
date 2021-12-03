using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Infrastructure
{
    public class Student
    {
        [Key]
        public string Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public Project? project { get; set; }

        // public ICollection<Request> requests { get; set; }

    }
}
