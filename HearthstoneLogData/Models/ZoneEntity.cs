using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneLogData.Models
{
    public class ZoneEntity
    {
        [Key]
        public int ZoneEntityID { get; set; }
        public int LogID { get; set; }

        public string CardID { get; set; }
        [ForeignKey("CardID")]
        public Card Card { get; set; }

        public int Player { get; set; }

        public int ZoneID { get; set; }
        [ForeignKey("ZoneID")]
        public Zone Zone { get; set; }

        public int ZonePosition { get; set; }
    }
}
