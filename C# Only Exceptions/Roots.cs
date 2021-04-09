using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dec3
{
    public readonly struct Roots
    {
        public Roots(double _x1, double _x2) => (x1, x2) = (_x1, _x2);
        public Roots(double _x1) => (x1, x2) = (_x1, default);

        public readonly double x1;
        public readonly double x2;
    }
}
