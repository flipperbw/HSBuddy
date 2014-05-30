using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneLogData.Models
{
    public class ZoneChange
    {
        [Key]
        public int ZoneChangeID { get; set; }
        public int LogID { get; set; }
        public int Changes { get; set; }
        public bool Complete { get; set; }
        public bool Local { get; set; }

        public int ZoneLocalTriggerID { get; set; }
        [ForeignKey("ZoneLocalTriggerID")]
        public ZoneLocalTrigger ZoneLocalTrigger { get; set; }

        public int ZoneFromID { get; set; }
        [ForeignKey("ZoneFromID")]
        public Zone ZoneFrom { get; set; }

        public int ZoneToID { get; set; }
        [ForeignKey("ZoneToID")]
        public Zone ZoneTo { get; set; }

        public int MatchID { get; set; }
        [ForeignKey("MatchID")]
        public Match Match { get; set; }

        public int ZoneEntityID { get; set; }
        [ForeignKey("ZoneEntityID")]
        public ZoneEntity ZoneEntity { get; set; }
    }
}
