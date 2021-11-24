using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Infrastructure
{
    public class Project
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        public int SupervisorId { get; set; }

        [Required]
        public int MaxApplications { get; set; }

        public ICollection<User> AppliedStudents{ get; set; }

        // Tag entity need to be set up
        //public ICollection<Tags> tags { get; set; }
    }
}