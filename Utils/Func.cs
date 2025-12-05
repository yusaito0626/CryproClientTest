using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class funcContainer : IDisposable
    {
        Action f;

        public funcContainer(Func<Action> starter)
        {
            f = starter();
        }
        public void Dispose()
        {
            var action = Interlocked.Exchange(ref f, null);
            action?.Invoke();
        }
    }
}
