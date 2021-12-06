using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Infrastructure
{
    public class Supervisor
    {
        [Key]
        public string Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}