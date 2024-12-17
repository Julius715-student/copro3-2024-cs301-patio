using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PirateGame
{
    class Database
    {
        private Prompt _menuData;
        private List<Prompt> _ccData;
        private List<string> _traitData;
        private Prompt _loadData;
        private List<Character> _characterData;
        private SqlConnection _connection;
        
        public Prompt GetMenuData()
        {
            return _menuData;
        }

        public List<Prompt> GetCCData()
        {
            return _ccData;
        }

        public List<string> GetTraitData()
        {
            return _traitData;
        }

        public Prompt GetLoadData()
        {
            return _loadData;
        }

        public List<Character> GetCharacterData()
        {
            return _characterData;
        }

        public void Initialize()
        {
            try
            {
                _connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\JULIU\SOURCE\REPOS\PIRATEGAME\PIRATEGAME\CHARACTERDB.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
                _connection.Open();
            }
            catch (SqlException sqlex)
            {
                Console.WriteLine(sqlex.Message);
            }
        }

        public void InitializeMenu()
        {
            _menuData.Head = "--{MYTHOLOGICAL SEAS}--";
            _menuData.ChoiceList = new List<Choice>
            {
                new Choice("NEW GAME", 1),
                new Choice("LOAD GAME", 2),
                new Choice("CAMPAIGN MODE", 3),
                new Choice("CREDITS", 4),
                new Choice("EXIT", 5)
            };
            _menuData.SelectedChoice = 0;
        }

        public void InitializeCC()
        {
            _ccData = new List<Prompt>();
            for (int i = 0; i < questions.Length; i++)
            {
                Prompt prompt = new();

                prompt.Head = questions[i];
                if (!(i > 17))
                {
                    prompt.ChoiceList = new List<Choice>();
                    for (int j = 0; j < choices[i].Length; j++)
                    {
                        if (choices[i][j].Equals("YES"))
                        {
                            prompt.ChoiceList.Add(new Choice(choices[i][j], true));
                            continue;
                        }
                        else if (choices[i][j].Equals("NO"))
                        {
                            prompt.ChoiceList.Add(new Choice(choices[i][j], false));
                            continue;
                        }

                        if (choices[i][j].Contains("["))
                        {
                            prompt.ChoiceList.Add(new Choice(choices[i][j], choices[i][j].Substring(0, choices[i][j].IndexOf("[")).Trim()));
                            continue;
                        }
                        prompt.ChoiceList.Add(new Choice(choices[i][j], choices[i][j]));
                    }
                    prompt.SelectedChoice = 0;
                }

                _ccData.Add(prompt);
            }
        }

        public void InitializeTraits()
        {
            _traitData = new List<string>();
            for (int i = 0; i < traits.Length; i++)
            {
                string trait = traits[i];
                _traitData.Add(trait);
            }
        }

        public void InitializeLoad()
        {
            _loadData.Head = "";
            _loadData.ChoiceList = new List<Choice>
            {
                new Choice("VIEW ALL CHARACTERS", 1),
                new Choice("DELETE A CHARACTER", 2),
            };
            _loadData.SelectedChoice = 0;
        }

        public void InitializeCharacters()
        {
            _characterData = new List<Character>();
            string query = "SELECT * FROM dbo.player_characters";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Character character = new();
                            for (int j = 0; j < columns.Length + 1; j++)
                            {
                                if (j == 0)
                                {
                                    character.ID = (int)reader.GetValue(j);
                                    continue;
                                }
                                character.Traits.Add(reader.GetValue(j));
                            }
                            _characterData.Add(character);
                        }
                        reader.Close();
                    }
                }
            }
            catch (SqlException sqlex)
            {
                Console.WriteLine(sqlex.Message);
            }
        }
        public void Insert(Character pirate)
        {
            string query = CreateInsertQuery();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    for (int i = 0; i < columns.Length; i++)
                    {
                        if (i > 8 && i < 12)
                        {
                            cmd.Parameters.Add("@" + columns[i], SqlDbType.Bit);
                            cmd.Parameters["@" + columns[i]].Value = pirate.Traits[i];
                            continue;
                        }

                        cmd.Parameters.Add("@" + columns[i], SqlDbType.VarChar);
                        cmd.Parameters["@" + columns[i]].Value = pirate.Traits[i];
                    }
                    cmd.ExecuteNonQuery();
                } 
            }
            catch (SqlException sqlex)
            {
                Console.WriteLine(sqlex.Message);
            }
        }

        private string CreateInsertQuery()
        {
            string query = "INSERT INTO dbo.player_characters (";

            for (int i = 0; i < columns.Length; i++) {
                query += columns[i];

                if (i == columns.Length - 1)
                {
                    query += ") ";
                    break;
                }

                query += ", ";
            }

            query += "VALUES (";

            for (int i = 0; i < columns.Length; i++)
            {
                query += "@" + columns[i];

                if (i == columns.Length - 1)
                {
                    query += ") ";
                    break;
                }

                query += ", ";
            }

            return query;
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM dbo.player_characters WHERE id = @id";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlex)
            {
                Console.WriteLine(sqlex.Message);
            }
            
        }

        public void Close()
        {
            _connection.Close();
        }

        private static string[] questions =
        {
            "What is your reputation as a pirate?",
            "Choose your starting location:",
            "Choose your Pirate Gift:",
            "Choose your body type:",
            "Choose your skin color:",
            "Choose your hairstyle:",
            "Choose your hair color:",
            "Choose your face shape:",
            "Choose your eye color:",
            "Wear an eyepatch?",
            "Add facial hair?",
            "Add beard?",
            "Choose your face accessory:",
            "Choose your attire:",
            "Choose your starting weapon:",
            "Choose your starting ship:",
            "Choose your Pirate's Aspiration:",
            "Choose your Quartermaster:",
            "What's your name?",
            "What would you like to call your crew?"

        };

        private static string[][] choices =
        {
            [
                "A RESPECTED LAD (VERY EASY)",
                "A TOLERABLE BUCKO (EASY)",
                "A NOBODY (NORMAL)",
                "A SCALAWAG (HARD)",
                "AN OUTLAW (VERY HARD)"
            ],
            [
                "BLACK SEA [Lots of squid, hard to see what's underwater]",
                "BERMUDA SQUARE [Hold on to your treasures! Because they might suddenly be gone...]",
                "SIREN CAVE [A very rocky area. Apparently, a siren was spotted here]",
                "SHARK’S NEST [Either you get a lot of fish meat, or you become the fish meat]",
                "MERMAID’S ABODE [An encounter with a mermaid can either be a blessing or a curse…]"
            ],
            [
                "POSEIDON'S WILL [Poseidon acknowledges you. Grants the ability to start/stop a storm]",
                "BLESSING OF THE DOLPHINS [You seem to have an affinity for dolphins. Grants the ability to call for dolphins that boost navigation speed]",
                "MERMAIDS' SONG [From a book, you learned the mermaids' song. Grants the ability to attract a mermaid]",
                "WRATH OF THE WHALE [You're as intimidating as a whale is big. Grants the ability to increase your size and strength temporarily]",
                "SHARK'S FEROCITY [A thirst for blood and battle compels you. Grants the ability to increase all offensive stats at the cost of health]"
            ],
            [
                "MASCULINE 1",
                "MASCULINE 2",
                "FEMININE 1",
                "FEMININE 2",
                "SLIM",
                "CHUBBY"
            ],
            [
                "SKIN COLOR 1 (FAIR)",
                "SKIN COLOR 2 (MEDIUM)",
                "SKIN COLOR 3 (BRONZE)",
                "SKIN COLOR 4 (LIGHT BROWN)",
                "SKIN COLOR 5 (OLIVE)"
            ],
            [
                "HAIRSTYLE 1 (HIGH PONYTAIL)",
                "HAIRSTYLE 2 (BRAIDED LONG HAIR)",
                "HAIRSTYLE 3 (MESSY LONG HAIR)",
                "HAIRSTYLE 4 (DREADLOCKS)",
                "BALD"
            ],
            [
                "BLACK",
                "GRAY",
                "WHITE",
                "BROWN",
                "BLUE"
            ],
            [
                "FACE SHAPE 1 (OVAL)",
                "FACE SHAPE 2 (ROUND)",
                "FACE SHAPE 3 (SQUARE)",
                "FACE SHAPE 4 (DIAMOND)",
                "FACE SHAPE 5 (HEART)"
            ],
            [
                "AQUAMARINE",
                "RUBY",
                "LIGHT BLUE",
                "WHITE",
                "BLACK"
            ],
            [
                "YES", "NO"
            ],
            [
                "YES", "NO"
            ],
            [
                "YES", "NO"
            ],
            [
                "SCAR 1 (RIGHT-EYE SCAR)",
                "SCAR 2 (LEFT-EYE SCAR)",
                "WAR PAINT",
                "MONOCLE",
                "GLASSES",
                "NONE"
            ],
            [
                "RIVER PIRATE ATTIRE [\"Flow like the river\", a very liberating attire]",
                "BUCCANEER ATTIRE [An attire fit for the Caribbean seas]",
                "CORSAIR ATTIRE [An attire with a Mediterranean aesthetic]",
                "PRIVATEER ATTIRE [A more sophisticated pirate attire]",
                "GOOD OLE PIRATE ATTIRE [The classic pirate look]"
            ],
            [
                "MUSKET [A long-range weapon, high damage, slow reload]",
                "BLUNDERBUSS [A short-range weapon, medium damage, fast reload]",
                "MACHETE [A melee weapon, medium damage, can be used for cutting things]",
                "DAGGER [A melee weapon, low damage, high attack speed]",
                "RAPIER [A melee weapon, medium damage, better range than most melee weapons]"
            ],
            [
                "TRINIDAD [A balanced ship, suitable for most crews]",
                "SANTIAGO [A sturdy ship, good luck trying to sink this beauty]",
                "VICTORIA [A ship that packs a punch, those cannons are no joke]",
                "CONCEPCION [A ship built for speed, the best choice for sightseeing]",
                "SAN ANTONIO [A rickety old thing, better hope it doesn't sink]"
            ],
            [
                "EXPLORER [You want to explore the world! Increase navigation speed]",
                "TREASURE HUNTER [Gold is all you need. Increase looted treasure]",
                "PLUNDERER [A fight on the seas is your desire. Boost attack when fighting on ships]",
                "LAND LOVER [You seek land to feel at home. Boost attack when fighting on land]",
                "PIRATE KING [Create the strongest pirate crew! Increase the chance of recruiting crew members]"
            ],
            [
                "JACK [A silver-tongued lad. Boosts success chance in negotiations]",
                "JACQUELINE [A feisty and cheerful lass. Boosts crew morale]",
                "FIREBEARD [A battle-hardened veteran. Boosts crew's attack power]",
                "FARNOCK [A master navigator. Boosts navigation speed]",
                "GLINDOLIN [An experienced caretaker. Boosts healing from resting]"
            ],
        };

        private static string[] traits =
        {
            "Pirate's Reputation",
            "Starting Location",
            "Pirate Gift",
            "Body Type",
            "Skin Color",
            "Hairstyle",
            "Hair Color",
            "Face Shape",
            "Eye Color",
            "Has an eyepatch",
            "Has facial hair",
            "Has a beard",
            "Face Accessory",
            "Attire",
            "Starting Weapon",
            "Starting Ship",
            "Pirate's Aspiration",
            "Quartermaster",
            "Pirate Name",
            "Crew Name",
        };

        private static string[] columns =
        {
            "reputation", 
            "location", 
            "gift", 
            "body", 
            "skin", 
            "hair", 
            "hair_color", 
            "face_shape", 
            "eye_color", 
            "eyepatch", 
            "facial_hair", 
            "beard", 
            "face_acc", 
            "attire", 
            "weapon", 
            "ship", 
            "aspiration", 
            "quartermaster", 
            "pirate_name", 
            "crew_name"
        };
    }

    class Choice
    {
        private string name;
        private Object value;

        public Choice(string name, Object value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name { get { return name; } }
        public object Value { get { return value; } }
    }
    struct Prompt
    {
        public string Head;
        public List<Choice> ChoiceList;
        public byte SelectedChoice;
    }
}
