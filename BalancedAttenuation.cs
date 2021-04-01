using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optical
{
    public sealed class BalancedAttenuation
    {
        private readonly double? loss;

        public BalancedAttenuation(double? loss)
        {
            this.loss = loss;
        }

        public double? Loss => this.loss;

    }
}
