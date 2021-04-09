using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dec3
{
    class RootsException : Exception
    {
        public RootsException(string massage) : base(massage)
        { }
        public RootsException(string message, Roots roots) : base(message)
        {
            Roots = roots;
        }
        public Roots Roots { get; set; }
    }
}
