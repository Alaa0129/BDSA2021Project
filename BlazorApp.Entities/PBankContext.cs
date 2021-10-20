using System.IO;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Entities 
{
    public class PBankContext : DbContext
    {
        public PBankContext(DbContextOptions<PBankContext> options) : base(options) { }

        
    }
}