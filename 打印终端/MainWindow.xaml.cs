using MFS.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.PowerPacks.Printing.Compatibility.VB6;
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
        private string createdfolder = "createdfile\\";
        private string folder = "pos\\";
        object missing = System.Reflection.Missing.Value;
        Printer printer = new Printer();
        const string ShopName = "尊享汇";
        const string Address = "西江路鸳江丽港9号楼一层28号（海关正对面，GAGA旁）";
        const string Phone = "2039123";

        static string ConvertToChinese(decimal x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString());
        }

        public MainWindow()
        {
            InitializeComponent();

            //选择打印机
            var pc = new PrinterCollection();
            foreach (Printer p in pc)
            {
                if (p.DeviceName == Properties.Settings.Default.DeviceName)
                {
                    printer = p;
                    break;
                }
            }
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
            Connection.On<Purchase>("printunloadpond", (p) =>
            {
                //卸单石化过磅单
                PrintUnloadPond(p);
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
            Connection.On<Assay>("printassay", (m) =>
            {
                //化验单
                PrintAssay(m);
            });
            Connection.On<ChargeLog>("printcharge", (log) =>
                PrintCharge(log)
            );
            Connection.On<string>("login", (username) => Log.Logs += username + " 已登录，正在执行操作\r");
        }

        private void PrintCharge(ChargeLog log)
        {
            Log.Logs += "正在打印charge：{log.Name}\r";

            printer.Print("———————————");
            printer.Print("充值回执");
            printer.Print("———————————");
            printer.Print($"商户名称：{ShopName}");
            printer.Print($"客户名称：{log.Name}");
            printer.Print($"充值金额：{log.Money}");
            printer.Print($"打印时间:{DateTime.Now}");
            printer.Print("———————————");
            printer.Print($"地址：{Address}");
            printer.Print($"电话：{Phone}");
            printer.Print($"欢迎再次光临");
            printer.EndDoc();
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
            WordReplace(wApp, "#CreatedAt#", cl.CreatedAt.ToString("yyyy-MM-dd HH:mm")); 
            WordReplace(wApp, "#PayType#", strPayType(cl.PayType));
            WordReplace(wApp, "#Balances#", cl.Client.Balances.ToString());
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + cl.Client.Name + ".docx";
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
            WordReplace(wApp, "#CreatedAt#", cl.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            WordReplace(wApp, "#PayType#", strPayType(cl.PayType));
            WordReplace(wApp, "#Balances#", cl.Company.Balances.ToString());
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + cl.Company.Name + ".docx";
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
            WordReplace(wApp, "#OilTemperature#", order.OilTemperature.ToString());
            WordReplace(wApp, "#Worker#", order.Worker);
            WordReplace(wApp, "#LastUpdatedAt#", order.LastUpdatedAt.ToString());
            WordReplace(wApp, "#Salesman#", order.Salesman);
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + order.Name + ".docx";
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
        #region 船舶清污收款单
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
            WordReplace(wApp, "#CreatedAt#", m.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + m.Name + ".docx";
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
        #region 船舶清污完工证
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
            WordReplace(wApp, "#CreatedAt#", m.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            WordReplace(wApp, "#EndTime#", m.EndTime.ToString());
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + m.Name + ".docx";
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
            WordReplace(wApp, "#Payments#", strPayments(order.Payments.ToList()));
            WordReplace(wApp, "#CNMoney#", ConvertToChinese(order.TotalMoney));
            WordReplace(wApp, "#Remark#", order.Remark);
            WordReplace(wApp, "#LastUpdatedBy#", order.Cashier);
            WordReplace(wApp, "#Salesman#", order.Salesman);
            WordReplace(wApp, "#CreatedAt#", order.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            PrintTime(wApp);

            string strInvoice = "";
            if (order.IsInvoice)
            {   
                strInvoice += "开票单位：" + order.BillingCompany + "\r";
                strInvoice += "开票单价：￥" + order.BillingPrice + "\r";
                strInvoice += "开票数量：" + order.BillingCount + order.Unit + "\r";
                //strInvoice += "类型：" + strTicketType(order.TicketType);
            }
            WordReplace(wApp, "#InvoiceContent#", strInvoice);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + order.Name + ".docx";
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
            WordReplace(wApp, "#CreatedAt#", order.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + order.Name + ".docx";
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
        #region 陆上卸油单
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

            WordReplace(wApp, "#Name#", p.Name);
            WordReplace(wApp, "#CarNo#", p.CarNo);
            WordReplace(wApp, "#ProductName#", p.Product?.Name);
            WordReplace(wApp, "#Count#", p.Count.ToString());
            WordReplace(wApp, "#OilCount#", p.OilCount.ToString("0.00"));
            WordReplace(wApp, "#Density#", p.Density.ToString("0.000"));
            WordReplace(wApp, "#ScaleWithCar#", p.ScaleWithCar.ToString());
            WordReplace(wApp, "#Scale#", p.Scale.ToString());

            string strToStore = "";
            foreach(var st in p.ToStoresList)
            {
                strToStore += "\r";
                strToStore += "卸入仓：" + st.Name + "\r";
                strToStore += "卸油前表数：" + st.InstrumentBf + "\r";
                strToStore += "卸油后表数：" + st.InstrumentAf + "\r";
                strToStore += "卸油数量：" + st.Count + "升\r";
            }
            WordReplace(wApp, "#ToStores#", strToStore);

            WordReplace(wApp, "#DiffLitre#", p.DiffLitre.ToString("0.00"));
            WordReplace(wApp, "#DiffTon#", p.DiffTon.ToString("0.00"));
            WordReplace(wApp, "#Worker#", p.Worker);
            WordReplace(wApp, "#StartTime#", p.StartTime.HasValue ? DateTime.Parse(p.StartTime.ToString()).ToString("yyyy-MM-dd HH:mm") : "");
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + p.Name + ".docx";
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
        #region 卸车石化过磅单
        private void PrintUnloadPond(Purchase p)
        {
            Log.Logs += $"正在打印卸车石化过磅单：{p.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder + Properties.Settings.Default.PrintUnloadPondDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据

            WordReplace(wApp, "#Name#", p.Name);
            WordReplace(wApp, "#CarNo#", p.CarNo);
            WordReplace(wApp, "#ScaleWithCar#", p.ScaleWithCar.ToString());
            WordReplace(wApp, "#Scale#", p.Scale.ToString());
            WordReplace(wApp, "#DiffWeight#", p.DiffWeight.ToString());
            WordReplace(wApp, "#Worker#", p.Worker);
            WordReplace(wApp, "#StartTime#", p.StartTime.HasValue ? DateTime.Parse(p.StartTime.ToString()).ToString("yyyy-MM-dd HH:mm"): "");

            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + p.Name + ".docx";
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

        #region 出库石化过磅单
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
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + o.Name + ".docx";
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
        #region 生产转仓单
        /// <summary>
        /// 生产转仓单
        /// </summary>
        /// <param name="m"></param>

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
            WordReplace(wApp, "#Name#", m.Name);
            WordReplace(wApp, "#OutStoreName#", m.OutStoreName);
            WordReplace(wApp, "#OutPlan#", m.OutPlan.ToString("0.00"));
            WordReplace(wApp, "#OutDensity#", m.OutDensity.ToString("0.000"));
            WordReplace(wApp, "#OutTemperature#", m.OutTemperature.ToString());
            WordReplace(wApp, "#OutFact#", m.OutFact.ToString("0.00"));
            WordReplace(wApp, "#InStoreName#", m.InStoreName);
            WordReplace(wApp, "#InDensity#", m.InDensity.ToString("0.000"));
            WordReplace(wApp, "#InTemperature#", m.InTemperature.ToString());
            WordReplace(wApp, "#InFact#", m.InFact.ToString("0.00"));
            WordReplace(wApp, "#Manufacturer#", m.Manufacturer);
            WordReplace(wApp, "#LastUpdatedAt#", m.LastUpdatedAt.ToString("yyyy-MM-dd HH:mm"));
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + m.Name + ".docx";
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
        #region 化验单
        /// <summary>
        /// 化验单
        /// </summary>
        /// <param name="a"></param>
        private void PrintAssay(Assay a)
        {
            Log.Logs += $"正在打印化验单：{a.Name}\r";

            Word.Application thisApplication = new Word.ApplicationClass();
            wApp = thisApplication;
            string tmpDocFile = AppDomain.CurrentDomain.BaseDirectory + folder + Properties.Settings.Default.PrintAssayDocx;
            object templatefile = tmpDocFile;
            wDoc = wApp.Documents.Add(ref templatefile, ref missing, ref missing, ref missing); //在现有进程内打开文档
            wDoc.Activate(); //当前文档置前

            //填充数据
            WordReplace(wApp, "#Name#", a.Name);
            if(a.Purchase != null)
                WordReplace(wApp, "#CarnoOrStorename#", "车号：" + a.Purchase.CarNo);
            if(a.Store != null)
                WordReplace(wApp, "#CarnoOrStorename#", "油仓：" + a.Store.Name);
            WordReplace(wApp, "#视密#", a.视密.ToString("0.00"));
            WordReplace(wApp, "#标密#", a.标密.ToString("0.00"));
            WordReplace(wApp, "#闭口闪点#", a.闭口闪点);
            WordReplace(wApp, "#Temperature#", a.Temperature.ToString("0.00"));
            WordReplace(wApp, "#OilTempTime#", a.OilTempTime.ToLongTimeString());
            WordReplace(wApp, "#SmellType#", strSmellType(a.SmellType));
            WordReplace(wApp, "#混水反应#", a.混水反应);
            WordReplace(wApp, "#十六烷值#", a.十六烷值);
            WordReplace(wApp, "#十六烷指数#", a.十六烷指数);
            WordReplace(wApp, "#初硫#", a.初硫.ToString("0.00"));
            WordReplace(wApp, "#Percentage10#", a.Percentage10.ToString("0.00"));
            WordReplace(wApp, "#Percentage50#", a.Percentage50.ToString("0.00"));
            WordReplace(wApp, "#Percentage90#", a.Percentage90.ToString("0.00"));
            WordReplace(wApp, "#回流#", a.回流.ToString("0.00"));
            WordReplace(wApp, "#干点#", a.干点.ToString("0.00"));
            WordReplace(wApp, "#蚀点#", a.蚀点.ToString("0.00"));
            WordReplace(wApp, "#凝点#", a.凝点.ToString("0.00"));
            WordReplace(wApp, "#含硫#", a.含硫.ToString("0.00"));
            WordReplace(wApp, "#CreatedBy#", a.CreatedBy);
            WordReplace(wApp, "#CreatedAt#", a.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            PrintTime(wApp);

            object background = false; //这个很重要，否则关闭的时候会提示请等待Word打印完毕后再退出，加上这个后可以使Word所有
            object filename = AppDomain.CurrentDomain.BaseDirectory + createdfolder + a.Name + ".docx";
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
            else if (op == OrderPayType.工行刷卡)
                return "工行刷卡";
            else if (op == OrderPayType.桂行刷卡)
                return "桂行刷卡";
            else if (op == OrderPayType.微信)
                return "微信";
            else if (op == OrderPayType.支付宝)
                return "支付宝";
            else
                return "";
        }
        private string strPayments(List<Payment> payments)
        {
            List<string> list = new List<string>();
            foreach(Payment p in payments)
            {
                list.Add( p.Name + "￥" + p.Money);
            }
            return "付款方式：" + string.Join(",", list);
        }
        private string strSmellType(SmellType st)
        {
            switch (st)
            {
                case SmellType.一般刺鼻:
                    return "一般刺鼻";
                case SmellType.不刺鼻:
                    return "不刺鼻";
                case SmellType.刺鼻:
                    return "刺鼻";
            }
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
