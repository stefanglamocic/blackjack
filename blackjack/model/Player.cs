using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackjack.model
{
    class Player
    {
        private List<Card> drawnCards = new List<Card>();
        private int _total = 0;
        public int Total { get => _total; }
        public string Name { get; }
        public int Balance { get; set; }

        public Player(string name)
        {
            Name = name;
        }

        public void Hit(Deck deck)
        {
            Card drawnCard = deck.DrawCard();
            drawnCards.Add(drawnCard);
            _total += drawnCard.Value;

            if(_total > 21 && ScanForAce())
            {
                _total -= 10;
            }
        }

        private Boolean ScanForAce()
        {
            foreach(Card card in drawnCards)
                if(card.Value == 11)
                {
                    card.Value = 1;
                    return true;
                }
            return false;
        }
    }
}
