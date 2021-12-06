using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Infrastructure
{
    public class Project
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
        public Supervisor Supervisor { get; set; } 

        [Required]
        public IList<Student> AppliedStudents{ get; set; } = new List<Student>();
        
        [Required]
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}