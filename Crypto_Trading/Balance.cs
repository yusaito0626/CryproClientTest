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

        public decimal total;
        public decimal available;
        public decimal inuse;
        public Balance() 
        {
            this.ccy = "";
            this.market = "";
            this.total = 0;
            this.available = 0;
            this.inuse = 0;
        }
    }
}
