using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Templates.Entities
{
    internal struct RGBMask
    {
        public double Red { get; }
        public double Green { get; }
        public double Blue { get; }

        public RGBMask(double red, double green, double blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}
