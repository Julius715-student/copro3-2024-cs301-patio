using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateGame
{
    class Game
    {
        private static readonly Database db = new();
        public int Start()
        {
            db.Initialize();
            db.InitializeMenu();
            TitleScreen menu = new TitleScreen();

            menu.WriteTitle();
            menu.Write(db.GetMenuData());

            switch (menu.Choose(db.GetMenuData()))
            {
                case 1:
                    return NewGame();
                case 2:
                    return LoadGame();
                case 3:
                    Campaign();
                    db.Close();
                    return 1;
                case 4:
                    Credits();
                    db.Close();
                    return 1;
                case 5:
                    Console.Clear();
                    db.Close();
                    Environment.Exit(0);
                    break;
            }
            return 0;
        }

        internal static int NewGame()
        {
            db.InitializeCharacters();
            if (db.GetCharacterData().Count == 9)
            {
                Console.Clear();
                Console.WriteLine("[THE PIRATE SHIP IS FULL]"); // PirateGame only has 9 character slots
                return 1;
            }

            db.InitializeCC();
            db.InitializeTraits();
            CharacterCreation create = new CharacterCreation();
            Character pirate = new Character();
            
            for (int i = 0; i < db.GetCCData().Count; i++)
            {
                if (i > 17)
                {
                    pirate.Traits.Add(create.ValidateName(db, i));
                    continue;
                }

                if (i == 6)
                {
                    pirate.IsBald = pirate.Traits[5].Equals("BALD") ? true : false;
                    if (pirate.IsBald)
                    {
                        pirate.Traits.Insert(6, "NONE");
                        continue;
                    }
                }

                create.Write(db.GetCCData()[i]);

                Object trait = create.Choose(db.GetCCData()[i]);
                if (int.TryParse(trait.ToString(), out _))
                {
                    return (int) trait;
                }

                pirate.Traits.Add(trait);
            }

            Console.Clear();
            pirate.WriteCharacter(db.GetTraitData());
            db.Insert(pirate);

            db.Close();
            return 1;
        }

        internal static int LoadGame()
        {
            db.InitializeLoad();
            db.InitializeCharacters();
            db.InitializeTraits();
            CharacterCreation load = new CharacterCreation();
            load.Write(db.GetLoadData());

            switch (load.Choose(db.GetLoadData()))
            {
                case 1: // For viewing all characters
                    Console.Clear(); Console.WriteLine("\x1b[3J"); Console.Clear();

                    if (db.GetCharacterData().Count == 0)
                    {
                        Console.WriteLine("[NO PIRATES ON THE SHIP]");
                        db.Close();
                        return 1;
                    }

                    foreach (Character pirate in db.GetCharacterData())
                    {
                        pirate.WriteCharacter(db.GetTraitData());
                        load.LineBreak();
                    }
                    db.Close();
                    return 1;

                case 2: // For deleting character
                    

                    if (db.GetCharacterData().Count == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("[NO PIRATES ON THE SHIP]");
                        db.Close();
                        return 1;
                    }

                    do
                    {
                        Console.Clear(); Console.WriteLine("\x1b[3J"); Console.Clear();
                        Console.WriteLine("[ESC] MAIN MENU\n" +
                            "\nCHOOSE A PIRATE TO SEND TO DAVY JONES' LOCKER");
                        for (int i = 0; i < db.GetCharacterData().Count; i++)
                        {
                            Console.WriteLine($"[{i+1}] NAME: {db.GetCharacterData()[i].Traits[18]}");
                        }

                        int choice = 0;
                        ConsoleKeyInfo key = Console.ReadKey();

                        if (char.IsDigit(key.KeyChar))
                        {
                            choice = int.Parse(key.KeyChar.ToString());
                        }
                        else if (key.Key == ConsoleKey.Escape)
                        {
                            db.Close();
                            return 15;
                        }
                        else
                        {
                            continue;
                        }

                        if (choice < 1 || choice > db.GetCharacterData().Count)
                        {
                            Console.Write(" (INVALID CHOICE)");
                            Thread.Sleep(2500);
                        }
                        else
                        {
                            Console.Clear();
                            db.GetCharacterData()[choice - 1].WriteCharacter(db.GetTraitData());
                            Console.WriteLine("\n\nARE YOU SURE YOU WANT TO DELETE THIS PIRATE?" +
                                "\n[1] YES" +
                                "\n[2] NO");

                            if (Console.ReadKey().Key == ConsoleKey.D1)
                            {
                                Console.Clear(); Console.WriteLine("\x1b[3J"); Console.Clear();
                                db.Delete(db.GetCharacterData()[choice - 1].ID);
                                Console.WriteLine("[A PIRATE HAS WALKED THE PLANK]");
                                Thread.Sleep(2500);
                                return LoadGame();
                            }
                        }
                    }
                    while (true);

                case 15:
                    return 15;
            }
            return 0;
        }

        internal static void Campaign()
        {
            Console.Clear();
            Console.WriteLine("CAMPAIGN MODE\n" +
                "\nOn one sunny day, it suddenly began to rain. It was unexpected, but people just accepted it and went about their daily lives. However, they soon realized that this rain was not normal. Everywhere and everyone, from the west to the east, from the north to the south, was experiencing this sudden downpour, all at the same time. Then, a loud voice rang out that could be heard all across the globe, “LET’S PLAY A GAME, SHALL WE?!”." +
                "\r\n\r\nSuddenly, the entire Earth started to shake violently. The rain became heavier and was accompanied by thunder and lightning. Water levels across all bodies of water around the world began to rise. Every living being was in disarray, their survival at the forefront of their minds. These calamities lasted an entire day…" +
                "\r\n\r\n“HAHAHA! ARE ANY OF YOU STILL ALIVE, I WONDER?”, said the voice from the sky after the disasters passed. “IF ANY OF YOU ARE STILL BREATHING, I INVITE YOU TO PLAY MY GAME! A TREASURE HUNT, I DARE SAY!”, the voice, which is that of a man’s, enthusiastically announced. “IN THIS WORLD, NOW FILLED WITH WATER AND BARELY ANY LAND, I ENTICE YOU TO FIND THE TREASURE OF THE SEAS!”. The owner of the voice, after all that has happened, finally introduces himself. “I, POSEIDON, WILL GRANT YOU THE POWER TO CONQUER THIS WORLD IF YOU MANAGE TO FIND THIS TREASURE!”. There were no murmurs nor any sound even after all these revelations; the world was quiet. “I WISH YOU ALL THE BEST OF LUCK, HAHAHAHA!”, with these final words, the King of the Sea bid his farewell." +
                "\r\n\r\nA century has passed since this event, known as POSEIDON’S PUNISHMENT. With around 90% of the Earth now covered in water, sea creatures are thriving more than ever before. Creatures on land, on the other hand, are just trying their best to survive. However, there is one race that was able to thrive on both land and sea: the Human race. With their ingenuity, they were able to survive and bring about the age known as the RESURGENCE OF PIRATES." +
                "\r\n\r\nIn this world, now known as POSEIDON’S PALM, pirates are the leaders, the saviors, the villains, and the criminals. With limited land, they scour the sea for opportunities. There are those who seek to find the treasure of Poseidon, those who desire to amass their wealth for selfish greed, those who live for the hunt of mythical creatures, and those who thirst for battle. Others seek to expand their knowledge of this new world, while others have the passion to explore all the sea has to offer. You are someone special in this world, blessed with a Pirate Gift, and you have the power to steer your fate towards your dreams and aspirations. Now, venture forth into the MYTHOLOGICAL SEAS.");
        }

        internal static void Credits()
        {
            Console.Clear();
            Console.WriteLine("CREDITS\n" +
                "\nProgrammerist - JULIUS LINEL PATIO" +
                "\nDocumentationist - JULIUS LINEL PATIO" +
                "\nPancit Canton - JULIUS LINEL PATIO");
        }
    }
}
