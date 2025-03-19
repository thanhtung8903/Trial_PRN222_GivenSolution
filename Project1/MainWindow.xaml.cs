using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Star> stars = new ObservableCollection<Star>();

        public MainWindow()
        {
            InitializeComponent();
            lbStars.ItemsSource = stars;
        }

        private void btnAddToList_Click(object sender, RoutedEventArgs e)
        {
            var star = new Star
            {
                Name = txtName.Text,
                Dob = dpDob.SelectedDate,
                Description = txtDescription.Text,
                Male = chMale.IsChecked ?? false,
                Nationality = txtNationality.Text
            };
            stars.Add(star);

            txtName.Text = "";
            dpDob.SelectedDate = null;
            txtDescription.Text = "";
            chMale.IsChecked = false;
            txtNationality.Text = "";
        }

        private void btnSendToServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(stars);
                using (TcpClient client = new TcpClient("127.0.0.1", 5000))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(jsonString);
                    stream.Write(data, 0, data.Length);

                    data = new byte[256];
                    int bytes = stream.Read(data, 0, data.Length);
                    string response = Encoding.UTF8.GetString(data, 0, bytes);

                    MessageBox.Show(response == "accepted" ? "Data sent succesfully!" : "Error" + response, "Server Response");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Connection Error");
            }
        }
    }
}