using MFS.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace 打印终端
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public IHubProxy HubProxy { get; set; }
        public HubConnection Connection { get; set; }
        string baseAddress = Properties.Settings.Default.baseAddress;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectAsync();
            this.Hide();
        }

        private async void ConnectAsync()
        {
            Connection = new HubConnection(baseAddress);
            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("print");
            HubProxy.On<SalesPlan>("printsalesplan", (salesplan) =>
                PrintSalesPlan(salesplan)
            );
            try
            {
                await Connection.Start();
            }
            catch (HttpClientException)
            {
                MessageBox.Show("网络不通");
            }
            textBox.AppendText($"Connected to server at {baseAddress}\r");
        }

        private void PrintSalesPlan(SalesPlan salesplan)
        {
            this.Dispatcher.Invoke(() => textBox.AppendText($"正在打印SalesPlan：{salesplan.Name}\r"));
        }

        private void Connection_Closed()
        {
            //Hide chat UI; show login UI
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() => textBox.AppendText("连接已经断开\r"));
            //断线重连
            ConnectAsync();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void TaskbarIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.Show();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
        }

    }
}
