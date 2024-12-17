using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateGame
{
    interface ICharacter
    {
        List<Object> Traits { get; set; }
        bool IsBald { get; set; }
        int ID { get; set; }

        void WriteCharacter(List<string> traits);
    }
}
