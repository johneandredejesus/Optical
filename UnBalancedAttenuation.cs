using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optical
{
    public class UnBalancedAttenuation
    {
        private readonly double? loss1;

        private readonly double? loss2;
        public UnBalancedAttenuation(double? loss1, double? loss2)
        {
            this.loss1 = loss1;
            this.loss2 = loss2;
        }

        public double? Loss1 => this.loss1;

        public double? Loss2 => this.loss2;
    }

   
}
