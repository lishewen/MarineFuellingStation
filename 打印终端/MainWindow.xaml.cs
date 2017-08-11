using MFS.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Word = Microsoft.Office.Interop.Word;

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
        private static Word._Document wDoc = null; //word文档
        private static Word._Application wApp = null; //word进程
        object missing = System.Reflection.Missing.Value;

        static string ConvertToChinese(decimal x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString());
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectAsync();

            //测试
            //PrintOrder(new Order
            //{
            //    CarNo = "ct200",
            //    CreatedAt = DateTime.Now,
            //    Name = "20170801"
            //});

            this.Hide();
        }

        private async void ConnectAsync()
        {
            Connection = new HubConnection(baseAddress);
            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("print");
            HubProxy.On<SalesPlan>("printsalesplan", (salesplan) =>
            {
                PrintSalesPlan(salesplan);
            });
            HubProxy.On<Order>("printorder", (order) =>
            {
                PrintOrder(order);
            });
            HubProxy.On<string>("login", (username) => Dispatcher.Invoke(() => textBox.AppendText(username + " 已登录，正在执行操作\r")));
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

        private void PrintOrder(Order order)
        {
            this.Dispatcher.Invoke(() => textBox.AppendText($"正在打印Order：{order.Name}\r"));

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + "调拨单.docx";
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#CarNo#", order.CarNo);
            WordReplace(wApp, "#CreateAt#", order.CreatedAt.ToString());
            WordReplace(wApp, "#Name#", order.Name);
            WordReplace(wApp, "#ProductName#", order.Product.Name);
            WordReplace(wApp, "#ProductCount#", order.Count.ToString());
            WordReplace(wApp, "#ProductPrice#", order.Price.ToString());
            WordReplace(wApp, "#TotalMoney#", order.TotalMoney.ToString());
            WordReplace(wApp, "#CreateBy#", order.CreatedBy);
            WordReplace(wApp, "#CNMoney#", ConvertToChinese(order.TotalMoney));

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + order.Name + ".docx";
            wDoc.SaveAs(ref filename, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref
                missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            wDoc.PrintOut(ref background, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref
               missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
               ref missing);
            object saveOption = Word.WdSaveOptions.wdSaveChanges;
            wDoc.Close(ref saveOption, ref missing, ref missing); //关闭当前文档，如果有多个模版文件进行操作，则执行完这一步后接着执行打开Word文档的方法即可
            saveOption = Word.WdSaveOptions.wdDoNotSaveChanges;
            wApp.Quit(ref saveOption, ref missing, ref missing); //关闭Word进程
        }

        private void WordReplace(Word._Application wApp, string oldstr, string newstr)
        {
            //替换模版中的字符开始
            object replaceAll = Word.WdReplace.wdReplaceAll; //替换所有
            wApp.Selection.Find.ClearFormatting();
            wApp.Selection.Find.Text = oldstr;        //替换的字符为#old#
            wApp.Selection.Find.Format = false;
            wApp.Selection.Find.Forward = true;    //向前查找
            wApp.Selection.Find.MatchByte = true;
            wApp.Selection.Find.Wrap = Word.WdFindWrap.wdFindAsk;
            wApp.Selection.Find.Replacement.ClearFormatting();
            wApp.Selection.Find.Replacement.Text = newstr; //替换后新字符
            wApp.Selection.Find.Execute(
             ref missing, ref missing, ref missing, ref missing, ref missing,
             ref missing, ref missing, ref missing, ref missing, ref missing,
             ref replaceAll, ref missing, ref missing, ref missing, ref missing);
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
