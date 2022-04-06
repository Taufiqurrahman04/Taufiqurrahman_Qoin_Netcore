using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore;
using Taufiqurrahman_Test_Qoin.ServiceWorker.Models;

namespace Taufiqurrahman_Test_Qoin.EntityFramework
{
    public class AppDbContext : DbContext
    {
        private const string _connString = "server=localhost;user id=root;password=admin;database=Taufiqurrahman_QoinTest";
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql(_connString,
                ServerVersion.AutoDetect(_connString));
        }

        public virtual DbSet<Test01> Test01 { get; set; }
    }
}
