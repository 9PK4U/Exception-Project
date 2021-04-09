using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desc1
{
    public readonly struct Roots
    {
        public Roots(double _x1, double _x2) => (x1, x2, countRoots) = (_x1, _x2, 2);
        public Roots(double _x1) => (x1, x2, countRoots) = (_x1, default, 1);
        public Roots(int _countRoots) => (x1, x2, countRoots) = (default, default, _countRoots);

        public readonly double x1;
        public readonly double x2;
        public readonly int countRoots;
    }
}
