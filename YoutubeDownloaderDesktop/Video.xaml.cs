using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloaderDesktop
{
    /// <summary>
    /// Interaction logic for Video.xaml
    /// </summary>
    public partial class Video : Window
    {
        public Video()
        {
            InitializeComponent();
            this.title.Text = MainWindow.video.Title;
            this.duration.Text = MainWindow.video.Duration.ToString();
            Video.UrlConverterToImg(MainWindow.video.Thumbnails[0].Url, this.VidImg);
        }

        public static void UrlConverterToImg(string url, Image img)
        {
            BitmapImage bitmapImage = new BitmapImage();
            int count1 = 100;
            WebRequest webRequest = WebRequest.Create(new Uri(url, UriKind.Absolute));
            webRequest.Timeout = -1;
            BinaryReader binaryReader = new BinaryReader(webRequest.GetResponse().GetResponseStream());
            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[count1];
            for (int count2 = binaryReader.Read(buffer, 0, count1); count2 > 0; count2 = binaryReader.Read(buffer, 0, count1))
                memoryStream.Write(buffer, 0, count2);
            bitmapImage.BeginInit();
            memoryStream.Seek(0L, SeekOrigin.Begin);
            bitmapImage.StreamSource = (Stream)memoryStream;
            bitmapImage.EndInit();
            img.Source = (ImageSource)bitmapImage;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Video video = this;
            try
            {
                video.BackSplash.Opacity = 100.0;
                IVideoStreamInfo highestVideoQuality = ((IEnumerable<IVideoStreamInfo>)(await MainWindow.client.Videos.Streams.GetManifestAsync(MainWindow.video.Id)).GetMuxedStreams()).GetWithHighestVideoQuality();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.ShowDialog();
                ProgressBar progressBar = new ProgressBar();
                Progress<double> progress = new Progress<double>((Action<double>)(p => progressBar.myProgress = (p * 100.0).ToString("N3") + "%"));
                progressBar.Show();
                await MainWindow.client.Videos.Streams.DownloadAsync((YoutubeExplode.Videos.Streams.IStreamInfo)highestVideoQuality, string.Format("{0}.{1}", (object)saveFileDialog.FileName, (object)highestVideoQuality.Container), (IProgress<double>)progress);
                video.BackSplash.Opacity = 100.0;
                progressBar.Close();
                int num = (int)MessageBox.Show("Download Complete UwU");
                video.Close();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
            video.BackSplash.Opacity = 100.0;
        }

    }
}
