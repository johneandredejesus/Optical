using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optical
{
    public sealed class SpliceAttenuation
    {
        private readonly double? loss;

        public SpliceAttenuation(double? loss)
        {
            this.loss = loss;
        }

        public double? Loss => this.loss;

    }
}
