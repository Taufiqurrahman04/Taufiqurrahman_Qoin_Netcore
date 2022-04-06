using Microsoft.EntityFrameworkCore;
using Taufiqurrahman_Test_Qoin.EntityFramework.Models;

namespace Taufiqurrahman_Test_Qoin.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Test01> Test01 { get; set; }
    }
}
