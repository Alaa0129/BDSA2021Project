using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Infrastructure
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(4400)]
        public string Description { get; set; }

        [Required]
        public Student Student { get; set; }

        [Required]
        public Supervisor Supervisor { get; set; }
    }
}