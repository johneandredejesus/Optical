using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optical.Math
{
    public sealed class CalculationManager : ICalculationManager
    {

        private readonly List<ICalculation> calculates;
        public CalculationManager()
        {
            this.calculates = new List<ICalculation>();
        }
        public void Add(ICalculation calculate)
        {
            this.calculates.Add(calculate);
        }
        public void Remove(ICalculation calculate)
        {
            this.calculates.Remove(calculate);
        }
        public void Calculate()
        {
            foreach (ICalculation calculate in this.calculates)
                calculate.Calculate();
        }
        public double? TotalSystemLoss()
        {
            double? total = 0;
            foreach(ICalculation calculation in calculates)
                total += calculation.TotalLoss;
            return total;
        }
    }
}
