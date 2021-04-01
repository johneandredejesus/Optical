using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optical.Math;

namespace Optical
{
    public sealed class Fiber : IFiber 
    {
        private  FiberWaveLength waveLength;

        private  FiberAttenuation fiberAttenuation;

        private double? _inPutPower;

        private double? _outPutPower;

        private double size;

        private readonly ICalculationManager calculationManager;

        private string reference;

        private double? totalLoss;

        private Color color;

        private bool locked;

        public double? InPutPower {  set => this._inPutPower = value; }

        public double? OutPutPower { get => this._outPutPower; }

        public double Size { get => this.size; set => this.size = value; }

        public FiberAttenuation FiberAttenuation => this.fiberAttenuation;

        public FiberWaveLength WaveLength { get => this.waveLength; }

        public string Reference { get => reference; }

        public double? TotalLoss => this.totalLoss;

        public string Name { get; set; }

        public Color Color => this.color;

        public bool Locked => this.locked;

        public Fiber(ICalculationManager calculationManager, FiberWaveLength waveLength=FiberWaveLength._1310, string reference="")
        {
            this.waveLength = waveLength;

            this.size = 0;

            this._outPutPower = null;

            this._inPutPower = null;

            this.fiberAttenuation = new FiberAttenuation(null);

            this.calculationManager = calculationManager;

            this.totalLoss = 0;

            this.color = Color.Blue;

            this.locked = false;

            this.reference = reference;

            this.calculationManager.Add(this);
        }

        public Fiber(FiberWaveLength waveLength = FiberWaveLength._1310, string reference= "")
        {
            this.waveLength = waveLength;

            this.size = 0;

            this._outPutPower = null;

            this._inPutPower = null;

            this.fiberAttenuation = new FiberAttenuation(null);

            this.totalLoss = 0;

            this.color = Color.Blue;

            this.locked = false;

            this.reference = reference;

        }

        public void AddCalculationManager(ICalculationManager calculationManager)
        {
            this.calculationManager.Add(this);
        }
        public Color ChangeColor(FiberColor fiberColor)
        {
            
            switch (fiberColor)
            {
                case FiberColor.BLUE:
                    return this.color = Color.Blue; 
                case FiberColor.ORANGE:
                    return this.color = Color.Orange;
                case FiberColor.GREEN:
                    return this.color = Color.Green;
                case FiberColor.BROWN:
                    return this.color = Color.Brown;
                case FiberColor.GRAY:
                    return this.color = Color.Gray;
                case FiberColor.WHITE:
                    return this.color = Color.White;
                case FiberColor.RED:
                    return this.color = Color.Red;
                case FiberColor.BLACK:
                    return this.color = Color.Black;
                case FiberColor.YELLOW:
                    return this.color = Color.Yellow;
                case FiberColor.VIOLET:
                    return this.color = Color.Violet;
                case FiberColor.PINK:
                    return this.color = Color.Pink;
                case FiberColor.AQUA:
                   return this.color = Color.Aqua;
                default:
                    goto case FiberColor.BLUE;
            }
        }
        public void Calculate() 
        {
            FiberAttenuation fiberAttenuation = GetAttenuation();

            this._outPutPower =  this._inPutPower + fiberAttenuation.Loss;

            this.fiberAttenuation = fiberAttenuation;
        }

        private  FiberAttenuation GetAttenuation()
        {
            if(this.size < 1)
                return new FiberAttenuation(0.00);

            switch (this.waveLength)
            {
                case FiberWaveLength._1310:
                    this.totalLoss = 0.32;
                    return new FiberAttenuation(this.totalLoss * this.size);
                case FiberWaveLength._1383:
                    this.totalLoss = 0.28;
                    return new FiberAttenuation(this.totalLoss * this.size);
                case FiberWaveLength._1490:
                    this.totalLoss = 0.21;
                    return new FiberAttenuation(this.totalLoss * this.size);
                case FiberWaveLength._1550:
                    this.totalLoss = 0.19;
                    return new FiberAttenuation(this.totalLoss * this.size);
                case FiberWaveLength._1625:
                    this.totalLoss = 0.20;
                    return new FiberAttenuation(this.totalLoss * this.size);
                default:
                    goto case FiberWaveLength._1310;
            }
        }
        public void Change(FiberWaveLength waveLength)
        {
            this.waveLength = waveLength;
            this.calculationManager.Calculate();
        }
        public void Dispose()
        {
            this.size = 0;

            this._outPutPower = null;

            this._inPutPower = null;

            this.fiberAttenuation = new FiberAttenuation(null);

            this.calculationManager.Remove(this);

            GC.SuppressFinalize(this);
        }

        public void Lock()
        {
            this.locked = true;
        }

        public void UnLock()
        {
            this.locked = false;
        }
    }


}
