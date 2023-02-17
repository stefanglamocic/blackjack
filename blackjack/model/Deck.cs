using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackjack.model
{
    class Deck
    {
        private List<Card> Cards = new List<Card>();

        public Deck()
        {
            for(int i = 0; i < 4; i++) 
            {
                Suite suite = (Suite)i;
                for(int j = 1; j <= 13; j++)
                {
                    string rank;
                    switch(j)
                    {
                        case 1:
                            rank = "A";
                            break;
                        case 11:
                            rank = "J";
                            break;
                        case 12:
                            rank = "Q";
                            break;
                        case 13:
                            rank = "K";
                            break;
                        default:
                            rank = j.ToString();
                            break;
                    }

                    Cards.Add(new Card(rank, suite));
                }
            }

            Random rng = new Random();
            Cards = Cards.OrderBy(c => rng.Next()).ToList();
        }

        public Card DrawCard()
        {
            Card temp = Cards.Last();
            Cards.RemoveAt(Cards.Count - 1);
            return temp;
        }
    }
}
