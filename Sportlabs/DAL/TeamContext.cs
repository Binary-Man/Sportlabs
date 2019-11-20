using SportsLabs.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SportsLabs.DAL
{
    public class TeamContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        
        public TeamContext()
            : base("name=DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TeamContext, Migrations.Configuration>());
        }

       
    }
}