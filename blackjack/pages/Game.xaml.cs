using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
            
            DataContext = player;
            PrepareForNewDealing();
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

        private async void DealButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.Bet <= 0)
                return;
            DealButton.IsEnabled = false;
            player.DrawnCards.Clear();
            dealer.DrawnCards.Clear();
            deck = new Deck();
            ChipsStack.Visibility = Visibility.Hidden;

            for (int i = 0; i < 2; i++)
            {
                player.Hit(deck);
                AddCardToCanvas(PlayerCanvas, i);
                await Task.Run(() => Thread.Sleep(1000));

                dealer.Hit(deck);
                AddCardToCanvas(DealerCanvas, i);
                if(i < 1)
                    await Task.Run(() => Thread.Sleep(1000));
            }

            if(player.Total == 21)
            {
                await PlayerWonBlackjack();
                return;
            }

            HitStandStack.Visibility = Visibility.Visible;
        }

        private void PrepareForNewDealing()
        {
            PlayerCanvas.Children.Clear();
            DealerCanvas.Children.Clear();
            HitStandStack.Visibility = Visibility.Hidden;
            InfoStack.Visibility = Visibility.Hidden;
            ChipsStack.Visibility = Visibility.Visible;
            player.ClearData();
            dealer.ClearData();
            DealButton.IsEnabled = true;
        }

        private async Task PlayerWonBlackjack()
        {
            ResultTextBlock.Foreground = Application.Current.Resources["TitleColor"] as LinearGradientBrush;
            ResultTextBlock.Text = "Blackjack";
            ResultTextBlock.Visibility = Visibility.Visible;
            await Task.Run(() => Thread.Sleep(1500));
            ResultTextBlock.Visibility = Visibility.Hidden;

            DisplayPlayerWinInfo();
            player.Balance += 2 * player.Bet;
            player.Bet = 0;
            await Task.Run(() => Thread.Sleep(2500));

            PrepareForNewDealing();
        }

        private void DisplayPlayerWinInfo()
        {
            WinnerInfo.Text = $"{player.Name} won";
            WinningInfo.Text = $"{2 * player.Bet}";
            InfoStack.Visibility = Visibility.Visible;
        }

        private void DisplayDealerWinInfo()
        {
            WinnerInfo.Text = $"{dealer.Name} won";
            WinningInfo.Text = $"{player.Bet}";
            InfoStack.Visibility = Visibility.Visible;
        }

        private void DisableStandHit()
        {
            StandButton.IsEnabled = false;
            HitButton.IsEnabled = false;
        }

        private void EnableStandHit()
        {
            StandButton.IsEnabled = true;
            HitButton.IsEnabled = true;
        }

        private async void StandButton_Click(object sender, RoutedEventArgs e)
        {
            DisableStandHit();
            RevealHiddenCard();
            dealer.FinishDrawing(deck);
            for(int i = 2; i < dealer.DrawnCards.Count; i++)
            {
                await Task.Run(() => Thread.Sleep(1000));
                AddCardToCanvas(DealerCanvas, i);
            }

            if (player.Total >= dealer.Total || dealer.Total > 21)
            {
                DisplayPlayerWinInfo();
                player.Balance += 2 * player.Bet;
                player.Bet = 0;
            }
            else
                DisplayDealerWinInfo();
            await Task.Run(() => Thread.Sleep(2500));

            PrepareForNewDealing();
            EnableStandHit();
        }

        private async void HitButton_Click(object sender, RoutedEventArgs e)
        {
            DisableStandHit();
            player.Hit(deck);
            AddCardToCanvas(PlayerCanvas, PlayerCanvas.Children.Count);
            if(player.Total == 21)
            {
                await PlayerWonBlackjack();
            }
            else if(player.Total > 21)
            {
                ResultTextBlock.Foreground = Brushes.Red;
                ResultTextBlock.Text = "Bust";
                ResultTextBlock.Visibility = Visibility.Visible;
                await Task.Run(() => Thread.Sleep(1500));
                ResultTextBlock.Visibility = Visibility.Hidden;
                DisplayDealerWinInfo();
                await Task.Run(() => Thread.Sleep(2500));
                PrepareForNewDealing();
            }
            EnableStandHit();
        }
    }
}
