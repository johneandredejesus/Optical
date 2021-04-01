using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optical.Math;

namespace Optical
{
    public interface ISplitter: ICalculation, IOptical, IPassive
    {
        public IFiber InPutFiber { get; }

        public List<IFiber> OutPutFiber { get; }

        public bool LockedInput { get; }

        public bool LockedOutput { get; }

        public void AddInputFiber(IFiber fiber);

        public void RemoveInPutFiber();

        public bool HasPower();
    }

}
