using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public partial class serverWindow : Window
    {
        private Socket socket;
        private List<Socket> clients = new List<Socket>();
        public serverWindow()
        {
            InitializeComponent();
            var ipPoint = new IPEndPoint(IPAddress.Any, 1489);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(100);

            ListenCl();
        }

        private async Task ListenCl()
        {
            while (true)
            {
                var client = await socket.AcceptAsync();
                clients.Add(client);

                ReceiveMsg(client);
            }
        }

        private async Task ReceiveMsg(Socket client)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await client.ReceiveAsync(bytes, SocketFlags.None);
                string msg = Encoding.UTF8.GetString(bytes);

                messagesList.Items.Add("сообщение от " + client.RemoteEndPoint + ": " + msg);

                foreach(var cl in clients)
                {
                    SendMsg(cl, msg);
                }
            }
        }

        private async Task SendMsg(Socket client, string msg)
        {
            byte[] bytes = new byte[1024];
            await client.SendAsync(bytes, SocketFlags.None);
        }

        private void Misha_Click(object sender, RoutedEventArgs e)
        {
            var w = new MainWindow();
            w.Show();
            this.Close();
        }
    }
}
