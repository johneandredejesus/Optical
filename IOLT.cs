using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optical.Math;

namespace Optical
{
    public interface IOLT : ICalculation, IOptical
    {
        public List<IFiber> OutPutFiber { get; }

        public bool LockedOutput { get; }

        public double? Power { get; }

        public void ChangePower(double? value);
    }
}
