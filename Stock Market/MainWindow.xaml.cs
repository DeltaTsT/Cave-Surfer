using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Stock_Market
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        StockMarket_Lib.cl_StockMarket cl_StockMarket = new StockMarket_Lib.cl_StockMarket();
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            List<string> tickerlist = new List<string> { "AAPL", "GOOG", "AMZN", "FB" };



            await AutomaticallyUpdateStocks();
            //UpdateStocks3();
        }
        public async void UpdateStocks()
        {
            List<StockMarket_Lib.StockPriceModel> _manualReadTagList = new List<StockMarket_Lib.StockPriceModel>();
            StockMarket_Lib.cl_StockMarket cl_StockMarket = new StockMarket_Lib.cl_StockMarket();

            foreach (ListViewItem item in lv_Stock_List.Items)
            {
                //ListViewItem item = (ListViewItem)lv_Stock_List.Items[1];
                //string stock=item.Content.ToString();

                foreach (var r in await cl_StockMarket.GetStockData(item.Content.ToString(), DateTime.Now.AddHours(-6).ToString("MM/dd/yyyy HH:mm"), DateTime.Now.ToString("MM/dd/yyyy HH:mm"), "daily"))
                {
                    _manualReadTagList.Add(r);
                }
            }
            dg_Stocks.ItemsSource = _manualReadTagList;
            txtblk_LastUpdated.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

        }
        public async void UpdateStocks3()
        {
            List<StockMarket_Lib.StockPriceModel> _manualReadTagList = new List<StockMarket_Lib.StockPriceModel>();
            StockMarket_Lib.cl_StockMarket cl_StockMarket = new StockMarket_Lib.cl_StockMarket();
            var tickerlist = Properties.Settings.Default.MyStockList;

            foreach (string ticker in tickerlist)
            {
                foreach (var r in await cl_StockMarket.GetStockData(ticker, DateTime.Now.AddHours(-6).ToString("MM/dd/yyyy HH:mm"), DateTime.Now.ToString("MM/dd/yyyy HH:mm"), "daily"))
                {
                    _manualReadTagList.Add(r);
                }
            }
            dg_Stocks.ItemsSource = _manualReadTagList;
            txtblk_LastUpdated.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

        }


        public async void UpdateStocks2()
        {
            List<StockMarket_Lib.StockPriceModel> _manualReadTagList = new List<StockMarket_Lib.StockPriceModel>();
            StockMarket_Lib.cl_StockMarket cl_StockMarket = new StockMarket_Lib.cl_StockMarket();


            foreach (ListViewItem item in lv_Stock_List.Items)
            {

                foreach (var r in await cl_StockMarket.GetStockData(item.Content.ToString(), DateTime.Now.AddHours(-6).ToString("MM/dd/yyyy HH:mm"), DateTime.Now.ToString("MM/dd/yyyy HH:mm"), "daily"))
                {
                    _manualReadTagList.Add(r);
                }
            }
            //dg_Stocks.ItemsSource = _manualReadTagList;
            //txtblk_LastUpdated.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");


        }
        public List<StockMarket_Lib.StockPriceModel> _manualReadTagList;
        public async Task AutomaticallyUpdateStocks()
        {
            while (true)
            {


                List<StockMarket_Lib.StockPriceModel> _manualReadTagList = new List<StockMarket_Lib.StockPriceModel>();

                List<string> tickerlist = new List<string> { "AAPL", "GOOG", "AMZN", "FB" };

                System.Collections.Specialized.StringCollection SavedSearchTerms = new System.Collections.Specialized.StringCollection();
                SavedSearchTerms = Properties.Settings.Default.MyStockList;
                foreach (string ticker in SavedSearchTerms)
                {
                    foreach (var r in await cl_StockMarket.GetStockData(ticker, DateTime.Now.AddMilliseconds(-2).ToString("MM/dd/yyyy HH:mm"), DateTime.Now.ToString("MM/dd/yyyy HH:mm"), "daily"))
                    {
                        _manualReadTagList.Add(r);
                    }
                }
                await Task.Run(() =>
    {


        Dispatcher.BeginInvoke(new Action(delegate
        {

            txtblk_LastUpdated.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            dg_Stocks.ItemsSource = _manualReadTagList;


        }));
        Thread.Sleep(3000);
    }

                //await Task.Run(() =>
                //{
                //    //Thread.Sleep(5000);

                //    Dispatcher.BeginInvoke(new Action(delegate
                //    {

                //        dg_Stocks.ItemsSource = _manualReadTagList;

                //    }));
                //}
                );
                //await 
                //dg_Stocks.ItemsSource = _manualReadTagList;
                //txtblk_LastUpdated.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            }

        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            await AutomaticallyUpdateStocks();
            //    UpdateStocks2();
            //    //await Status();
            //    var result = await Task.Run(() =>
            //    {
            //        UpdateStocks();

            //        return true;
            //    });
            //}
            //async Task Status()
            //{
            //    await Task.Run(() =>
            //    {

            //        while (true)
            //        {
            //            dg_Stocks.ItemsSource = _manualReadTagList;
            //            txtblk_LastUpdated.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            //        }
            //    }


            //    );
        }

        public class StockPriceModel
        {
            public string Ticker { get; set; }
            public string Date { get; set; }
            public decimal Open { get; set; }
            public decimal High { get; set; }
            public decimal Low { get; set; }
            public decimal Close { get; set; }
            public decimal AdjustedClose { get; set; }
            public decimal Volume { get; set; }
        }
        bool IsItAgoodTicker(string ticker)
        {
            try
            {
                var r = cl_StockMarket.GetStockData(ticker, DateTime.Now.AddHours(-6).ToString("MM/dd/yyyy HH:mm"), DateTime.Now.ToString("MM/dd/yyyy HH:mm"), "daily");
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        private void btn_AddStock_Click(object sender, RoutedEventArgs e)
        {
            if (IsItAgoodTicker(txtbx_Stock.Text) == true)
            {
                Properties.Settings.Default.MyStockList.Add(txtbx_Stock.Text);
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("Wrong symbol! Try again!");
            }
            UpdateStocks3();

        }
    }
}
