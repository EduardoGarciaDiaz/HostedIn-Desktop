using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HostedIn_DesktopClient.Models;
using Newtonsoft.Json;

namespace HostedIn_DesktopClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient HTTP_CLIENT = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateNewAccount();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private async void  CreateNewAccount()
        {
            //TO-DO recuperar campos de GUI
            User user = new User("correoPrueba@gmail.com", "Tristan Eduardo", DateTime.Now, "2281549222", "contraseña", "estudiante", "Xalapa", new byte[0]);
            ApiRest apiRest = ApiRest.GetInstance();
            HttpResponseMessage result = await apiRest.CreateAccount(user);
            string jsonResponse = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                User newSaved = JsonConvert.DeserializeObject<User>(jsonResponse);
                txbBlock.Text = "El usaurio " + newSaved.fullName + " fue guardado correctamente";
            }
            else
            {
                txbBlock.Text = jsonResponse;
            }
        }


        

    }
}
