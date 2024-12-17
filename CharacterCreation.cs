using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateGame
{
    class CharacterCreation : Menu
    {
        public override void Write(Prompt prompt)
        {
            Console.Clear();

            
            Console.WriteLine(prompt.Head + "\n");
            foreach (Choice choice in prompt.ChoiceList)
            {
                if (choice == prompt.ChoiceList[prompt.SelectedChoice])
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write(" ");
                }
                Console.WriteLine(choice.Name);
            }

            Console.WriteLine("\n\n[ESC] MAIN MENU");
        }

        public override Object Choose(Prompt prompt)
        {
            ConsoleKeyInfo currentKey;

            do
            {
                currentKey = Console.ReadKey();

                if (currentKey.Key == ConsoleKey.DownArrow)
                {
                    if (prompt.SelectedChoice + 1 < prompt.ChoiceList.Count)
                    {
                        prompt.SelectedChoice++;
                        new CharacterCreation().Write(prompt);
                    }
                }

                if (currentKey.Key == ConsoleKey.UpArrow)
                {
                    if (prompt.SelectedChoice - 1 >= 0)
                    {
                        prompt.SelectedChoice--;
                        new CharacterCreation().Write(prompt);
                    }
                }

                if (currentKey.Key == ConsoleKey.Enter)
                {
                    return prompt.ChoiceList[prompt.SelectedChoice].Value;
                }
                else if (currentKey.Key == ConsoleKey.Escape)
                {
                    return 15;
                }
            }
            while (true);
        }

        public string ValidateName(Database db, int index)
        {
            string name = null;

            switch (index)
            {
                case 18: // For Pirate Name
                    string first = null;

                    do
                    {
                        try
                        {
                            Console.Clear();
                            Console.WriteLine(db.GetCCData()[index].Head + "\n");
                            Console.Write("First Name: ");
                            first = Console.ReadLine().Trim();

                            if (first.Length < 2 || first.Length > 16)
                            {
                                throw new InvalidNameException("(2-16 CHARACTERS, LETTERS ONLY)");
                            }

                            if (first.All(c => char.IsLetter(c) || c == ' '))
                            {
                                break;
                            }
                            throw new InvalidNameException("(2-16 CHARACTERS, LETTERS ONLY)");
                        }
                        catch (InvalidNameException ine)
                        {
                            Console.Clear();
                            Console.WriteLine(db.GetCCData()[index].Head + "\n");
                            Console.Write("First Name: " + first + " " + ine.Message);
                            Thread.Sleep(2500);
                        }
                    }
                    while (true);

                    string last = null;

                    do
                    {
                        try
                        {
                            Console.Clear();
                            Console.WriteLine(db.GetCCData()[index].Head + "\n");
                            Console.Write("First Name: " + first +
                                "\nLast Name: ");
                            last = Console.ReadLine().Trim();

                            if (last.Length < 2 || last.Length > 16)
                            {
                                throw new InvalidNameException("(2-16 CHARACTERS, LETTERS ONLY)");
                            }

                            if (last.All(c => char.IsLetter(c) || c == ' '))
                            {
                                break;
                            }
                            throw new InvalidNameException("(2-16 CHARACTERS, LETTERS ONLY)");
                        }
                        catch (InvalidNameException ine)
                        {
                            Console.Clear();
                            Console.WriteLine(db.GetCCData()[index].Head + "\n");
                            Console.Write("First Name: " + first +
                                "\nLast Name: " + last + " " + ine.Message);
                            Thread.Sleep(2500);
                        }
                    }
                    while (true);

                    name = first + " " + last;
                    bool nameIsInvalid = false;

                    foreach (Character character in db.GetCharacterData())
                    {
                        if (name.ToLower().Equals(character.Traits[18].ToString().ToLower()))
                        {
                            Console.Clear();
                            Console.WriteLine(db.GetCCData()[index].Head + "\n");
                            Console.WriteLine("First Name: " + first +
                                "\nLast Name: " + last);
                            Console.Write("[A PIRATE WITH THAT NAME ALREADY EXISTS]");
                            Thread.Sleep(2500);

                            nameIsInvalid = true;
                            break;
                        }
                    }

                    if (nameIsInvalid)
                    {
                        name = ValidateName(db, index);
                    }
                    break;

                case 19: // For Crew Name
                    string crew = null;

                    do
                    {
                        try
                        {
                            Console.Clear();
                            Console.WriteLine(db.GetCCData()[index].Head + "\n");
                            Console.Write("Crew Name: ");
                            crew = Console.ReadLine().Trim();

                            if (crew.Length < 2 || crew.Length > 32)
                            {
                                throw new InvalidNameException("(2-32 CHARACTERS, LETTERS AND NUMBERS ONLY)");
                            }

                            if (crew.All(c => char.IsLetterOrDigit(c) || c == ' '))
                            {
                                break;
                            }
                            throw new InvalidNameException("(2-32 CHARACTERS, LETTERS AND NUMBERS ONLY)");
                        }
                        catch (InvalidNameException ine)
                        {
                            Console.Clear();
                            Console.WriteLine(db.GetCCData()[index].Head + "\n");
                            Console.Write("Crew Name: " + crew + " " + ine.Message);
                            Thread.Sleep(2500);
                        }
                    }
                    while (true);

                    name = crew;
                    break;
            }
            return name;
        }

        internal void LineBreak()
        {
            int width = Console.WindowWidth;
            string line = new string('-', width);
            Console.WriteLine("\n" +
                line);
        }
    }

    class InvalidNameException : Exception
    {
        public InvalidNameException(string message) : base(message)
        {

        }
    }
}
