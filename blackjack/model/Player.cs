using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace blackjack.model
{
    class Player : INotifyPropertyChanged
    {
        private List<Card> drawnCards = new List<Card>();
        private int _total = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Total { get => _total; } //zbir vrijednosti karata
        public string Name { get; }
        public int Balance { get; set; } // novac

        public Player(string name)
        {
            Name = name;
            OnPropertyChanged();
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
            OnPropertyChanged();
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

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
