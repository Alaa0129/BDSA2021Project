using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Infrastructure
{
    public class Request
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public int StudentId { get; set; }

        public ICollection<User> AppliedStudents{ get; set; }
    }
}