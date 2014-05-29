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
        public static List<DeckCardList> DeckCardLists { get; set; }
        #endregion

        #region constructors

        public AllDeckCardLists()
        {

        }
        #endregion

        /// <summary>
        /// Get all user-defined DeckCardLists from the json list on first use,
        /// returns the local DeckCardLists otherwise
        /// TODO: Write overrides, just like the CardList constuctor
        /// </summary>
        public static List<DeckCardList> GetLists()
        {
            if (DeckCardLists == null)
            {
                FileStream jsonFS = new FileStream(Directory.GetCurrentDirectory() + @"\data\AllDeckCardLists.json", FileMode.OpenOrCreate);

                StreamReader jsonSR = new StreamReader(jsonFS);


                string json = jsonSR.ReadToEnd();
                jsonSR.Close();

                DeckCardLists = JsonConvert.DeserializeObject<List<DeckCardList>>(json);
            }
            return DeckCardLists;
        }

        /// <summary>
        /// Added functionality to add a list to the list of lists... Yeah, need better names.
        /// </summary>
        /// <param name="deckCardList"></param>
        public void AddList(DeckCardList deckCardList)
        {
            DeckCardLists.Add(deckCardList);
        }

        /// <summary>
        /// Save the lists back out to the JSON file.
        /// </summary>
        public void SaveLists()
        {
            if (DeckCardLists != null)
            {
                FileStream jsonFS = new FileStream(Directory.GetCurrentDirectory() + @"\data\AllDeckCardLists.json", FileMode.OpenOrCreate);

                string json = JsonConvert.SerializeObject(DeckCardLists);

                using (StreamWriter sw = new StreamWriter(jsonFS))
                {
                    sw.Write(json);
                }
            }
        }

        //TODO: Write a method to scrape the screen for decks, serialize and save as JSON
        //Probably shouldn't live here, but it would be nice to have it.
    }
}
