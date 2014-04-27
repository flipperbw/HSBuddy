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

namespace hearthdecktracker
{
    public partial class Form1 : Form
    {
        private bool dragging;
        private Point offset;
        //private TableLayoutPanel tab;
        /*public static Color Interpolate(this Color source, Color target, double percent)
        {
            var r = (byte)(source.R + (target.R - source.R) * percent);
            var g = (byte)(source.G + (target.G - source.G) * percent);
            var b = (byte)(source.B + (target.B - source.B) * percent);

            return Color.FromArgb(255, r, g, b);
        }*/

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
            /*new Card {
                Name = "Fiery War Axe",
                Text = "abc",
                Mana = 0,
                Atk = 0,
                Def = 0,
                Dmg = "1",
                Heal = "1",
                Catk = "1",
                Targ = "1"
            }*/
        };

        public static void hello()
        {
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                IgnoreTrailingSeparatorChar = true,
                IgnoreUnknownColumns = true
            };
            CsvContext cc = new CsvContext();
            IEnumerable<Card> allthecards = cc.Read<Card>(@"C:\Users\flipp_000\Documents\Visual Studio 2013\Projects\hearthdecktracker\hearthdecktracker\Resources\hd.csv", inputFileDescription);
            
            /*
            var productsByName =
            from p in products
            orderby p.Name
            select new { p.Name, p.LaunchDate, p.Price, p.Description };*/

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
        public void setlists()
        {
             Alldecklists.Add(new Decklist {
                    Name = "Choose Deck...",
                    Cardlist = null
                });
             Alldecklists.Add(
                new Decklist
                {
                    Name = "Warrior (Control)",
                    Cardlist =
                    {
                        new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Fiery War Axe")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Shield Slam")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Execute")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Whirlwind")
                        },new Deckcard {
                            Amount = 1, // put an alternate in here
                            carddetails = Allcards.Find(r => r.Name == "Slam")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Armorsmith")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Cruel Taskmaster")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Shield Block")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Acolyte of Pain")
                        },new Deckcard {
                            Amount = 1,
                            carddetails = Allcards.Find(r => r.Name == "Big Game Hunter")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Frothing Berserker")
                        },new Deckcard {
                            Amount = 1,
                            carddetails = Allcards.Find(r => r.Name == "Brawl")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Faceless Manipulator")
                        },new Deckcard {
                            Amount = 1,
                            carddetails = Allcards.Find(r => r.Name == "Cairne Bloodhoof")
                        },new Deckcard {
                            Amount = 1,
                            carddetails = Allcards.Find(r => r.Name == "Sylvanas Windrunner")
                        },new Deckcard {
                            Amount = 1,
                            carddetails = Allcards.Find(r => r.Name == "Baron Geddon")
                        },new Deckcard {
                            Amount = 1,
                            carddetails = Allcards.Find(r => r.Name == "Grommash Hellscream")
                        },new Deckcard {
                            Amount = 1,
                            carddetails = Allcards.Find(r => r.Name == "Ragnaros the Firelord")
                        },new Deckcard {
                            Amount = 1,
                            carddetails = Allcards.Find(r => r.Name == "Alexstrasza")
                        },new Deckcard {
                            Amount = 1,
                            carddetails = Allcards.Find(r => r.Name == "Ysera")
                        }
                    }
                }
            );
            Alldecklists.Add(
                new Decklist
                {
                    Name = "Warlock (Zoo)",
                    Cardlist =
                    {
                        new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Soulfire")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Abusive Sergeant")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Argent Squire")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Flame Imp")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Shieldbearer")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Voidwalker")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Young Priestess")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Amani Berserker")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Dire Wolf Alpha")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Knife Juggler")
                        },new Deckcard {
                            Amount = 1,
                            carddetails = Allcards.Find(r => r.Name == "King Mukla")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Shattered Sun Cleric")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Dark Iron Dwarf")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Defender of Argus")
                        },new Deckcard {
                            Amount = 2,
                            carddetails = Allcards.Find(r => r.Name == "Doomguard")
                        },new Deckcard {
                            Amount = 1,
                            carddetails = Allcards.Find(r => r.Name == "The Black Knight")
                        }
                    }
                }
            );
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
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Feral Spriti")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Hex")},
                    new Deckcard {Amount = 2, carddetails = Allcards.Find(r => r.Name == "Lighting Storm")},
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
            hello();
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

        /*private void update_decklist(object sender, EventArgs e)
        {
            var i = 1;

            tab = new TableLayoutPanel();
            tab.Visible = false;
            tab.SuspendLayout();
            this.Controls.Add(tab);
            tab.ColumnCount = 5;
            //tab.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25F));
            tab.Location = new Point(1, 95);
            tab.Name = "tableLayoutPanel1";
            tab.RowCount = ((IEnumerable<Deckcard>)comboBox1.SelectedValue).Count() + 1;
            //tab.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tab.Size = new Size(330, 700);
            //tab.TabIndex = 7;

            // make a new row class, name it row1, and just get row1.atk

            tab.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.0F));
            tab.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70.0F));
            tab.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.0F));
            tab.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.0F));
            tab.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.0F));

            var h1 = new Label();
            h1.Text = "mana";
            h1.Font = new Font("BlizzardGlobal", 6F);
            var h2 = new Label();
            h2.Text = "name";
            h2.Font = new Font("BlizzardGlobal", 9F);
            var h3 = new Label();
            h3.Text = "atk/def";
            h3.Font = new Font("BlizzardGlobal", 9F);
            var h4 = new Label();
            h4.AutoSize = true;
            h4.Text = "dmg,heal,Δatk\ntarget";
            h4.Font = new Font("BlizzardGlobal", 5F);
            var h5 = new Label();
            h5.Text = "amt";
            h5.Font = new Font("BlizzardGlobal", 7F);

            tab.Controls.Add(h1, 0, 0);
            tab.Controls.Add(h2, 1, 0);
            tab.Controls.Add(h3, 2, 0);
            tab.Controls.Add(h4, 3, 0);
            tab.Controls.Add(h5, 4, 0);

            foreach (Deckcard card in (IEnumerable<Deckcard>)comboBox1.SelectedValue)
            {

                //MessageBox.Show(card.carddetails.Text.ToString());
                var lab1 = new Label();
                lab1.BorderStyle = BorderStyle.Fixed3D;
                //lab.Name = "label1";
                lab1.AutoSize = true;
                lab1.Text = card.carddetails.Mana.ToString();
                lab1.BackColor = Color.White;
                //lab.Font = new Font("BlizzardGlobal", 14F, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lab1.Font = new Font("BlizzardGlobal", 12F);

                var lab2 = new Label();
                lab2.BorderStyle = BorderStyle.Fixed3D;
                //lab2.AutoSize = true;
                lab2.AutoSize = true;
                lab2.Text = card.carddetails.Name;
                lab2.BackColor = Color.White;
                lab2.Font = new Font("BlizzardGlobal", 12F);

                var lab3 = new Label();
                lab3.BorderStyle = BorderStyle.Fixed3D;
                lab3.Text = card.carddetails.Atk + "/" + card.carddetails.Def; //adjust for spells
                lab3.BackColor = Color.White;
                lab3.Font = new Font("BlizzardGlobal", 12F);

                var lab4 = new Label();
                lab4.BorderStyle = BorderStyle.Fixed3D;
                lab4.Text = card.carddetails.Dmg + "|" + card.carddetails.Heal + "|" + card.carddetails.Catk +
                    "\n" + card.carddetails.Targ;
                lab4.BackColor = Color.White;
                lab4.Font = new Font("BlizzardGlobal", 7F);

                var lab5 = new Label();
                lab5.BorderStyle = BorderStyle.Fixed3D;
                lab5.Text = card.Amount.ToString();
                lab5.BackColor = Color.White;
                lab5.Font = new Font("BlizzardGlobal", 12F);

                tab.Controls.Add(lab1, 0, i);
                tab.Controls.Add(lab2, 1, i);
                tab.Controls.Add(lab3, 2, i);
                tab.Controls.Add(lab4, 3, i);
                tab.Controls.Add(lab5, 4, i);

                i++;
            }

            //tab.Click += new System.EventHandler(this.label2_Click);
            tab.ResumeLayout();
            tab.Visible = true;

        }

        //private void tableLayoutPanel1_Click(object sender, EventArgs e)
        //{
         //   var cellPos = GetRowColIndex(
         //       tableLayoutPanel1,
         //       tableLayoutPanel1.PointToClient(Cursor.Position));
        //}
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
            {
                if (tab != null)
                {
                    tab.Dispose();
                }

                update_decklist(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string currentdeck = comboBox1.Text;
            if ((tab != null) && (comboBox1.SelectedIndex != 0))
            {
                tab.Dispose();
                update_decklist(sender, e);
            }
        }

        */


        /****************
         * new way
         * **************/

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int oldamt = int.Parse(dataGridView1.CurrentRow.Cells["Amt"].Value.ToString());
            int newamt = Math.Max(0, oldamt - 1);
            dataGridView1.CurrentRow.Cells["Amt"].Value = newamt;
            if (newamt == 0)
            {
                dataGridView1.CurrentRow.DefaultCellStyle.ForeColor = Color.DarkGray;
                dataGridView1.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.DarkGray;
            }
            //dataGridView1.CurrentRow.DefaultCellStyle.SelectionBackColor.Interpolate = dataGridViewCellStyle1.BackColor;
            //dataGridView1.CurrentRow.DefaultCellStyle.SelectionBackColor = dataGridViewCellStyle1.BackColor;
            //dataGridViewCellStyle1.SelectionForeColor = dataGridViewCellStyle1.ForeColor;
        }

        private void update_decklist()
        {
            dataGridView1.Rows.Clear();

            foreach (Deckcard card in (IEnumerable<Deckcard>)comboBox1.SelectedValue)
            {
                dataGridView1.Rows.Add(
                    Convert.ToInt32(card.carddetails.Mana),
                    card.carddetails.Name,
                    card.carddetails.Atk + "/" + card.carddetails.Def, //adjust for spells
                    card.carddetails.Dmg + "|" + card.carddetails.Heal + "|" + card.carddetails.Catk,
                    card.carddetails.Targ,
                    Convert.ToInt32(card.Amount)
                );
            }

            dataGridView1.Visible = true;
            /*
            tab.RowCount = ((IEnumerable<Deckcard>)comboBox1.SelectedValue).Count() + 1;
            tab.Size = new Size(330, 700);

            // make a new row class, name it row1, and just get row1.atk


            //tab.Click += new System.EventHandler(this.label2_Click);
            */


        }

    }

}
