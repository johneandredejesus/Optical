using Optical.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optical
{
    public sealed class Splice : ISplice
    {
        public IFiber InPutFiber => this.inPutFiber;

        public IFiber OutPutFiber => this.outPutFiber;

        public double? TotalLoss => this.totalLoss;

        private IFiber inPutFiber;

        private IFiber outPutFiber;

        private SpliceType spliceType;

        private ICalculationManager calculationManager;

        public SpliceAttenuation Attenuation { get { return this.attenuation; } }

        private double? totalLoss;

        private SpliceAttenuation attenuation;

        public string Name { get; set; }

        public IFiber CurrentInPutFiber { get { return this.inPutFiber; } }

        public bool LockedInput { get { return this.inPutFiber is not null; } }

        public bool LockedOutput { get { return this.outPutFiber.Locked; } }

        public Splice(SpliceType spliceType, ICalculationManager calculationManager)
        {
            this.spliceType = spliceType;

            this.calculationManager = calculationManager;

            this.outPutFiber = OutPuts();

            this.calculationManager.Add(this);

        }

        public void AddInPutFiber(IFiber fiber)
        {

            this.inPutFiber = fiber;

            this.inPutFiber.Lock();

            this.calculationManager.Calculate();
        }

        public void RemoveInPutFiber()
        {
            this.inPutFiber.UnLock();

            this.inPutFiber = null;

            this.calculationManager.Calculate();
        }

        public void Calculate()
        {
            if (this.inPutFiber == null)
                this.outPutFiber.InPutPower = null;
            else
            {
                this.attenuation = GetAttenuation(this.inPutFiber.OutPutPower);
                this.outPutFiber.InPutPower = attenuation.Loss;
            }
                
            this.outPutFiber.Calculate();
        }

        public bool HasPower()
        {
            return this.inPutFiber == null ? false : true;
        }

        private SpliceAttenuation GetAttenuation(double? value)
        {
            switch (this.spliceType)
            {
                case SpliceType.FUSION:
                    this.totalLoss = 0.10;
                    return new SpliceAttenuation((value - this.totalLoss));
                case SpliceType.MECHANIC:
                    this.totalLoss = 0.30;
                    return new SpliceAttenuation((value - this.totalLoss));
                case SpliceType.CONNECTOR:
                    this.totalLoss = 0.75;
                    return new SpliceAttenuation((value - this.totalLoss));
                default:
                    goto case SpliceType.FUSION;
            }
        }
        private IFiber OutPuts()
        {
            return new Fiber(this.calculationManager, reference: "1");
        }
        public void Change(SpliceType spliceType)
        {

            this.spliceType = spliceType;

            this.calculationManager.Calculate();
        }

   
        public void Dispose()
        {
            this.inPutFiber = null;

            this.totalLoss = 0;

            this.calculationManager.Remove(this.outPutFiber);

            this.outPutFiber = null;

            this.calculationManager.Calculate();

            this.calculationManager.Remove(this);

            GC.SuppressFinalize(this);
        }
    }
}
