using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Trading
{
    public class Balance
    {
        public string ccy;
        public string market;

        public decimal balance;
        public Balance() 
        {
            this.ccy = "";
            this.market = "";
            this.balance = 0;
        }
    }
}
