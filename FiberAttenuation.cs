using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optical
{
    public class FiberAttenuation
    {
        private readonly double? loss;

        public FiberAttenuation(double? loss)
        {
            this.loss = loss * (-1);
        }

        public double? Loss => this.loss;
    }
}
