using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optical.Math
{
    public interface ICalculationManager
    {
        public void Add(ICalculation calculate);
        public void Remove(ICalculation calculate);
        public void Calculate();
        public double? TotalSystemLoss();

    }
}
