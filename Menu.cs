using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateGame
{
    abstract class Menu
    {
        public abstract void Write(Prompt prompt);
        public abstract Object Choose(Prompt prompt);
    }
}
