using MFS.Models;
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
                //调拨单 
                PrintOrder(order);
            });
            Connection.On<Purchase>("printunload", (p) =>
            {
                //陆上卸油单
                PrintUnload(p);
            });
            Connection.On<MoveStore>("printmovestore", (m) =>
            {
                //生产转仓单
                PrintMoveStore(m);
            });
            Connection.On<BoatClean>("printboatclean", (m) =>
            {
                //船舶清污单
                PrintBoatClean(m);
            });
            Connection.On<BoatClean>("printboatcleancollection", (m) =>
            {
                //船舶清污收款单
                PrintBoatCleanCollection(m);
            });
            Connection.On<Order>("printlandload", (m) =>
            {
                //陆上装车单
                PrintLandLoad(m);
            });
            Connection.On<Order>("printdeliver", (m) =>
            {
                //陆上送货单
                PrintDeliver(m);
            });
            Connection.On<ChargeLog>("printclientprepayment", (c) =>
            {
                //个人预收款确认单
                PrintClientPrepayment(c);
            });
            Connection.On<ChargeLog>("printcompanyprepayment", (c) =>
            {
                //公司预收款确认单
                PrintCompanyPrepayment(c);
            });
            Connection.On<Order>("printponderation", (m) =>
            {
                //出库石化过磅单
                PrintPonderation(m);
            });
            Connection.On<string>("login", (username) => Log.Logs += username + " 已登录，正在执行操作\r");
        }
        #region 个人预收款确认单
        /// <summary>
        /// 个人预收款确认单
        /// </summary>
        /// <param name="order"></param>
        private void PrintClientPrepayment(ChargeLog cl)
        {
            Log.Logs += $"正在打印ClientPrepayment：{cl.Client.CarNo}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder + Properties.Settings.Default.PrintClientPrepaymentDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#CarNo#", cl.Client.CarNo);
            WordReplace(wApp, "#Money#", cl.Money.ToString("0.00"));
            WordReplace(wApp, "#CNMoney#", ConvertToChinese(cl.Money));
            WordReplace(wApp, "#CreatedBy#", cl.CreatedBy);
            WordReplace(wApp, "#CreatedAt#", cl.CreatedAt.ToString()); 
            WordReplace(wApp, "#PayType#", strPayType(cl.PayType));
            WordReplace(wApp, "#Balances#", cl.Client.Balances.ToString());
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + cl.Client.Name + ".docx";
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
        #endregion
        #region 公司预收款确认单
        /// <summary>
        /// 公司预收款确认单
        /// </summary>
        /// <param name="order"></param>
        private void PrintCompanyPrepayment(ChargeLog cl)
        {
            Log.Logs += $"正在打印ClientPrepayment：{cl.Company.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder + Properties.Settings.Default.PrintCompanyPrepaymentDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#CompanyName#", cl.Company.Name);
            WordReplace(wApp, "#Money#", cl.Money.ToString("0.00"));
            WordReplace(wApp, "#CNMoney#", ConvertToChinese(cl.Money));
            WordReplace(wApp, "#CreatedBy#", cl.CreatedBy);
            WordReplace(wApp, "#CreatedAt#", cl.CreatedAt.ToString());
            WordReplace(wApp, "#PayType#", strPayType(cl.PayType));
            WordReplace(wApp, "#Balances#", cl.Company.Balances.ToString());
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + cl.Company.Name + ".docx";
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
        #endregion
        #region 陆上装车单
        /// <summary>
        /// 陆上装车单
        /// </summary>
        /// <param name="order">model</param>
        private void PrintLandLoad(Order order)
        {
            Log.Logs += $"正在打印LoadOil：{order.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder +  Properties.Settings.Default.PrintLandLoadDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            decimal DiffWeightToLitre = order.DiffWeight / (decimal)order.Density * 1000;
            //填充数据
            WordReplace(wApp, "#Name#", order.Name);
            WordReplace(wApp, "#ClientName#", order.Client.CarNo);
            WordReplace(wApp, "#CompanyName#", order.Client.Company?.Name);
            WordReplace(wApp, "#StoreName#", order.Store.Name);
            WordReplace(wApp, "#DiffWeight#", order.DiffWeight.ToString("0.00"));
            WordReplace(wApp, "#Count#", order.Count.ToString("0.00"));
            WordReplace(wApp, "#DiffOrder#", (order.DiffWeight - order.Count).ToString());
            WordReplace(wApp, "#Price#", order.IsPrintPrice? order.Price.ToString() : "0.00");
            WordReplace(wApp, "#TotalMoney#", order.TotalMoney.ToString());
            WordReplace(wApp, "#OilCount#", order.OilCountLitre.ToString());
            WordReplace(wApp, "#DiffWeightToLitre#", DiffWeightToLitre.ToString("0.00"));
            WordReplace(wApp, "#DiffLitre#", (order.OilCountLitre - DiffWeightToLitre).ToString("0.00"));
            WordReplace(wApp, "#Instrument_bf#", (order.Instrument1 - order.OilCountLitre).ToString());
            WordReplace(wApp, "#Instrument_af#", order.Instrument1.ToString());
            WordReplace(wApp, "#Density#", order.Density.ToString("0.000"));
            //WordReplace(wApp, "#OilTemperature#", order.OilTemperature.ToString());
            WordReplace(wApp, "#Worker#", order.Worker);
            WordReplace(wApp, "#LastUpdatedAt#", order.LastUpdatedAt.ToString());
            WordReplace(wApp, "#Salesman#", order.Salesman);
            PrintTime(wApp);

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
        #endregion
        private void PrintBoatCleanCollection(BoatClean m)
        {
            Log.Logs += $"正在打印BoatCleanCollection：{m.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder + Properties.Settings.Default.PrintBoatCleanCollectionDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#Name#", m.Name.ToString());
            WordReplace(wApp, "#CarNo#", m.CarNo);
            WordReplace(wApp, "#Company#", m.Company);
            WordReplace(wApp, "#Phone#", m.Phone);
            WordReplace(wApp, "#Money#", m.Money.ToString("0.00"));
            WordReplace(wApp, "#CNMoney#", ConvertToChinese(m.Money));
            WordReplace(wApp, "#LastUpdatedBy#", m.LastUpdatedBy);
            WordReplace(wApp, "#CreatedAt#", m.CreatedAt.ToString());
            PrintTime(wApp);

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
        /// <summary>
        /// 船舶清污完工证
        /// </summary>
        /// <param name="m"></param>
        private void PrintBoatClean(BoatClean m)
        {
            Log.Logs += $"正在打印BoatClean：{m.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder + Properties.Settings.Default.PrintBoatCleanDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#Name#", m.Name);
            WordReplace(wApp, "#CarNo#", m.CarNo);
            WordReplace(wApp, "#Company#", m.Company);
            WordReplace(wApp, "#Voyage#", m.Voyage.ToString());
            WordReplace(wApp, "#Tonnage#", m.Tonnage.ToString());
            WordReplace(wApp, "#ResponseId#", m.ResponseId);
            WordReplace(wApp, "#Address#", m.Address);
            WordReplace(wApp, "#CreatedAt#", m.CreatedAt.ToString());
            WordReplace(wApp, "#EndTime#", m.EndTime.ToString());
            PrintTime(wApp);

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
        #region 调拨单
        /// <summary>
        /// 调拨单 也是收款单 水上陆上共用
        /// </summary>
        /// <param name="order"></param>
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
            WordReplace(wApp, "#Price#", order.IsPrintPrice ? order.Price.ToString("0.00") : "");
            WordReplace(wApp, "#TotalMoney#", order.IsPrintPrice? order.TotalMoney.ToString("0.00") : "");
            WordReplace(wApp, "#CNMoney#", ConvertToChinese(order.TotalMoney));
            WordReplace(wApp, "#Remark#", order.Remark);
            WordReplace(wApp, "#LastUpdatedBy#", order.LastUpdatedBy);
            WordReplace(wApp, "#Salesman#", order.Salesman);
            WordReplace(wApp, "#CreatedAt#", order.CreatedAt.ToString());
            PrintTime(wApp);

            string strInvoice = "";
            if (order.IsInvoice)
            {   
                strInvoice += "开票单位：" + order.BillingCompany + "\r";
                strInvoice += "开票单价：￥" + order.BillingPrice + "\r";
                strInvoice += "开票数量：" + order.BillingCount + order.Unit + "\r";
                strInvoice += "类型：" + strTicketType(order.TicketType);
            }
            WordReplace(wApp, "#InvoiceContent#", strInvoice);

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
        #endregion
        #region 陆上送货单
        private void PrintDeliver(Order order)
        {
            Log.Logs += $"正在打印Deliver：{order.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder + Properties.Settings.Default.PrintDeliverDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#Name#", order.Name);
            WordReplace(wApp, "#CarNo#", order.CarNo);
            WordReplace(wApp, "#Count#", order.Count.ToString());
            WordReplace(wApp, "#Unit#", order.Unit);
            WordReplace(wApp, "#Price#", order.IsPrintPrice ? order.Price.ToString() : "0.00");
            WordReplace(wApp, "#TotalMoney#", order.IsPrintPrice? order.TotalMoney.ToString() : "0.00");
            WordReplace(wApp, "#CNMoney#", order.IsPrintPrice? ConvertToChinese(order.TotalMoney) : "");
            WordReplace(wApp, "#Remark#", order.Remark);
            WordReplace(wApp, "#CreatedBy#", order.CreatedBy);
            WordReplace(wApp, "#Salesman#", order.Salesman);
            WordReplace(wApp, "#CreatedAt#", order.CreatedAt.ToString());
            PrintTime(wApp);

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
        #endregion
        private void PrintUnload(Purchase p)
        {
            Log.Logs += $"正在打印陆上卸油单：{p.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder + Properties.Settings.Default.PrintUnloadDocx;
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
            PrintTime(wApp);

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
        /// <summary>
        /// 出库石化过磅单
        /// </summary>
        /// <param name="p"></param>
        private void PrintPonderation(Order o)
        {
            Log.Logs += $"正在打印出库石化过磅单：{o.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder + Properties.Settings.Default.PrintPonderationDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#Name#", o.Name);
            WordReplace(wApp, "#CarNo#", o.CarNo);
            WordReplace(wApp, "#OilCarWeight#", o.OilCarWeight.ToString("0.00"));
            WordReplace(wApp, "#EmptyCarWeight#", o.EmptyCarWeight.ToString("0.00"));
            WordReplace(wApp, "#DiffWeight#", o.DiffWeight.ToString("0.00"));
            WordReplace(wApp, "#Worker#", o.Worker);
            WordReplace(wApp, "#LastUpdatedAt#", o.LastUpdatedAt.ToString());
            WordReplace(wApp, "#EndOilDateTime#", o.EndOilDateTime.ToString());
            WordReplace(wApp, "#StartOilDateTime#", o.StartOilDateTime.ToString());
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + o.Name + ".docx";
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
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder + Properties.Settings.Default.PrintMoveStoreDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#CreateBy#", m.CreatedBy);
            WordReplace(wApp, "#Name#", m.Name);
            WordReplace(wApp, "#CreateAt#", m.CreatedAt.ToLongDateString());
            PrintTime(wApp);

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
        /// <summary>
        /// 每张单固定底部分别都打印当前打印操作的时间
        /// </summary>
        /// <param name="wApp"></param>
        private void PrintTime(Word._Application wApp)
        {
            WordReplace(wApp, "#PrintTime#", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        }
        private string strPayType(OrderPayType op)
        {
            if (op == OrderPayType.现金)
                return "现金";
            else if (op == OrderPayType.工行刷卡 || op == OrderPayType.桂行刷卡)
                return "刷卡";
            else if (op == OrderPayType.微信)
                return "微信";
            else if (op == OrderPayType.支付宝)
                return "支付宝";
            else
                return "";
        }
        private string strTicketType(TicketType type)
        {
            switch (type)
            {
                case TicketType.循票:
                    return "循票";
                case TicketType.柴票:
                    return "柴票";
            }
            return "";
        }
    }
}
