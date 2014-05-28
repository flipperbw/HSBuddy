using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQtoCSV;

namespace hearthdecktracker.data
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Card
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        [CsvColumn(Name = "cardname", FieldIndex = 1)]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CsvColumn(Name = "cardid", FieldIndex = 2)]
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CsvColumn(Name = "cardtext", FieldIndex = 3)]
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CsvColumn(Name = "mana", FieldIndex = 4)]
        public int Mana { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CsvColumn(Name = "atk", FieldIndex = 5)]
        public int Atk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CsvColumn(Name = "def", FieldIndex = 6)]
        public int Def { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CsvColumn(Name = "dmg", FieldIndex = 7)]
        public string Dmg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CsvColumn(Name = "heal", FieldIndex = 8)]
        public string Heal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CsvColumn(Name = "catk", FieldIndex = 9)]
        public string Catk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CsvColumn(Name = "to", FieldIndex = 10)]
        public string Targ { get; set; }
        #endregion
    }
}
