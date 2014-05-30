using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneLogData.Models
{
    public class MatchType
    {
        [Key]
        public int MatchTypeID { get; set; }
        public string Desc { get; set; }


        public static List<MatchType> GetSeedData()
        {
            List<MatchType> listMatchType = new List<MatchType>();

            listMatchType.Add(new MatchType() { MatchTypeID = 1, Desc = "Play" });
            listMatchType.Add(new MatchType() { MatchTypeID = 2, Desc = "Arena" });
            listMatchType.Add(new MatchType() { MatchTypeID = 3, Desc = "Practice" });

            return listMatchType;

        }
    }
}
