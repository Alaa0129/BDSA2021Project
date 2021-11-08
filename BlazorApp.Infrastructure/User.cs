using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Infrastructure
{
    public class User
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Lastname { get; set; }
    }
}
