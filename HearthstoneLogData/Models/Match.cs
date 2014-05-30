using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneLogData.Models
{
    public class Match
    {
        [Key]
        public int MatchID { get; set; }
        public DateTime MatchStart { get; set; }
        public DateTime MatchEnd { get; set; }

        public string FriendlyHeroID { get; set; }
        [ForeignKey("FriendlyHeroID")]
        public Card FriendlyHero { get; set; }

        public string FriendlyHeroPowerID { get; set; }
        [ForeignKey("FriendlyHeroPowerID")]
        public Card FriendlyHeroPower { get; set; }

        public string OpposingHeroID { get; set; }
        [ForeignKey("OpposingHeroID")]
        public Card OpposingHero { get; set; }

        public string OpposingHeroPowerID { get; set; }
        [ForeignKey("OpposingHeroPowerID")]
        public Card OpposingHeroPower { get; set; }
    }
}
