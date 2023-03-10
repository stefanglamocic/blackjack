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
        private int _balance;
        private int _bet;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Card> DrawnCards { get { return drawnCards; } }
        public int Total { get => _total; } //zbir vrijednosti karata
        public string Name { get; }
        public int Balance {
            get => _balance;
            set 
            {
                _balance = value;
                OnPropertyChanged();
            } } // novac
        public int Bet {
            get => _bet;
            set
            {
                _bet = value;
                OnPropertyChanged();
            }
        }

        public Player(string name)
        {
            Name = name;
            Balance = 10000;
            Bet = 0;
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

        public void ClearData()
        {
            Bet = 0;
            _total = 0;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
