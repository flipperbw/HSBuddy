using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQtoCSV;

namespace hearthdecktracker.data
{
    [Serializable]
    public class CardList
    {
        //Implemented a poor-man's Singleton.

        #region Properties
        private static List<Card> _list;
        public static List<Card> List 
        { 
            get 
            {
                if (_list == null)
                    _list = GetCardList();

                return _list; 
            } 
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Get the card list, using the default CSV path
        /// </summary>
        private static List<Card> GetCardList()
        {
            //TODO: Move default path to a config file
            return GetCardList(Directory.GetCurrentDirectory().ToString() + @"\data\hd.csv");
        }

        /// <summary>
        /// Get the card list with a specified csvpath
        /// </summary>
        /// <param name="csvPath">Location on the HDD of the card data csv</param>
        private static List<Card> GetCardList(string csvPath)
        {
            List<Card> cardlist = new List<Card>();
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                IgnoreTrailingSeparatorChar = true,
                IgnoreUnknownColumns = true
            };

            CsvContext cc = new CsvContext();
            IEnumerable<Card> allTheCards = cc.Read<Card>(csvPath, inputFileDescription);

            var cardList = new List<Card>();

            foreach (Card c in allTheCards)
            {
                cardlist.Add(c);
            }

            return cardlist;
        }
        #endregion
    }
}
