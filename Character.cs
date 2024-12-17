using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateGame
{
    class Character : ICharacter
    {
        private List<Object> _characterTraits = new List<Object>();
        private bool _isBald;
        private int _id;
        public List<Object> Traits
        {
            get
            {
                return _characterTraits;
            }
            set
            {
                _characterTraits.Add(value);
            }
        }

        public bool IsBald
        {
            get
            {
                return _isBald;
            }
            set
            {
                _isBald = value;
            }
        }

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public void WriteCharacter(List<string> traits)
        {
            Console.WriteLine("\n[PIRATE'S IDENTITY]");
            WriteInfo(traits, 18);
            Console.WriteLine("[PIRATE'S DESTINY]");
            WriteInfo(traits);
            Console.WriteLine("\n[PIRATE'S VISAGE]");
            WriteInfo(traits, 3, 14);
        }

        private void WriteInfo(List<string> traits)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(traits[i] + ": " + Traits[i]);
            }
        }


        private void WriteInfo(List<string> traits, byte start, byte end)
        {
            for (int i = start; i < end; i++)
            {
                Console.WriteLine(traits[i] + ": " + Traits[i]);
            }
        }

        private void WriteInfo(List<string> traits, byte start)
        {
            for (int i = start; i < traits.Count; i++)
            {
                Console.WriteLine(traits[i] + ": " + Traits[i]);
            }

            for (int i = 14; i < 18; i++)
            {
                Console.WriteLine(traits[i] + ": " + Traits[i]);
            }

            Console.WriteLine();
        }
    }
}
