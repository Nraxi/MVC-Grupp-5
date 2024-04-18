using Microsoft.EntityFrameworkCore;
using MVC_Grupp_5.Models;

namespace MVC_Grupp_5.Data
{
    public class MVC_Grupp_5Context : DbContext
    {
        public MVC_Grupp_5Context (DbContextOptions<MVC_Grupp_5Context> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicle { get; set; } = default!;
    }
}
