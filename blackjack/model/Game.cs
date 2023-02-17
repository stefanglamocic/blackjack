using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackjack.model
{
    class Game
    {
        private Deck Deck;

        public Game()
        {
            Deck = new Deck();
        }

        public void Start()
        {
            Dealer dealer = new Dealer();
        }
    }
}
