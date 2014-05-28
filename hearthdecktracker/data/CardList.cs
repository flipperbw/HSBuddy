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
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public List<Card> List { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Get the card list, using the default CSV path
        /// </summary>
        public CardList()
        {
            //TODO: Move default path to a config file
            new CardList(Directory.GetCurrentDirectory().ToString() + @"\data\hd.csv");
        }

        /// <summary>
        /// Get the card list with a specified csvpath
        /// </summary>
        /// <param name="csvPath">Location on the HDD of the card data csv</param>
        public CardList(string csvPath)
        {
            if (List == null)
            {
                List = new List<Card>();

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
                    this.List.Add(c);
                }
            }
        }
        #endregion
    }
}
