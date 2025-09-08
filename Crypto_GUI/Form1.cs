using Crypto_Clients;
using Crypto_Trading;
using CryptoClients.Net.Enums;
using System.Net;
using System.Threading;

namespace Crypto_GUI
{
    public partial class Form1 : Form
    {
        Crypto_Clients.Crypto_Clients cl = new Crypto_Clients.Crypto_Clients();
        private Dictionary<string, Instrument> instruments;

        string baseCcy = "BTC";
        string quoteCcy = "USDT";

        System.Threading.Thread updatingTh;
        public Form1()
        {
            InitializeComponent();
            this.instruments = new Dictionary<string, Instrument>();

            Instrument ins = new Instrument();
            ins.setSymbolMarket(baseCcy + quoteCcy, Exchange.Bybit);
            ins.ToBsize = 10;
            instruments[ins.symbol_market] = ins;
            ins = new Instrument();
            ins.setSymbolMarket(baseCcy + "-" + quoteCcy, Exchange.Coinbase);
            ins.ToBsize = 10;

            updatingTh = new Thread(update);
            updatingTh.Start();
        }

        private void updatetext2(string str)
        {
            this.labeltest2.Text = str;
        }
        private void updatetext1(string str)
        {
            this.labeltest1.Text = str;
        }

        private void updatetext3(string str)
        {
            this.labeltest3.Text = str;
        }
        private void update()
        {
            string filepath = "C:\\Users\\yusai\\log.csv";
            string symbol_market = "";
            Instrument ins;
            using (FileStream f = new FileStream(filepath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter s = new StreamWriter(f))
                {
                    while (true)
                    {
                        DataOrderBook msg;
                        string strMsg;
                        string bestbidask;
                        while (cl.ordBookQueue.TryDequeue(out msg))
                        {
                            symbol_market = msg.symbol.ToUpper() + "@" + msg.market;
                            if (instruments.ContainsKey(symbol_market))
                            {
                                ins = instruments[symbol_market];
                                ins.updateQuotes(msg);
                                strMsg = ins.ToString("Quote5");
                                if (ins.market == Exchange.Bybit)
                                {
                                    this.BeginInvoke(updatetext2, strMsg);
                                    bestbidask = "Ask:" + ins.adjusted_bestask.Item1.ToString("N2") + " Bid:" + ins.adjusted_bestbid.Item1.ToString("N2") + "\n";
                                    bestbidask += "Spread:" + (ins.adjusted_bestask.Item1 - ins.adjusted_bestbid.Item1).ToString("N2");
                                    this.BeginInvoke(updatetext3, bestbidask);
                                }
                                else if (ins.market == Exchange.Coinbase)
                                {
                                    this.BeginInvoke(updatetext1, strMsg);
                                }
                            }
                            else
                            {
                                strMsg = "[ERROR] The symbol market did not found. " + symbol_market;
                            }
                            Console.WriteLine(strMsg);
                            s.WriteLine(strMsg);
                            //Console.WriteLine(msg.ToString());
                            //s.WriteLine(msg.ToString());
                            cl.pushToOrderBookStack(msg);
                        }
                        while (cl.strQueue.TryDequeue(out strMsg))
                        {
                            Console.WriteLine(strMsg);
                            s.WriteLine(strMsg);
                        }
                    }
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            cl.readCredentials(Exchange.Coinbase, "C:\\Users\\yusai\\coinbase_viewonly.json");
            cl.readCredentials(Exchange.Bybit, "C:\\Users\\yusai\\bybit_viewonly.json");
            await cl.subscribeCoinbaseOrderBook(baseCcy, quoteCcy);
            await cl.subscribeBybitOrderBook(baseCcy, quoteCcy);
            //await cl.getBalance(markets);
            //Console.WriteLine("Done");
            
        }
    }
}
