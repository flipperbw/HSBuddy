using HearthstoneLogData.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneLogData.DAL
{
    public class HearthstoneLogDataInitializer : DropCreateDatabaseIfModelChanges<HearthstoneLogDataContext>
    {
        protected override void Seed(HearthstoneLogDataContext context)
        {
            //Seed zones
            var zones = Zone.GetSeedData();
            zones.ForEach(s => context.Zones.Add(s));
            context.SaveChanges();

            //Seed match types
            var matchTypes = MatchType.GetSeedData();
            matchTypes.ForEach(s => context.MatchTypes.Add(s));
            context.SaveChanges();
        }
    }
}
