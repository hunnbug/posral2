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
        private List<Socket> sockets = new List<Socket>();
        private List<string> clients = new List<string>();
        Logs logi = new Logs();
        public serverWindow()
        {
            InitializeComponent();
            var ipPoint = new IPEndPoint(IPAddress.Any, 1489);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(100);

            ListenCl();
        }


        private void ZapisvLogi(string zapis)
        {
            logi.logs.Add(zapis);
        }

        private async Task ListenCl()
        {
            while (true)
            {
                var client = await socket.AcceptAsync();
                sockets.Add(client);
                pediki.ItemsSource = null;
                pediki.ItemsSource = sockets;

                ReceiveMsg(client);
                Disconnected(client);
            }
        }


        private async Task ReceiveMsg(Socket client)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await client.ReceiveAsync(bytes, SocketFlags.None);
                string msg = Encoding.UTF8.GetString(bytes);

                messagesList.Items.Add(DateTime.Now + " " + "сообщение от " + client.RemoteEndPoint + ": " + msg);
                ZapisvLogi(DateTime.Now + " " + "сообщение от " + client.RemoteEndPoint + ": " + msg);

                foreach(var cl in sockets)
                {
                    SendMsg(cl, msg);
                }
            }
        }

        private async Task SendMsg(Socket client, string msg)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(msg);
            await client.SendAsync(bytes, SocketFlags.None);
        }

        private void Misha_Click(object sender, RoutedEventArgs e)
        {
            var w = new MainWindow();
            w.Show();
            this.Close();
        }

        private async Task Disconnected(Socket client)
        {
            if(!client.Connected)
            {
                sockets.Remove  (client);
                client.Shutdown (SocketShutdown.Both);
                client.Close();

                ZapisvLogi(DateTime.Now + " " + $"Disconnected {client.RemoteEndPoint}");
                messagesList.Items.Add(DateTime.Now + " " + $"Disconnected {client.RemoteEndPoint}");
            }
        }

        private void Returns_Click(object sender, RoutedEventArgs e)
        {
            var w = new logsWindow(logi);
            w.Show();
        }
    }
}
