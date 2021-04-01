using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optical.Math;
using System.Drawing;

namespace Optical
{
 
    public interface IFiber: ICalculation, IOptical
    {
        public Color Color { get; } 

        public double?  InPutPower { set; }

        public double? OutPutPower { get; }

        public double Size { get; set; }

        public FiberWaveLength WaveLength { get; }

        public string Reference { get; }

        public void Change(FiberWaveLength waveLength);

        public Color ChangeColor(FiberColor fiberColor);

        public void Lock();

        public void UnLock();

        public bool Locked { get; }
      
    }
}
