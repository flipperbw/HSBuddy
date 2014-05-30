using HearthstoneLogData.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneLogData.DAL
{
    public class HearthstoneLogDataContext : DbContext
    {
        public HearthstoneLogDataContext()
            : base("HearthstoneLogDataContext")
        {
        }

        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchType> MatchTypes { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<ZoneLocalTrigger> ZoneLocalTriggers { get; set; }
        public DbSet<ZonePowerTask> ZonePowerTasks { get; set; }
        public DbSet<ZoneEntity> ZoneEntities { get; set; }
        public DbSet<ZoneChange> ZoneChanges { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
