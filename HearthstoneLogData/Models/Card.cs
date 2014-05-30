using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneLogData.Models
{
    public class Card
    {
        [Key]
        public string CardID { get; set; }
        public string Name { get; set; }
    }
}
