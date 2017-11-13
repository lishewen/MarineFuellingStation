﻿using MFS.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
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
using 打印终端.Models;
using Word = Microsoft.Office.Interop.Word;

namespace 打印终端
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public HubConnection Connection { get; set; }
        string baseAddress = Properties.Settings.Default.baseAddress;
        private static Word._Document wDoc = null; //word文档
        private static Word._Application wApp = null; //word进程
        private string folder = "pos\\";
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
            Log.Logs += $"Connecting to {baseAddress}\r";
            Connection = await ConnectAsync(baseAddress);
            Log.Logs += $"Connected to server at {baseAddress}\r";
            Connection.Closed += Connection_Closed;
            Connection.On<SalesPlan>("printsalesplan", (salesplan) =>
            {
                PrintSalesPlan(salesplan);
            });
            Connection.On<Order>("printorder", (order) =>
            {
                PrintOrder(order);
            });
            Connection.On<Purchase>("printunload", (p) =>
            {
                PrintUnload(p);
            });
            Connection.On<MoveStore>("printmovestore", (m) =>
            {
                PrintMoveStore(m);
            });
            Connection.On<BoatClean>("printboatclean", (m) =>
            {
                PrintBoatClean(m);
            });
            Connection.On<BoatClean>("printboatcleancollection", (m) =>
            {
                PrintBoatCleanCollection(m);
            });
            Connection.On<Order>("printlandload", (m) =>
            {
                PrintLandLoad(m);
            });
            Connection.On<Order>("printprepayment", (m) =>
            {
                PrintPrepayment(m);
            });
            Connection.On<Purchase>("printunload1", (p) =>
            {
                PrintUnload1(p);
            });
            Connection.On<string>("login", (username) => Log.Logs += username + " 已登录，正在执行操作\r");
        }

        private void PrintPrepayment(Order order)
        {
            Log.Logs += $"正在打印Prepayment：{order.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.PrintPrepaymentDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#CarNo#", order.CarNo);
            WordReplace(wApp, "#CreateAt#", order.CreatedAt.ToString());
            WordReplace(wApp, "#ClientName#", order.Client.Name);
            WordReplace(wApp, "#CompanyName#", order.Client.Company.Name);
            WordReplace(wApp, "#Name#", order.Name);
            WordReplace(wApp, "#ProductName#", "石化油");
            WordReplace(wApp, "#Count#", order.Count.ToString());
            WordReplace(wApp, "#ProductCount#", order.Count.ToString());
            WordReplace(wApp, "#ProductPrice#", order.Price.ToString());
            WordReplace(wApp, "#TotalMoney#", order.TotalMoney.ToString());
            WordReplace(wApp, "#CreateBy#", order.CreatedBy);
            WordReplace(wApp, "#Salesman#", order.Salesman);

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

        private void PrintLandLoad(Order order)
        {
            Log.Logs += $"正在打印LoadOil：{order.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.PrintLandLoadDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#Name#", order.Name);
            WordReplace(wApp, "#CarNo#", order.CarNo);
            WordReplace(wApp, "#CreateAt#", order.CreatedAt.ToString());
            WordReplace(wApp, "#ClientName#", order.Client.Name);
            WordReplace(wApp, "#CompanyName#", order.Client.Company.Name);
            
            WordReplace(wApp, "#ProductName#", "石化油");
            WordReplace(wApp, "#Count#", order.Count.ToString());
            WordReplace(wApp, "#Unit#", order.Unit);
            WordReplace(wApp, "#Price#", order.Price.ToString());
            WordReplace(wApp, "#TotalMoney#", order.TotalMoney.ToString());
            WordReplace(wApp, "#CNMoney#", ConvertToChinese(order.TotalMoney));
            WordReplace(wApp, "#Remark#", order.Remark);
            WordReplace(wApp, "#LastUpdatedBy#", order.LastUpdatedBy);
            WordReplace(wApp, "#Salesman#", order.Salesman);

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

        private void PrintBoatCleanCollection(BoatClean m)
        {
            Log.Logs += $"正在打印BoatCleanCollection：{m.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.PrintBoatCleanCollectionDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#CreateAt#", m.CreatedAt.ToString());
            WordReplace(wApp, "#CreateBy#", m.CreatedBy);
            WordReplace(wApp, "#PayType#", m.Payments.First().PayTypeId.ToString());
            WordReplace(wApp, "#Money#", m.Money.ToString());
            WordReplace(wApp, "#Phone#", m.Phone);
            WordReplace(wApp, "#CompanyName#", m.Company);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + m.Name + ".docx";
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

        private void PrintBoatClean(BoatClean m)
        {
            Log.Logs += $"正在打印BoatClean：{m.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.PrintBoatCleanDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#CarNo#", m.CarNo);
            WordReplace(wApp, "#CreateAt#", m.CreatedAt.ToString());
            WordReplace(wApp, "#Name#", m.Name);
            WordReplace(wApp, "#Voyage#", m.Voyage.ToString());
            WordReplace(wApp, "#Tonnage#", m.Tonnage.ToString());
            WordReplace(wApp, "#ResponseId#", m.ResponseId);
            WordReplace(wApp, "#Address#", m.Address);
            WordReplace(wApp, "#Company#", m.Company);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + m.Name + ".docx";
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

        private void PrintOrder(Order order)
        {
            Log.Logs += $"正在打印Order：{order.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder + Properties.Settings.Default.PrintOrderDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#Name#", order.Name);
            WordReplace(wApp, "#CarNo#", order.CarNo);
            WordReplace(wApp, "#ProductName#", "石化油");
            WordReplace(wApp, "#Count#", order.Count.ToString());
            WordReplace(wApp, "#Unit#", order.Unit);
            WordReplace(wApp, "#Price#", order.Price.ToString());
            WordReplace(wApp, "#TotalMoney#", order.TotalMoney.ToString());
            WordReplace(wApp, "#CNMoney#", ConvertToChinese(order.TotalMoney));
            WordReplace(wApp, "#Remark#", order.Remark);
            WordReplace(wApp, "#LastUpdatedBy#", order.LastUpdatedBy);
            WordReplace(wApp, "#Salesman#", order.Salesman);
            WordReplace(wApp, "#CreatedAt#", order.CreatedAt.ToString());

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

        private void PrintUnload(Purchase p)
        {
            Log.Logs += $"正在打印陆上卸油：{p.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.PrintUnloadDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#CarNo#", p.CarNo);
            WordReplace(wApp, "#TrailerNo#", p.TrailerNo);
            WordReplace(wApp, "#ProductName#", p.Product?.Name);
            WordReplace(wApp, "#Name#", p.Name);
            WordReplace(wApp, "#CreateAt#", p.CreatedAt.ToLongDateString());
            WordReplace(wApp, "#UpdateBy#", p.LastUpdatedBy);
            WordReplace(wApp, "#Count#", p.Count.ToString());
            WordReplace(wApp, "#StoreName#", p.ToStores?[0].Name);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + p.Name + ".docx";
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

        private void PrintUnload1(Purchase p)
        {
            Log.Logs += $"正在打印出库石化过磅单：{p.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.PrintUnloadDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#CreateAt#", p.CreatedAt.ToLongDateString());
            WordReplace(wApp, "#CreateBy#", p.CreatedBy);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + p.Name + ".docx";
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

        private void PrintMoveStore(MoveStore m)
        {
            Log.Logs += $"正在打印生产转仓：{m.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.PrintMoveStoreDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#CreateBy#", m.CreatedBy);
            WordReplace(wApp, "#Name#", m.Name);
            WordReplace(wApp, "#CreateAt#", m.CreatedAt.ToLongDateString());

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + m.Name + ".docx";
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
            Log.Logs += $"正在打印SalesPlan：{salesplan.Name}\r";
        }

        private Task Connection_Closed(Exception e)
        {
            Log.Logs += "连接已经断开\r";
            //断线重连
            ConnectAsync();
            return Task.CompletedTask;
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

        private async Task<HubConnection> ConnectAsync(string baseUrl)
        {
            // Keep trying to until we can start
            while (true)
            {
                var connection = new HubConnectionBuilder()
                                .WithUrl(baseUrl)
                                .WithConsoleLogger(LogLevel.Trace)
                                .Build();
                try
                {
                    await connection.StartAsync();
                    //向Server发送ClientId
                    await connection.SendAsync("Conn", Properties.Settings.Default.ClientId);
                    return connection;
                }
                catch (Exception)
                {
                    await Task.Delay(1000);
                }
            }
        }
    }
}
