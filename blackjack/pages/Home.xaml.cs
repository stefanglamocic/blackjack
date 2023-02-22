using System;
using System.Collections.Generic;
using System.Linq;
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

namespace blackjack.pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            MainWindow.UpdateWindowSize(800, 600);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string playerName = tbPlayerName.Text.Trim();
            if (!String.IsNullOrEmpty(playerName) && !playerName.Equals("Username..."))
            {
                NavigationService.Navigate(new Game(playerName));
            }
            else
                MessageBox.Show("Unesite ime");
        }

        private void tbPlayerName_GotFocus(object sender, RoutedEventArgs e)
        {
            if(tbPlayerName.Text.Equals("Username..."))
            {
                tbPlayerName.Text = "";
                tbPlayerName.Opacity = 1;
            }
        }

        private void tbPlayerName_LostFocus(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(tbPlayerName.Text))
            {
                tbPlayerName.Text = "Username...";
                tbPlayerName.Opacity = 0.3;
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Arrow.Opacity = 1;
            ((Button)sender).Background = Application.Current.Resources["YellowLight"] as SolidColorBrush;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow.Opacity = 0.6;
            ((Button)sender).Background = Application.Current.Resources["YellowDark"] as SolidColorBrush;
        }
    }
}
