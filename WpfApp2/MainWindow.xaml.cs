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

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateServer_Click(object sender, RoutedEventArgs e)
        {
            var w = new serverWindow();
            //цюЫрщц()ж
            w.Show();
            this.Close();
        }

        private void CreateClient_Click(object sender, RoutedEventArgs e)
        {
            var w = new clientWindow(privetPider.Text);
            w.Show();
            this.Close();
        }
    }
}