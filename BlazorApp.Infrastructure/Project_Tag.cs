using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Infrastructure
{
    public class Project_Tag
    {
        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}