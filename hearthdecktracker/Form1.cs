using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using LINQtoCSV;
using System.IO;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using BobNetProto;
using PegasusGame;
using PegasusShared;
using PegasusUtil;

namespace hearthdecktracker
{
    public partial class Form1 : Form
    {
        private bool dragging;
        private Point offset;

        private static SortedDictionary<int, ConnectAPI.PacketDecoder> s_packetDecoders = new SortedDictionary<int, ConnectAPI.PacketDecoder>();
        private static IBattleNet s_impl = new BattleNetDll();

        public class Card
        {
            [CsvColumn(Name = "cardname", FieldIndex = 1)]
            public string Name { get; set; }
            [CsvColumn(Name = "cardtext", FieldIndex = 2)]
            public string Text { get; set; }
            [CsvColumn(Name = "mana", FieldIndex = 3)]
            public int Mana { get; set; }
            [CsvColumn(Name = "atk", FieldIndex = 4)]
            public int Atk { get; set; }
            [CsvColumn(Name = "def", FieldIndex = 5)]
            public int Def { get; set; }
            [CsvColumn(Name = "dmg", FieldIndex = 6)]
            public string Dmg { get; set; }
            [CsvColumn(Name = "heal", FieldIndex = 7)]
            public string Heal { get; set; }
            [CsvColumn(Name = "catk", FieldIndex = 8)]
            public string Catk { get; set; }
            [CsvColumn(Name = "to", FieldIndex = 9)]
            public string Targ { get; set; }
        }

        public static List<Card> Allcards = new List<Card> { 

        };

        public static void readcards()
        {
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                IgnoreTrailingSeparatorChar = true,
                IgnoreUnknownColumns = true
            };
            CsvContext cc = new CsvContext();
            IEnumerable<Card> allthecards = cc.Read<Card>(Path.Combine(Directory.GetCurrentDirectory(), "\\hd.csv"), inputFileDescription);
            
            foreach (Card c in allthecards) {
                Allcards.Add(c);
            }
        }

        public class Deckcard
        {
            public int Amount { get; set; }
            public Card carddetails { get; set; }
            public Deckcard()
            {
                carddetails = new Card();
            }
        }

        public class Decklist
        {
            public string Name { get; set; }
            public List<Deckcard> Cardlist { get; set; }
            public Decklist()
            {
                Cardlist = new List<Deckcard>();
            }
        }

        List<Decklist> Alldecklists = new List<Decklist>();


        /****************************
         * DECK LISTS ARE HERE
         ***************************/

