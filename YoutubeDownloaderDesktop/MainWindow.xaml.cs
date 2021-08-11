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
using YoutubeExplode;
using YoutubeExplode.Videos;

namespace YoutubeDownloaderDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static YoutubeExplode.Videos.Video video;
        public static YoutubeClient client = new YoutubeClient();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void check_link(object sender, RoutedEventArgs e)
        {
            bool Valide = true;
            try
            {
                this.BackSplash.Opacity = 100.0;
                video = await client.Videos.GetAsync(Link.Text);
                this.BackSplash.Opacity = 0.0;
            }
            catch (Exception ex)
            {
                Valide = false;
                int num = (int)MessageBox.Show(ex.Message);
            }
            this.BackSplash.Opacity = 0.0;
            if (!Valide)
                return;
            new Video().Show();
        }
    }
}
