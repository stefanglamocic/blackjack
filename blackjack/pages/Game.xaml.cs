using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using blackjack.model;

namespace blackjack.pages
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        private Player player;
        private Dealer dealer;
        private Deck deck;

        public Game(string playerName)
        {
            InitializeComponent();
            MainWindow.UpdateWindowSize(1180, 930);
            player = new Player(playerName);
            dealer = new Dealer();
            deck = new Deck();
            DataContext = player;
            HitStandStack.Visibility = Visibility.Hidden;
            InfoStack.Visibility = Visibility.Hidden;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home());
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Game(player.Name));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Chip_Click(object sender, RoutedEventArgs e)
        {
            int chipValue = Int32.Parse(((TextBlock)((Button)sender).Content).Text);
            if(chipValue < 0 && player.Bet >= -chipValue)
            {
                player.Bet += chipValue;
                player.Balance -= chipValue;
            }
            else if(chipValue < 0 && player.Bet < -chipValue)
            {
                player.Balance += player.Bet;
                player.Bet = 0;
            }
            else if(chipValue > 0 && player.Balance >= chipValue)
            {
                player.Bet += chipValue;
                player.Balance -= chipValue;
            }
            else if(chipValue > 0 && player.Balance < chipValue)
            {
                player.Bet += player.Balance;
                player.Balance = 0;
            }
        }

        private void AddCardToCanvas(Canvas canvas, int index)
        {
            var person = canvas.Equals(PlayerCanvas) ? player : dealer;

            var b = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1.2),
                CornerRadius = new CornerRadius(3),
                Child = new Image
                {
                    Height = 94,
                    Source = (canvas.Equals(DealerCanvas) && index == 1) ?
                        new BitmapImage(new Uri("pack://application:,,,/assets/back.png")) :
                        new BitmapImage(new Uri("pack://application:,,,/" + person.DrawnCards[index].ImagePath))
                }
            };

            Canvas.SetLeft(b, index * 32);
            canvas.Children.Add(b);
        }

        private void RevealHiddenCard()
        {
            Border b = (Border)DealerCanvas.Children[1];
            b.Child = new Image
            {
                Height = 94,
                Source = new BitmapImage(new Uri("pack://application:,,,/" + dealer.DrawnCards[1].ImagePath))
            };
        }

        private void DisableChipButtons()
        {
            foreach(var b in ChipsStack.Children)
            {
                if (b is Button button)
                    button.IsEnabled = false;
            }
        }

        private void DealButton_Click(object sender, RoutedEventArgs e)
        {
            dealer.Hit(deck);
            dealer.Hit(deck);

            AddCardToCanvas(DealerCanvas, 0);
            AddCardToCanvas(DealerCanvas, 1);

            RevealHiddenCard();

            HitStandStack.Visibility = Visibility.Visible;

            ChipsStack.Visibility = Visibility.Hidden;
        }
    }
}
