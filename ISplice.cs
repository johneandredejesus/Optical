using Optical.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optical
{
    public interface ISplice : ICalculation, IOptical, IPassive
    {
        public IFiber InPutFiber { get; }

        public IFiber OutPutFiber { get; }

        public bool LockedInput { get; }

        public bool LockedOutput { get; }

        public void AddInPutFiber(IFiber fiber);

        public void RemoveInPutFiber();

        public void Change(SpliceType spliceType);

        public bool HasPower();
    }
}
