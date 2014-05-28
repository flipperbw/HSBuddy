using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hearthdecktracker.data
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DeckCardList
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<DeckCard> DeckCards { get; set; }
        #endregion
    }
}
