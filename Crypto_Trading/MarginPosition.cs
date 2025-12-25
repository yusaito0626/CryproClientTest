using CryptoExchange.Net.Converters.SystemTextJson;
using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Crypto_Trading
{
    public class MarginPosition
    {
        public string symbol;
        public string market;
        public string symbolmarket
        {
            get { return symbol + "@" + market; }
        }
        public orderSide side;//Buy:long Sell:short
        public decimal quantity;
        public decimal avgPrice;
        public decimal unrealizedFee;
        public decimal unrealizedInterest;

        public MarginPosition()
        {
            this.symbol = "";
            this.market = "";
            this.side = orderSide.NONE;
            this.quantity = 0;
            this.avgPrice = 0;
            this.unrealizedFee = 0;
            this.unrealizedInterest = 0;
        }
        public void setBitbankJson(JsonElement js)
        {
            this.symbol = js.GetProperty("pair").GetString();
            this.market = "bitbank";
            string str_side = js.GetProperty("position_side").GetString();
            if(str_side == "long")
            {
                this.side = orderSide.Buy;
            }
            else if(str_side == "short")
            {
                this.side = orderSide.Sell;
            }
            this.quantity = decimal.Parse(js.GetProperty("open_amount").GetString());
            this.avgPrice = decimal.Parse(js.GetProperty("average_price").GetString());
            this.unrealizedFee = decimal.Parse(js.GetProperty("unrealized_fee_amount").GetString());
            this.unrealizedInterest = decimal.Parse(js.GetProperty("unrealized_interest_amount").GetString());
        }
        public string ToString()
        {
            string res = this.symbol + "," + this.market + "," + this.side.ToString() + "," + this.quantity.ToString() + "," + this.avgPrice.ToString() + "," + this.unrealizedFee.ToString() + "," + this.unrealizedInterest.ToString();
            return res;
        }
        public void init()
        {
            this.symbol = "";
            this.market = "";
            this.side = orderSide.NONE;
            this.quantity = 0;
            this.avgPrice = 0;
            this.unrealizedFee = 0;
            this.unrealizedInterest = 0;
        }
    }
}
