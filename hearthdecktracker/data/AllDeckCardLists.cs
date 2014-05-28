using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace hearthdecktracker.data
{
    [Serializable]
    public class AllDeckCardLists
    {
        #region properties
        /// <summary>
        /// 
        /// </summary>
        public List<DeckCardList> DeckCardLists { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// Get all user-defined DeckCardLists from a json object.
        /// TODO: Write overrides, just like the CardList constuctor
        /// </summary>
        public AllDeckCardLists()
        {
            FileStream jsonFS = new FileStream(Directory.GetCurrentDirectory() + @"\data\AllDeckCardLists.json", FileMode.OpenOrCreate);

            StreamReader jsonSR = new StreamReader(jsonFS);


            string json = jsonSR.ReadToEnd();
            jsonSR.Close();

            DeckCardLists = JsonConvert.DeserializeObject<List<DeckCardList>>(json);
        }
        #endregion

        //TODO: Write a save method

        //TODO: Write a method to scrape the screen for decks, serialize and save as JSON
    }
}
