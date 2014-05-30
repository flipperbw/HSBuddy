using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneLogData.Models
{
    public class Zone
    {
        [Key]
        public int ZoneID { get; set; }
        public string Desc { get; set; }


        public static List<Zone> GetSeedData()
        {
            List<Zone> listZone = new List<Zone>();

            listZone.Add(new Zone() { ZoneID = 1, Desc = "FRIENDLY DECK" });
            listZone.Add(new Zone() { ZoneID = 2, Desc = "FRIENDLY HAND" });
            listZone.Add(new Zone() { ZoneID = 3, Desc = "FRIENDLY PLAY" });
            listZone.Add(new Zone() { ZoneID = 4, Desc = "FRIENDLY GRAVEYARD" });
            listZone.Add(new Zone() { ZoneID = 5, Desc = "OPPOSING DECK" });
            listZone.Add(new Zone() { ZoneID = 6, Desc = "OPPOSING HAND" });
            listZone.Add(new Zone() { ZoneID = 7, Desc = "OPPOSING PLAY" });
            listZone.Add(new Zone() { ZoneID = 8, Desc = "OPPOSING GRAVEYARD" });

            listZone.Add(new Zone() { ZoneID = 9, Desc = "DECK" });
            listZone.Add(new Zone() { ZoneID = 10, Desc = "HAND" });
            listZone.Add(new Zone() { ZoneID = 11, Desc = "PLAY" });
            listZone.Add(new Zone() { ZoneID = 12, Desc = "GRAVEYARD" });
            listZone.Add(new Zone() { ZoneID = 13, Desc = " " });

            return listZone;

        }
    }
}
