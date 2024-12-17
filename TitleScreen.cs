using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateGame
{
    class TitleScreen : Menu
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
                        new TitleScreen().Write(prompt);
                    }
                }

                if (currentKey.Key == ConsoleKey.UpArrow)
                {
                    if (prompt.SelectedChoice - 1 >= 0)
                    {
                        prompt.SelectedChoice--;
                        new TitleScreen().Write(prompt);
                    }
                }

                if (currentKey.Key == ConsoleKey.Enter)
                {
                    return prompt.ChoiceList[prompt.SelectedChoice].Value;
                }
            }
            while (true);
        }
        public void WriteTitle()
        {
            Console.Clear();
            Console.Write("--{MYTHOLOGICAL SEAS}--");
            Thread.Sleep(2000);
            Console.Write("\n\n\n\n     PRESS ANY KEY");
            Console.ReadKey();
            Console.Clear();
        }

    }
}
