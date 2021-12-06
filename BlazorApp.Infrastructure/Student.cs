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

        public Project Project { get; set; }

        public ICollection<Request> Requests { get; set; } = new List<Request>();

    }
}
