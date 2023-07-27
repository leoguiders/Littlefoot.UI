using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TcpClient = NetCoreServer.TcpClient;

namespace Littlefoot.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ByteClient _byteClient;

        class ByteClient : TcpClient
        {
            private Action<byte[], long, long> ReceiveMessage;

            public ByteClient(string hostname, int port, Action<byte[], long, long> receiveMessage) : base(hostname, port)
            {
                ReceiveMessage = receiveMessage;
            }

            public void DisconnectAndStop()
            {
                _stop = true;
                DisconnectAsync();
                while (IsConnected)
                    Thread.Yield();
            }

            protected override void OnConnected()
            {
                Trace.WriteLine($"TCP client connected a new session with Id {Id}");
            }

            protected override void OnDisconnected()
            {
                Trace.WriteLine($"TCP client disconnected a session with Id {Id}");

                // Wait for a while...
                Thread.Sleep(1000);

                // Try to connect again
                if (!_stop)
                    ConnectAsync();
            }

            protected override void OnReceived(byte[] buffer, long offset, long size)
            {
                ReceiveMessage(buffer, offset, size);
            }

            protected override void OnError(SocketError error)
            {
                Trace.WriteLine($"TCP client caught an error with code {error}");
            }

            private bool _stop;
        }

        public MainWindow()
        {
            InitializeComponent();
            StartTCPClient();
        }

        private void StartTCPClient()
        {
            // TCP server address
            string address = "127.0.0.1";

            // TCP server port
            int port = 8556;

            Trace.WriteLine($"TCP server address: {address}");
            Trace.WriteLine($"TCP server port: {port}");

            // Create a new TCP chat client
            _byteClient = new ByteClient(address, port, OnReceived);

            // Connect the client
            Trace.Write("Client connecting...");
            _byteClient.ConnectAsync();
            Trace.WriteLine("Done!");

        }

        private void OnReceived(byte[] buffer, long offset, long size)
        {
            Shape[] fixtures = { Fixture1, Fixture2, Fixture3, Fixture4, Fixture5, Fixture6 };
            
            for (int i = 0; i < 6; i++)
            {
                var master = buffer[19 + (i * 9)];
                var red = buffer[21 + (i * 9)];
                var green = buffer[22 + (i * 9)];
                var blue = buffer[23 + (i * 9)];
                var amber = buffer[24 + (i * 9)];

                fixtures[i].Dispatcher.Invoke(new Action(() => {
                    SolidColorBrush mySolidColorBrush = new();
                    mySolidColorBrush.Color = Color.FromArgb(master, red, green, blue);
                    fixtures[i].Fill = mySolidColorBrush;
                }));
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            SendCommand("START_STOP");
        }

        private void SendCommand(string command)
        {
            if (_byteClient.IsConnected)
                _byteClient.SendAsync(Encoding.UTF8.GetBytes(command));
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            SendCommand("PAUSE_CONTINUE");
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            SendCommand("TAP");
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button9_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button10_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FadeTimePedal_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void MasterDimmerPedal_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
