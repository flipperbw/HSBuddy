using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneLogData.Models
{
    public class ZoneLocalTrigger
    {
        [Key]
        public int ZoneLocalTriggerID { get; set; }

        public int ZonePowerTaskID { get; set; }
        [ForeignKey("ZonePowerTaskID")]
        public ZonePowerTask ZonePowerTask { get; set; }

        public int ZoneEntityID { get; set; }
        [ForeignKey("ZoneEntityID")]
        public ZoneEntity ZoneEntity { get; set; }

        public int SourceZoneID { get; set; }
        [ForeignKey("SourceZoneID")]
        public Zone SourceZone { get; set; }

        public int SourcePosition { get; set; }

        public int DestinationZoneID { get; set; }
        [ForeignKey("DestinationZoneID")]
        public Zone DestinationZone { get; set; }

    }
}
