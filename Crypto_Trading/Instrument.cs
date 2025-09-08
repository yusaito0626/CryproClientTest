using System.Threading;

using Crypto_Clients;
using CryptoExchange.Net.Objects.Options;

namespace Crypto_Trading
{
    public class Instrument
    {
        public string symbol;
        public string market;
        public string symbol_market;
        public string master_symbol;


        public volatile int quotes_lock;
        public SortedDictionary<decimal, decimal> asks;
        public SortedDictionary<decimal, decimal> bids;

        //Amount Weighted Best Ask/Bid +/- fee
        public ValueTuple<decimal, decimal> adjusted_bestask;
        public ValueTuple<decimal, decimal> adjusted_bestbid;

        public decimal taker_fee;
        public decimal maker_fee;

        public decimal ToBsize;

        public Instrument()
        {
            this.symbol = "";
            this.market = "";
            this.symbol_market = "";
            this.master_symbol = "";

            this.quotes_lock = 0;
            this.asks = new SortedDictionary<decimal, decimal>();
            this.bids = new SortedDictionary<decimal, decimal>();

            //Amount Weighted Best Ask/Bid +/- fee
            this.adjusted_bestask = new ValueTuple<decimal, decimal>(0,0);
            this.adjusted_bestbid = new ValueTuple<decimal, decimal>(0,0);

            this.taker_fee = 0;
            this.maker_fee = 0;
            this.ToBsize = 0;
        }

        public void setSymbolMarket(string symbol, string market)
        {
            this.symbol = symbol;
            this.market = market;
            this.symbol_market = symbol + "@" + market;
        }

        public void updateQuotes(Crypto_Clients.DataOrderBook update)
        {
            int desired = 1;
            int expected = 0;
            switch (update.updateType)
            {
                case CryptoExchange.Net.Objects.SocketUpdateType.Snapshot:

                    while(Interlocked.CompareExchange(ref this.quotes_lock,1,0) != 0)
                    {

                    }
                    this.asks.Clear();
                    foreach(var item in update.asks)
                    {
                        this.asks[item.Key] = item.Value;
                    }
                    this.bids.Clear();
                    foreach (var item in update.bids)
                    {
                        this.bids[item.Key] = item.Value;
                    }
                    Volatile.Write(ref this.quotes_lock, 0);
                    break;
                case CryptoExchange.Net.Objects.SocketUpdateType.Update:

                    while (Interlocked.CompareExchange(ref this.quotes_lock, 1, 0) != 0)
                    {

                    }
                    foreach (var item in update.asks)
                    {
                        if (item.Value == 0)
                        {
                            this.asks.Remove(item.Key);
                        }
                        else
                        {
                            this.asks[item.Key] = item.Value;
                        }
                    }
                    foreach (var item in update.bids)
                    {
                        if (item.Value == 0)
                        {
                            this.bids.Remove(item.Key);
                        }
                        else
                        {
                            this.bids[item.Key] = item.Value;
                        }
                    }
                    Volatile.Write(ref this.quotes_lock, 0);
                    break;
            }
            this.updateBeskAskBid(this.ToBsize);
        }

        public void updateBeskAskBid(decimal quantity)
        {
            if(quantity > 0)
            {
                decimal cumQuantity = 0;
                decimal weightedPrice = 0;
                foreach(var item in this.asks)
                {
                    if(cumQuantity + item.Value < quantity)
                    {
                        cumQuantity += item.Value;
                        weightedPrice += item.Value * item.Key;
                    }
                    else
                    {
                        cumQuantity += (quantity - cumQuantity);
                        weightedPrice += (quantity - cumQuantity) * item.Key;
                        break;
                    }
                }
                weightedPrice /= cumQuantity;
                this.adjusted_bestask.Item1 = weightedPrice;
                this.adjusted_bestask.Item2 = cumQuantity;

                cumQuantity = 0;
                weightedPrice = 0;
                foreach (var item in this.bids.Reverse())
                {
                    if (cumQuantity + item.Value < quantity)
                    {
                        cumQuantity += item.Value;
                        weightedPrice += item.Value * item.Key;
                    }
                    else
                    {
                        cumQuantity += (quantity - cumQuantity);
                        weightedPrice += (quantity - cumQuantity) * item.Key;
                        break;
                    }
                }
                weightedPrice /= cumQuantity;
                this.adjusted_bestbid.Item1 = weightedPrice;
                this.adjusted_bestbid.Item2 = cumQuantity;
            }
            else
            {
                this.adjusted_bestask.Item1 = this.asks.First().Key * (1 + this.taker_fee);
                this.adjusted_bestask.Item2 = this.asks.First().Value;
                this.adjusted_bestbid.Item1 = this.bids.Last().Key * (1 - this.taker_fee);
                this.adjusted_bestbid.Item2 = this.bids.Last().Value;
            }
        }

        public string ToString(string content = "")
        {
            string output = this.symbol_market + " " + content + "\n";
            if(content == "Quote5")
            {
                int i = 4;
                int priceStrLength = 0;
                if (i >= this.asks.Count)
                {
                    i = this.asks.Count - 1;
                }
                while (i >= 0)
                {
                    string temp = "";
                    temp += this.asks.ElementAt(i).Value.ToString("N5").PadLeft(10) + " | ";
                    string strPrice = this.asks.ElementAt(i).Key.ToString("N2");
                    if (i == 4)
                    {
                        priceStrLength = strPrice.Length;
                        if(priceStrLength < 7)
                        {
                            priceStrLength = 7;
                        }
                    }
                    else
                    {
                        strPrice = strPrice.PadLeft(priceStrLength);
                    }
                    temp += strPrice + " | ";
                    temp = temp.PadRight(temp.Length + 10);
                    output += temp + "\n";
                    --i;
                }
                i = 0;
                foreach(var item in this.bids.Reverse())
                {
                    if(i > 4)
                    {
                        break;
                    }
                    else
                    {
                        string temp = "".PadLeft(10) + " | ";
                        string strPrice = item.Key.ToString("N2").PadLeft(priceStrLength);
                        temp += strPrice + " | ";
                        temp += item.Value.ToString("N5").PadRight(10);
                        output += temp + "\n";
                    }
                        ++i;
                }
            }
            return output;
        }
    }
}
