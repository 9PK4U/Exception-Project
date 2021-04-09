using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dec3
{
    public readonly struct Variables
    {
        public Variables(double _a, double _b, double _c) => (a, b, c) = (_a, _b, _c);

        public readonly double a;
        public readonly double b;
        public readonly double c;

    }
}