        public void setlists()
        {
            Alldecklists.Add(new Decklist {
                Name = "Choose Deck...",
                Cardlist = null
            });
            Alldecklists.Add(new Decklist {
                Name = "Warrior (My Control)",
                Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Fiery War Axe")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Shield Slam")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Execute")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Whirlwind")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Slam")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Armorsmith")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Cruel Taskmaster")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Shield Block")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Acolyte of Pain")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Frothing Berserker")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Spellbreaker")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Stampeding Kodo")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Faceless Manipulator")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Cairne Bloodhoof")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "The Black Knight")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Baron Geddon")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Grommash Hellscream")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Ragnaros the Firelord")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Alexstrasza")}
                }
            });
            Alldecklists.Add(new Decklist
            {
                 Name = "Warrior (Control)",
                 Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Fiery War Axe")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Shield Slam")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Execute")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Whirlwind")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Slam")}, // put an alternate in herecarddetails = Allcards.Find(r => r.Name == "Slam")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Armorsmith")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Cruel Taskmaster")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Shield Block")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Acolyte of Pain")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Big Game Hunter")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Frothing Berserker")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Brawl")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Faceless Manipulator")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Cairne Bloodhoof")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Sylvanas Windrunner")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Baron Geddon")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Grommash Hellscream")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Ragnaros the Firelord")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Alexstrasza")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Ysera")}
                }
            });
            Alldecklists.Add(new Decklist {
                Name = "Warlock (Zoo)",
                Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Soulfire")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Abusive Sergeant")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Argent Squire")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Flame Imp")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Shieldbearer")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Voidwalker")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Young Priestess")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Amani Berserker")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Dire Wolf Alpha")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Knife Juggler")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "King Mukla")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Shattered Sun Cleric")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Dark Iron Dwarf")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Defender of Argus")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Doomguard")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "The Black Knight")}
                }
            });
            Alldecklists.Add(new Decklist
            {
                Name = "Warlock (Murlocs)",
                Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Soulfire")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Abusive Sergeant")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Flame Imp")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Mortal Coil")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Grimscale Oracle")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Murloc Raider")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Murloc Tidecaller")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Voidwalker")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Young Priestess")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Bluegill Warrior")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Knife Juggler")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Murloc Tidehunter")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Coldlight Seer")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Murloc Warleader")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Leeroy Jenkins")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Old Murk-Eye")}
            }
            });
            Alldecklists.Add(new Decklist {
                    Name = "Hunter (Sunshine)", Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Hunter's Mark") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Flare") }, // or 2
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Tracking") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Stonetusk Boar") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Timber Wolf") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Explosive Trap") }, // or 1
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Freezing Trap") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Unleash the Hounds") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Ironbeak Owl") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Scavenging Hyena") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Starving Buzzard") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Eaglehorn Bow") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Animal Companion") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Kill Command") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "King Mukla") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Houndmaster") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Leeroy Jenkins") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Savannah Highmane") }
            }});
            Alldecklists.Add(new Decklist
            {
                Name = "Hunter (Cycle)",
                Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Hunter's Mark") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Arcane Shot") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Flare") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Tracking") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Leper Gnome") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Stonetusk Boar") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Timber Wolf") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Explosive Trap") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Freezing Trap") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Unleash the Hounds") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Loot Hoarder") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Starving Buzzard") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Eaglehorn Bow") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Deadly Shot") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Kill Command") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Leeroy Jenkins") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "The Black Knight") }
            }
            });
            Alldecklists.Add(new Decklist
            {
                Name = "Hunter (Kolento)",
                Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Hunter's Mark") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Flare") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Tracking") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Stonetusk Boar") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Timber Wolf") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Unleash the Hounds") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Ironbeak Owl") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "River Crocolisk") }, //1
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Scavenging Hyena") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Starving Buzzard") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Animal Companion") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Deadly Shot") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Kill Command") },
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "King Mukla") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Houndmaster") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Stampeding Kodo") },
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Savannah Highmane") }
            }
            });
            Alldecklists.Add(new Decklist
            {
                Name = "Priest (Amaz)",
                Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Circle of Healing")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Power Word: Shield")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Northshire Cleric")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Shadow Word: Pain")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Acidic Swamp Ooze")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Sunfury Protector")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Wild Pyromancer")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Shadow Word: Death")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Thoughtsteal")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Earthen Ring Farseer")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Injured Blademaster")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Shadow Madness")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Auchenai Soulpriest")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Sen'jin Shieldmasta")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Holy Nova")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Holy Fire")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Cabal Shadow Priest")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Sylvanas Windrunner")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Ragnaros the Firelord")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Mind Control")}
            }
            });
            Alldecklists.Add(new Decklist
            {
                Name = "Druid (Watcher)",
                Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Innervate")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Mark of the Wild")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Wrath")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Ancient Watcher")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Sunfury Protector")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Bloodmage Thalnos")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Harvest Golem")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Big Game Hunter")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Savage Roar")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Chillwind Yeti")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Defender of Argus")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Swipe")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Keeper of the Grove")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Druid of the Claw")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Cairne Bloodhoof")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "The Black Knight")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Force of Nature")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Ancient of Lore")},
            }
            });
            Alldecklists.Add(new Decklist
            {
                Name = "Druid (Ramp)",
                Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Innervate")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Mark of the Wild")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Wild Growth")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Wrath")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Bloodmage Thalnos")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Healing Touch")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Big Game Hunter")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Swipe")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Keeper of the Grove")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Sen'jin Shieldmasta")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Druid of the Claw")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Faceless Manipulator")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Gadgetzan Auctioneer")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Cairne Bloodhoof")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Sunwalker")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "The Black Knight")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Ancient of Lore")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Ragnaros the Firelord")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Cenarius")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Ysera")},
            }
            });
            Alldecklists.Add(new Decklist
            {
                Name = "Rogue (Miracle)",
                Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Backstab")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Preparation")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Shadowstep")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Cold Blood")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Conceal")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Deadly Poison")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Blade Flurry")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Eviscerate")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Sap")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Shiv")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Bloodmage Thalnos")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Edwin VanCleef")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Acolyte of Pain")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Fan of Knives")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "SI:7 Agent")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Leeroy Jenkins")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Gadgetzan Auctioneer")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Assassin's Blade")}
            }
            });
            Alldecklists.Add(new Decklist
            {
                Name = "Rogue (Malygos)",
                Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Backstab")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Preparation")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Deadly Poison")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Sinister Strike")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Blade Flurry")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Eviscerate")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Sap")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Shiv")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Bloodmage Thalnos")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Loot Hoarder")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Edwin VanCleef")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Fan of Knives")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "SI:7 Agent")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Earthen Ring Farseer")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Gadgetzan Auctioneer")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Azure Drake")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Assassin's Blade")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Malygos")}
            }
            });
            Alldecklists.Add(new Decklist
            {
                Name = "Warlock (Handlock)",
                Cardlist = {
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Soulfire")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Mortal Coil")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Power Overwhelming")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Ancient Watcher")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Bloodmage Thalnos")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Ironbeak Owl")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Sunfury Protector")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Big Game Hunter")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Earthen Ring Farseer")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Shadowflame")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Defender of Argus")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Leeroy Jenkins")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Twilight Drake")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Faceless Manipulator")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Siphon Soul")},
                    new Deckcard { Amount = 1, carddetails = Allcards.Find(r => r.Name == "Alexstrasza")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Mountain Giant")},
                    new Deckcard { Amount = 2, carddetails = Allcards.Find(r => r.Name == "Molten Giant")}
            }
            });
            Alldecklists.Add(new Decklist
            {
                Name = "Mage (Aggro)",
                Cardlist = {
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Arcane Missiles"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Ice Lance"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Mirror Image"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Argent Squire"), Amount = 1 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Leper Gnome"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Mana Wyrm"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Frostbolt"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Bloodmage Thalnos"), Amount = 1 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Knife Juggler"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Loot Hoarder"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Sorcerer's Apprentice"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Arcane Intellect"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Wolfrider"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Fireball"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Water Elemental"), Amount = 2 },
                    new Deckcard { carddetails = Allcards.Find(r => r.Name == "Azure Drake"), Amount = 2 },
            }
            });
            Alldecklists.Add(new Decklist
            {
                Name = "Shaman (Do It)",
                Cardlist = {
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Earth Shock")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Lightning Bolt")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Rockbiter Weapon")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Stormforged Axe")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Windfury")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Acidic Swamp Ooze")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Bloodmage Thalnos")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Flametongue Totem")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Wild Pyromancer")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Feral Spirit")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Hex")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Lightning Storm")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Mana Tide Totem")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Unbound Elemental")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Defender of Argus")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Leeroy Jenkins")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Azure Drake")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Gadgetzan Auctioneer")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Argent Commander")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Cairne Bloodhoof")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Fire Elemental")},
            }
            });
            Alldecklists.Add(new Decklist
            {
                Name = "Paladin (Control)",
                Cardlist = {
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Argent Squire")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Equality")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Bloodmage Thalnos")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Sunfury Protector")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Wild Pyromancer")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Sword of Justice")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Aldor Peacekeeper")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Earthen Ring Farseer")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Harvest Golem")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Truesilver Champion")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Consecration")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Defender of Argus")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Azure Drake")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Stampeding Kodo")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Avenging Wrath")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Cairne Bloodhoof")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Ragnaros the Firelord")},
                    new Deckcard {Amount = 1, carddetails = Allcards.Find(r => r.Name == "Tirion Fordring")}
            }
            });
            /*
            Alldecklists.Add(new Decklist {
                Name = "Zoo", Cardlist = {
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")},
            }});
            */
        }

        public Form1()
        {
            readcards();
            setlists();
            InitializeComponent();
            comboBox1.DataSource = Alldecklists;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            dragging = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            offset = new Point(e.X, e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
           if (dragging)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }

        /*******************
         * Displaying stuff
         *******************/

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
            {
                update_decklist();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
            {
                update_decklist();
            }
        }

        float pct;

        private void InitializeTimer() {
            Timer t = new Timer();
            t.Interval = 10;
            pct = 0F;
            t.Tick += new EventHandler(timer1_Tick);
            t.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timer t = (Timer)sender;
            
            if (pct < 1F)
            {
                dataGridView1.CurrentRow.DefaultCellStyle.SelectionBackColor = dataGridView1.CurrentRow.DefaultCellStyle.SelectionBackColor.Interpolate(SystemColors.Window, pct);
                pct += 0.2F;
            }
            else
            {
                t.Enabled = false;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int oldamt;
            int chg;

            if (e.Button == MouseButtons.Left)
            {
                oldamt = int.Parse(dataGridView1.CurrentRow.Cells["Amt"].Value.ToString());
                chg = -1;
                if (oldamt != 0)
                {
                    dataGridView1.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Red;

                    int newamt = Math.Max(0, oldamt + chg);
                    dataGridView1.CurrentRow.Cells["Amt"].Value = newamt;

                    InitializeTimer();

                    if (newamt == 0)
                    {
                        dataGridView1.CurrentRow.DefaultCellStyle.ForeColor = Color.DarkGray;
                        dataGridView1.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.DarkGray;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                dataGridView1.ClearSelection();
                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dataGridView1.Rows[e.RowIndex].Selected = true;
                dataGridView1.Focus();

                oldamt = int.Parse(dataGridView1.CurrentRow.Cells["Amt"].Value.ToString());
                chg = 1;

                if (oldamt != 2)
                {
                    dataGridView1.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Green;

                    int newamt = Math.Max(0, oldamt + chg);
                    dataGridView1.CurrentRow.Cells["Amt"].Value = newamt;

                    InitializeTimer();

                    dataGridView1.CurrentRow.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                    dataGridView1.CurrentRow.DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
                }
            }
            else
            {
                chg = 0;
            }
        }

        private void update_decklist()
        {
            dataGridView1.Rows.Clear();

            foreach (Deckcard card in (IEnumerable<Deckcard>)comboBox1.SelectedValue)
            {
                string ad;
                if (card.carddetails.Atk == 0 && card.carddetails.Def == 0)
                {
                    ad = "---";
                }
                else {
                    ad = card.carddetails.Atk + "/" + card.carddetails.Def;
                }
                dataGridView1.Rows.Add(
                    Convert.ToInt32(card.carddetails.Mana),
                    card.carddetails.Name,
                    ad,
                    card.carddetails.Dmg + "|" + card.carddetails.Heal + "|" + card.carddetails.Catk,
                    card.carddetails.Targ,
                    Convert.ToInt32(card.Amount)
                );
            }

            dataGridView1.Visible = true;
        }

        private void readtcpdata()
        {
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            PacketDevice selectedDevice = allDevices[0]; //3

            s_packetDecoders.Add(1, new ConnectAPI.DefaultProtobufPacketDecoder<GetGameState, GetGameState.Builder>());
            s_packetDecoders.Add(2, new ConnectAPI.DefaultProtobufPacketDecoder<ChooseOption, ChooseOption.Builder>());
            s_packetDecoders.Add(3, new ConnectAPI.DefaultProtobufPacketDecoder<ChooseEntities, ChooseEntities.Builder>());
            s_packetDecoders.Add(4, new ConnectAPI.DefaultProtobufPacketDecoder<Precast, Precast.Builder>());
            s_packetDecoders.Add(6, new ConnectAPI.DefaultProtobufPacketDecoder<ClientPacket, ClientPacket.Builder>());
            s_packetDecoders.Add(11, new ConnectAPI.DefaultProtobufPacketDecoder<GiveUp, GiveUp.Builder>());
            s_packetDecoders.Add(13, new ConnectAPI.DefaultProtobufPacketDecoder<ForcedEntityChoice, ForcedEntityChoice.Builder>());
            s_packetDecoders.Add(100, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasPlayer, AtlasPlayer.Builder>());
            s_packetDecoders.Add(101, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasError, AtlasError.Builder>());
            s_packetDecoders.Add(102, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasCollection, AtlasCollection.Builder>());
            s_packetDecoders.Add(103, new ConnectAPI.DefaultProtobufPacketDecoder<AutoLogin, AutoLogin.Builder>());
            s_packetDecoders.Add(104, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasDecks, AtlasDecks.Builder>());
            s_packetDecoders.Add(105, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasSuccess, AtlasSuccess.Builder>());
            s_packetDecoders.Add(106, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasOrders, AtlasOrders.Builder>());
            s_packetDecoders.Add(107, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAchieves, AtlasAchieves.Builder>());
            s_packetDecoders.Add(108, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAchieveInfo, AtlasAchieveInfo.Builder>());
            s_packetDecoders.Add(109, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasBoosters, AtlasBoosters.Builder>());
            s_packetDecoders.Add(110, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasDrafts, AtlasDrafts.Builder>());
            s_packetDecoders.Add(111, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasCurrencyDetails, AtlasCurrencyDetails.Builder>());
            s_packetDecoders.Add(112, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasCardBacks, AtlasCardBacks.Builder>());
            s_packetDecoders.Add(113, new ConnectAPI.DefaultProtobufPacketDecoder<BeginPlaying, BeginPlaying.Builder>());
            s_packetDecoders.Add(123, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleCommand, DebugConsoleCommand.Builder>());
            s_packetDecoders.Add(124, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleResponse, DebugConsoleResponse.Builder>());
            s_packetDecoders.Add(125, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleGetCmdList, DebugConsoleGetCmdList.Builder>());
            s_packetDecoders.Add(142, new ConnectAPI.DefaultProtobufPacketDecoder<DebugPaneNewItems, DebugPaneNewItems.Builder>());
            s_packetDecoders.Add(143, new ConnectAPI.DefaultProtobufPacketDecoder<DebugPaneDelItems, DebugPaneDelItems.Builder>());
            s_packetDecoders.Add(145, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleUpdateFromPane, DebugConsoleUpdateFromPane.Builder>());
            s_packetDecoders.Add(146, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleCmdList, DebugConsoleCmdList.Builder>());
            s_packetDecoders.Add(147, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleGetZones, DebugConsoleGetZones.Builder>());
            s_packetDecoders.Add(148, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleZones, DebugConsoleZones.Builder>());
            s_packetDecoders.Add(168, new ConnectAPI.DefaultProtobufPacketDecoder<AuroraHandshake, AuroraHandshake.Builder>());
            s_packetDecoders.Add(201, new ConnectAPI.DefaultProtobufPacketDecoder<GetAccountInfo, GetAccountInfo.Builder>());
            s_packetDecoders.Add(203, new ConnectAPI.DefaultProtobufPacketDecoder<UtilHandshake, UtilHandshake.Builder>());
            s_packetDecoders.Add(204, new ConnectAPI.DefaultProtobufPacketDecoder<UtilAuth, UtilAuth.Builder>());
            s_packetDecoders.Add(205, new ConnectAPI.DefaultProtobufPacketDecoder<UpdateLogin, UpdateLogin.Builder>());
            s_packetDecoders.Add(206, new ConnectAPI.DefaultProtobufPacketDecoder<DebugAuth, DebugAuth.Builder>());
            s_packetDecoders.Add(209, new ConnectAPI.DefaultProtobufPacketDecoder<CreateDeck, CreateDeck.Builder>());
            s_packetDecoders.Add(210, new ConnectAPI.DefaultProtobufPacketDecoder<DeleteDeck, DeleteDeck.Builder>());
            s_packetDecoders.Add(211, new ConnectAPI.DefaultProtobufPacketDecoder<RenameDeck, RenameDeck.Builder>());
            s_packetDecoders.Add(213, new ConnectAPI.DefaultProtobufPacketDecoder<AckNotice, AckNotice.Builder>());
            s_packetDecoders.Add(214, new ConnectAPI.DefaultProtobufPacketDecoder<GetDeck, GetDeck.Builder>());
            s_packetDecoders.Add(220, new ConnectAPI.DefaultProtobufPacketDecoder<DeckGainedCard, DeckGainedCard.Builder>());
            s_packetDecoders.Add(221, new ConnectAPI.DefaultProtobufPacketDecoder<DeckLostCard, DeckLostCard.Builder>());
            s_packetDecoders.Add(222, new ConnectAPI.DefaultProtobufPacketDecoder<DeckSetData, DeckSetData.Builder>());
            s_packetDecoders.Add(223, new ConnectAPI.DefaultProtobufPacketDecoder<AckCardSeen, AckCardSeen.Builder>());
            s_packetDecoders.Add(225, new ConnectAPI.DefaultProtobufPacketDecoder<OpenBooster, OpenBooster.Builder>());
            s_packetDecoders.Add(228, new ConnectAPI.DefaultProtobufPacketDecoder<ClientTracking, ClientTracking.Builder>());
            s_packetDecoders.Add(229, new ConnectAPI.DefaultProtobufPacketDecoder<SubmitBug, SubmitBug.Builder>());
            s_packetDecoders.Add(230, new ConnectAPI.DefaultProtobufPacketDecoder<SetProgress, SetProgress.Builder>());
            s_packetDecoders.Add(235, new ConnectAPI.DefaultProtobufPacketDecoder<DraftBegin, DraftBegin.Builder>());
            s_packetDecoders.Add(237, new ConnectAPI.DefaultProtobufPacketDecoder<GetBattlePayConfig, GetBattlePayConfig.Builder>());
            s_packetDecoders.Add(239, new ConnectAPI.DefaultProtobufPacketDecoder<SetOptions, SetOptions.Builder>());
            s_packetDecoders.Add(240, new ConnectAPI.DefaultProtobufPacketDecoder<GetOptions, GetOptions.Builder>());
            s_packetDecoders.Add(242, new ConnectAPI.DefaultProtobufPacketDecoder<DraftRetire, DraftRetire.Builder>());
            s_packetDecoders.Add(243, new ConnectAPI.DefaultProtobufPacketDecoder<AckAchieveProgress, AckAchieveProgress.Builder>());
            s_packetDecoders.Add(244, new ConnectAPI.DefaultProtobufPacketDecoder<DraftGetPicksAndContents, DraftGetPicksAndContents.Builder>());
            s_packetDecoders.Add(245, new ConnectAPI.DefaultProtobufPacketDecoder<DraftMakePick, DraftMakePick.Builder>());
            s_packetDecoders.Add(250, new ConnectAPI.DefaultProtobufPacketDecoder<GetPurchaseMethod, GetPurchaseMethod.Builder>());
            s_packetDecoders.Add(253, new ConnectAPI.DefaultProtobufPacketDecoder<GetAchieves, GetAchieves.Builder>());
            s_packetDecoders.Add(255, new ConnectAPI.DefaultProtobufPacketDecoder<GetBattlePayStatus, GetBattlePayStatus.Builder>());
            s_packetDecoders.Add(257, new ConnectAPI.DefaultProtobufPacketDecoder<BuySellCard, BuySellCard.Builder>());
            s_packetDecoders.Add(259, new ConnectAPI.DefaultProtobufPacketDecoder<DevBnetIdentify, DevBnetIdentify.Builder>());
            s_packetDecoders.Add(261, new ConnectAPI.DefaultProtobufPacketDecoder<GuardianTrack, GuardianTrack.Builder>());
            s_packetDecoders.Add(263, new ConnectAPI.DefaultProtobufPacketDecoder<CloseCardMarket, CloseCardMarket.Builder>());
            s_packetDecoders.Add(267, new ConnectAPI.DefaultProtobufPacketDecoder<CheckAccountLicenses, CheckAccountLicenses.Builder>());
            s_packetDecoders.Add(268, new ConnectAPI.DefaultProtobufPacketDecoder<MassDisenchantRequest, MassDisenchantRequest.Builder>());
            s_packetDecoders.Add(273, new ConnectAPI.DefaultProtobufPacketDecoder<DoPurchase, DoPurchase.Builder>());
            s_packetDecoders.Add(274, new ConnectAPI.DefaultProtobufPacketDecoder<CancelPurchase, CancelPurchase.Builder>());
            s_packetDecoders.Add(276, new ConnectAPI.DefaultProtobufPacketDecoder<CheckGameLicenses, CheckGameLicenses.Builder>());
            s_packetDecoders.Add(279, new ConnectAPI.DefaultProtobufPacketDecoder<PurchaseWithGold, PurchaseWithGold.Builder>());
            s_packetDecoders.Add(281, new ConnectAPI.DefaultProtobufPacketDecoder<CancelQuest, CancelQuest.Builder>());
            s_packetDecoders.Add(284, new ConnectAPI.DefaultProtobufPacketDecoder<ValidateAchieve, ValidateAchieve.Builder>());
            s_packetDecoders.Add(287, new ConnectAPI.DefaultProtobufPacketDecoder<DraftAckRewards, DraftAckRewards.Builder>());
            s_packetDecoders.Add(291, new ConnectAPI.DefaultProtobufPacketDecoder<SetCardBack, SetCardBack.Builder>());
            s_packetDecoders.Add(293, new ConnectAPI.DefaultProtobufPacketDecoder<DoThirdPartyPurchase, DoThirdPartyPurchase.Builder>());
            s_packetDecoders.Add(294, new ConnectAPI.DefaultProtobufPacketDecoder<GetThirdPartyPurchaseStatus, GetThirdPartyPurchaseStatus.Builder>());
            s_packetDecoders.Add(298, new ConnectAPI.DefaultProtobufPacketDecoder<TriggerLaunchDayEvent, TriggerLaunchDayEvent.Builder>());
            s_packetDecoders.Add(401, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetPlayerInfo, AtlasGetPlayerInfo.Builder>());
            s_packetDecoders.Add(402, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetCollection, AtlasGetCollection.Builder>());
            s_packetDecoders.Add(403, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetCardDetails, AtlasGetCardDetails.Builder>());
            s_packetDecoders.Add(404, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetDecks, AtlasGetDecks.Builder>());
            s_packetDecoders.Add(405, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAddCard, AtlasAddCard.Builder>());
            s_packetDecoders.Add(406, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasRemoveCard, AtlasRemoveCard.Builder>());
            s_packetDecoders.Add(407, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasChangeArcaneDust, AtlasChangeArcaneDust.Builder>());
            s_packetDecoders.Add(408, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasRestoreCard, AtlasRestoreCard.Builder>());
            s_packetDecoders.Add(409, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetOrders, AtlasGetOrders.Builder>());
            s_packetDecoders.Add(410, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetAchieves, AtlasGetAchieves.Builder>());
            s_packetDecoders.Add(411, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetAchieveInfo, AtlasGetAchieveInfo.Builder>());
            s_packetDecoders.Add(412, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetBoosters, AtlasGetBoosters.Builder>());
            s_packetDecoders.Add(413, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAddBooster, AtlasAddBooster.Builder>());
            s_packetDecoders.Add(414, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasRemoveBooster, AtlasRemoveBooster.Builder>());
            s_packetDecoders.Add(415, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetDrafts, AtlasGetDrafts.Builder>());
            s_packetDecoders.Add(416, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAddDraft, AtlasAddDraft.Builder>());
            s_packetDecoders.Add(417, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasRemoveDraft, AtlasRemoveDraft.Builder>());
            s_packetDecoders.Add(418, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasChangeGold, AtlasChangeGold.Builder>());
            s_packetDecoders.Add(419, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetCurrencyDetails, AtlasGetCurrencyDetails.Builder>());
            s_packetDecoders.Add(420, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasChangeBonusGold, AtlasChangeBonusGold.Builder>());
            s_packetDecoders.Add(421, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetCardBacks, AtlasGetCardBacks.Builder>());
            s_packetDecoders.Add(422, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAddCardBack, AtlasAddCardBack.Builder>());
            s_packetDecoders.Add(423, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasRemoveCardBack, AtlasRemoveCardBack.Builder>());

            s_packetDecoders.Add(5, new ConnectAPI.DefaultProtobufPacketDecoder<DebugMessage, DebugMessage.Builder>());
            s_packetDecoders.Add(7, new ConnectAPI.DefaultProtobufPacketDecoder<StartGameState, StartGameState.Builder>());
            s_packetDecoders.Add(8, new ConnectAPI.DefaultProtobufPacketDecoder<FinishGameState, FinishGameState.Builder>());
            s_packetDecoders.Add(9, new ConnectAPI.DefaultProtobufPacketDecoder<PegasusGame.TurnTimer, PegasusGame.TurnTimer.Builder>());
            s_packetDecoders.Add(10, new ConnectAPI.DefaultProtobufPacketDecoder<NAckOption, NAckOption.Builder>());
            s_packetDecoders.Add(12, new ConnectAPI.DefaultProtobufPacketDecoder<GameCanceled, GameCanceled.Builder>());
            s_packetDecoders.Add(14, new ConnectAPI.DefaultProtobufPacketDecoder<AllOptions, AllOptions.Builder>());
            //s_packetDecoders.Add(15, new ConnectAPI.DefaultProtobufPacketDecoder<UserUI, UserUI.Builder>());
            s_packetDecoders.Add(16, new ConnectAPI.DefaultProtobufPacketDecoder<GameSetup, GameSetup.Builder>());
            s_packetDecoders.Add(17, new ConnectAPI.DefaultProtobufPacketDecoder<EntityChoice, EntityChoice.Builder>());
            s_packetDecoders.Add(18, new ConnectAPI.DefaultProtobufPacketDecoder<PreLoad, PreLoad.Builder>());
            s_packetDecoders.Add(19, new ConnectAPI.DefaultProtobufPacketDecoder<PowerHistory, PowerHistory.Builder>());
            s_packetDecoders.Add(21, new ConnectAPI.DefaultProtobufPacketDecoder<PegasusGame.Notification, PegasusGame.Notification.Builder>());
            s_packetDecoders.Add(114, new ConnectAPI.DefaultProtobufPacketDecoder<GameStarting, GameStarting.Builder>());
            s_packetDecoders.Add(167, new ConnectAPI.DefaultProtobufPacketDecoder<DeadendUtil, DeadendUtil.Builder>());
            s_packetDecoders.Add(169, new ConnectAPI.DefaultProtobufPacketDecoder<Deadend, Deadend.Builder>());
            s_packetDecoders.Add(202, new ConnectAPI.DefaultProtobufPacketDecoder<DeckList, DeckList.Builder>());
            s_packetDecoders.Add(207, new ConnectAPI.DefaultProtobufPacketDecoder<Collection, Collection.Builder>());
            s_packetDecoders.Add(208, new ConnectAPI.DefaultProtobufPacketDecoder<GamesInfo, GamesInfo.Builder>());
            s_packetDecoders.Add(212, new ConnectAPI.DefaultProtobufPacketDecoder<ProfileNotices, ProfileNotices.Builder>());
            s_packetDecoders.Add(215, new ConnectAPI.DefaultProtobufPacketDecoder<DeckContents, DeckContents.Builder>());
            s_packetDecoders.Add(216, new ConnectAPI.DefaultProtobufPacketDecoder<DBAction, DBAction.Builder>());
            s_packetDecoders.Add(217, new ConnectAPI.DefaultProtobufPacketDecoder<DeckCreated, DeckCreated.Builder>());
            s_packetDecoders.Add(218, new ConnectAPI.DefaultProtobufPacketDecoder<DeckDeleted, DeckDeleted.Builder>());
            s_packetDecoders.Add(219, new ConnectAPI.DefaultProtobufPacketDecoder<DeckRenamed, DeckRenamed.Builder>());
            s_packetDecoders.Add(224, new ConnectAPI.DefaultProtobufPacketDecoder<BoosterList, BoosterList.Builder>());
            s_packetDecoders.Add(226, new ConnectAPI.DefaultProtobufPacketDecoder<BoosterContent, BoosterContent.Builder>());
            s_packetDecoders.Add(227, new ConnectAPI.DefaultProtobufPacketDecoder<ProfileLastLogin, ProfileLastLogin.Builder>());
            s_packetDecoders.Add(231, new ConnectAPI.DefaultProtobufPacketDecoder<ProfileDeckLimit, ProfileDeckLimit.Builder>());
            s_packetDecoders.Add(232, new ConnectAPI.DefaultProtobufPacketDecoder<MedalInfo, MedalInfo.Builder>());
            s_packetDecoders.Add(233, new ConnectAPI.DefaultProtobufPacketDecoder<ProfileProgress, ProfileProgress.Builder>());
            s_packetDecoders.Add(234, new ConnectAPI.DefaultProtobufPacketDecoder<MedalHistory, MedalHistory.Builder>());
            s_packetDecoders.Add(236, new ConnectAPI.DefaultProtobufPacketDecoder<CardBacks, CardBacks.Builder>());
            s_packetDecoders.Add(238, new ConnectAPI.DefaultProtobufPacketDecoder<BattlePayConfigResponse, BattlePayConfigResponse.Builder>());
            s_packetDecoders.Add(241, new ConnectAPI.DefaultProtobufPacketDecoder<ClientOptions, ClientOptions.Builder>());
            s_packetDecoders.Add(246, new ConnectAPI.DefaultProtobufPacketDecoder<DraftBeginning, DraftBeginning.Builder>());
            s_packetDecoders.Add(247, new ConnectAPI.DefaultProtobufPacketDecoder<DraftRetired, DraftRetired.Builder>());
            s_packetDecoders.Add(248, new ConnectAPI.DefaultProtobufPacketDecoder<DraftChoicesAndContents, DraftChoicesAndContents.Builder>());
            s_packetDecoders.Add(249, new ConnectAPI.DefaultProtobufPacketDecoder<DraftChosen, DraftChosen.Builder>());
            s_packetDecoders.Add(251, new ConnectAPI.DefaultProtobufPacketDecoder<DraftError, DraftError.Builder>());
            s_packetDecoders.Add(252, new ConnectAPI.DefaultProtobufPacketDecoder<Achieves, Achieves.Builder>());
            s_packetDecoders.Add(254, new ConnectAPI.NoOpPacketDecoder());
            s_packetDecoders.Add(256, new ConnectAPI.DefaultProtobufPacketDecoder<PurchaseResponse, PurchaseResponse.Builder>());
            s_packetDecoders.Add(258, new ConnectAPI.DefaultProtobufPacketDecoder<BoughtSoldCard, BoughtSoldCard.Builder>());
            s_packetDecoders.Add(260, new ConnectAPI.DefaultProtobufPacketDecoder<CardValues, CardValues.Builder>());
            s_packetDecoders.Add(262, new ConnectAPI.DefaultProtobufPacketDecoder<ArcaneDustBalance, ArcaneDustBalance.Builder>());
            s_packetDecoders.Add(264, new ConnectAPI.DefaultProtobufPacketDecoder<GuardianVars, GuardianVars.Builder>());
            s_packetDecoders.Add(265, new ConnectAPI.DefaultProtobufPacketDecoder<BattlePayStatusResponse, BattlePayStatusResponse.Builder>());
            s_packetDecoders.Add(266, new ConnectAPI.ThrottlePacketDecoder());
            s_packetDecoders.Add(269, new ConnectAPI.DefaultProtobufPacketDecoder<MassDisenchantResponse, MassDisenchantResponse.Builder>());
            s_packetDecoders.Add(270, new ConnectAPI.DefaultProtobufPacketDecoder<PlayerRecords, PlayerRecords.Builder>());
            s_packetDecoders.Add(271, new ConnectAPI.DefaultProtobufPacketDecoder<RewardProgress, RewardProgress.Builder>());
            s_packetDecoders.Add(272, new ConnectAPI.DefaultProtobufPacketDecoder<PurchaseMethod, PurchaseMethod.Builder>());
            s_packetDecoders.Add(275, new ConnectAPI.DefaultProtobufPacketDecoder<CancelPurchaseResponse, CancelPurchaseResponse.Builder>());
            s_packetDecoders.Add(277, new ConnectAPI.DefaultProtobufPacketDecoder<CheckLicensesResponse, CheckLicensesResponse.Builder>());
            s_packetDecoders.Add(278, new ConnectAPI.DefaultProtobufPacketDecoder<GoldBalance, GoldBalance.Builder>());
            s_packetDecoders.Add(280, new ConnectAPI.DefaultProtobufPacketDecoder<PurchaseWithGoldResponse, PurchaseWithGoldResponse.Builder>());
            s_packetDecoders.Add(282, new ConnectAPI.DefaultProtobufPacketDecoder<CancelQuestResponse, CancelQuestResponse.Builder>());
            s_packetDecoders.Add(283, new ConnectAPI.DefaultProtobufPacketDecoder<HeroXP, HeroXP.Builder>());
            s_packetDecoders.Add(285, new ConnectAPI.DefaultProtobufPacketDecoder<ValidateAchieveResponse, ValidateAchieveResponse.Builder>());
            s_packetDecoders.Add(286, new ConnectAPI.DefaultProtobufPacketDecoder<PlayQueue, PlayQueue.Builder>());
            s_packetDecoders.Add(288, new ConnectAPI.DefaultProtobufPacketDecoder<DraftRewardsAcked, DraftRewardsAcked.Builder>());
            s_packetDecoders.Add(289, new ConnectAPI.DefaultProtobufPacketDecoder<Disconnected, Disconnected.Builder>());
            s_packetDecoders.Add(292, new ConnectAPI.DefaultProtobufPacketDecoder<SetCardBackResponse, SetCardBackResponse.Builder>());
            s_packetDecoders.Add(295, new ConnectAPI.DefaultProtobufPacketDecoder<ThirdPartyPurchaseStatusResponse, ThirdPartyPurchaseStatusResponse.Builder>());
            s_packetDecoders.Add(296, new ConnectAPI.DefaultProtobufPacketDecoder<SetProgressResponse, SetProgressResponse.Builder>());
            s_packetDecoders.Add(299, new ConnectAPI.DefaultProtobufPacketDecoder<TriggerEventResponse, TriggerEventResponse.Builder>());
            s_packetDecoders.Add(300, new ConnectAPI.DefaultProtobufPacketDecoder<MassiveLoginReply, MassiveLoginReply.Builder>());

            using (PacketCommunicator communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                Console.WriteLine("Listening...");
                communicator.SetFilter("port 1119 or port 3724");
                communicator.ReceivePackets(0, DispatcherHandler);
            }

            Console.WriteLine("-- End of file reached.");
            Console.ReadKey();

        }

        private static void DispatcherHandler(Packet packet)
        {
            int e = packet.Ethernet.IpV4.Tcp.Http.Length;
            var d = packet.Buffer.Reverse().Take(e).Reverse().ToArray();

            PegasusPacket x = new PegasusPacket();
            try
            {
                x.Decode(d, 0, d.Length);
            }
            catch
            {
            }
            finally
            {
                ConnectAPI.PacketDecoder decoder;

                if (s_packetDecoders.TryGetValue(x.Type, out decoder))
                {
                    Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff"));
                    PegasusPacket item = decoder.HandlePacket(x);

                    var bod = item.Body;
                    var typ = item.Type;
                    Console.WriteLine(typ);
                    //var i = item.Body.GetType();

                    if (typ == 19)
                    {
                        PowerHistory history = (PowerHistory)bod;
                        IList<PowerHistoryData> listy = history.ListList;
                        foreach (var phd in listy)
                        {
                            Console.WriteLine(phd);
                        }
                    }
                    else if (typ == 14)
                    {
                        AllOptions history = (AllOptions)bod;
                        IList<PegasusGame.Option> listy = history.OptionsList;
                        foreach (var phd in listy)
                        {
                            Console.WriteLine(phd);
                        }
                    }
                    /*
                    else if () {

                    }*/
                    else
                    {
                        Console.WriteLine(x.Body.ToString());
                    }
                }
            }
            //tt++;
        }

    }
    public static class MyColorsExtensions
    {
        public static Color Interpolate(this Color source, Color target, double percent)
        {
            var r = (byte)(source.R + (target.R - source.R) * percent);
            var g = (byte)(source.G + (target.G - source.G) * percent);
            var b = (byte)(source.B + (target.B - source.B) * percent);

            return Color.FromArgb(255, r, g, b);
        }
    }

}
