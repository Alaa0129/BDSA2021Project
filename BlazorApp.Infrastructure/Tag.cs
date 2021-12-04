using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Infrastructure
{
    public class Tag
    {   
        
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Project> Projects { get; set; }

        public Tag(string name)
        {
            Name = name;
        }
        
    }
}