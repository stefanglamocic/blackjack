using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackjack.model
{
    public class Card
    {
        public int Value { get; set; }
        public string Rank { get; }
        public Suite Suite { get; }
        private string _imagePath = "assets/";
        public string ImagePath { get => _imagePath; }

        public Card(string rank, Suite suite)
        {
            Rank = rank;
            Suite = suite;
            Value = RankToValue(rank);
            setImagePath();
        }

        private int RankToValue(string rank)
        {
            switch(rank)
            {
                case "J":
                case "Q":
                case "K":
                    return 10;
                case "A":
                    return 11;
                default:
                    return Int32.TryParse(rank, out int v) ? v : 0;
            }
        }

        public override string ToString()
        {
            return $"{Rank} of {Suite}";
        }

        private void setImagePath()
        {
            switch(Rank)
            {
                case "A":
                    _imagePath += "A_of_";
                    break;
                case "J":
                    _imagePath += "J_of_";
                    break;
                case "Q":
                    _imagePath += "Q_of_";
                    break;
                case "K":
                    _imagePath += "K_of_";
                    break;
                default:
                    _imagePath += Rank + "_of_";
                    break;
            }

            switch(Suite)
            {
                case Suite.Club:
                    _imagePath += "clubs.png";
                    break;
                case Suite.Diamond:
                    _imagePath += "diamonds.png";
                    break;
                case Suite.Spade:
                    _imagePath += "spades.png";
                    break;
                case Suite.Heart:
                    _imagePath += "hearts.png";
                    break;
            }
        }
    }

    public enum Suite
    {
        Heart,
        Diamond,
        Spade,
        Club
    }
}
