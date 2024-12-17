using System;

namespace PirateGame
{
    class Program
    {
        public static void Main(string[] args)
        {
            Game pirateGame = new Game();
            int exit;

            do
            {
                Console.Clear(); Console.WriteLine("\x1b[3J"); Console.Clear();

                exit = pirateGame.Start();
                
                if (exit == 15)
                {
                    continue;
                }
                else if (exit != 1)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("\n\n" +
                    "[1] MAIN MENU\n" +
                    "[2] EXIT GAME");

                    if (Console.ReadKey().Key == ConsoleKey.D1)
                    {
                        continue;
                    }
                    break;
                }
            }
            while (true);
            Console.Clear();
        }
    }
}