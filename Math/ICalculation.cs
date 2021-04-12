using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optical.Math
{
    public interface ICalculation: IDisposable
    {
        public void Calculate();
        public double? TotalLoss { get; }
    }

}
