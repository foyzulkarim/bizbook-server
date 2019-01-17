using System.Data.Entity.Migrations;
using Server.Identity.Models;

namespace Server.Identity.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SecurityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SecurityDbContext context)
        {
            
        }
    }
}
