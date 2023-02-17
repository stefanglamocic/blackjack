using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackjack.model
{
    class Dealer : Player
    {
        public Dealer() : base("Dealer") { }

        public void FinishDrawing(Deck deck)
        {
            while (Total < 17)
                Hit(deck);
        }
    }
}
