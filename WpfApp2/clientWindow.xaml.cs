using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace WpfApp2
{
    public partial class clientWindow : Window
    {
        Socket server;
        public clientWindow()
        {
            InitializeComponent();

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Connect("127.0.0.1", 1489);
            ReceiveMsg();
        }
        private async Task ReceiveMsg()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await server.ReceiveAsync(bytes, SocketFlags.None);
                string msg = Encoding.UTF8.GetString(bytes);

                listbx.Items.Add(msg);
            }
        }

        private async Task SendMsg(string msg)
        {
            byte[] bytes = new byte[1024];
            await server.SendAsync(bytes, SocketFlags.None);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMsg(asd.Text);
        }
    }
}
