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
    public class DeckCard
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Card _card;

        /// <summary>
        /// If a Card is assigned, return it. Else, find the Card in the CardList based upon the CardName
        /// </summary>
        public Card Card
        {
            get
            {
                if (_card != null) return _card;
                else
                {

                    _card = CardList.List.Find(c => c.Name == CardName);

                    return _card;
                }
            }

            set
            {
                _card = value;
            }

        }
        #endregion

        
    }
}
