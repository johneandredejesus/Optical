using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optical.Math;

namespace Optical
{
    public sealed class BalancedSplitter : ISplitter
    {

        private  BalancedAttenuation attenuation;

        private  List<IFiber> outPutFiber;

        private IFiber inPutFiber;

        private  ICalculationManager calculationManager;

        private readonly BalancedType balanced;

        private double? totalLoss;
       
        public BalancedType Balanced => this.balanced;

        public IFiber InPutFiber { get { VerifyCalculationManager(); return this.inPutFiber; } } 

        public List<IFiber> OutPutFiber { get { VerifyCalculationManager(); return this.outPutFiber; } }

        public BalancedAttenuation Attenuation => this.attenuation;

        public double? TotalLoss => this.totalLoss;

        public string Name { get; set; }

        public IFiber CurrentInPutFiber { get { VerifyCalculationManager(); return this.inPutFiber; } }

        public bool LockedInput { get { VerifyCalculationManager(); return this.inPutFiber is not null; } }

        public bool LockedOutput { get { VerifyCalculationManager(); return this.outPutFiber.Count(fiber => fiber.Locked == true) == this.outPutFiber.Count; } }

        public BalancedSplitter(BalancedType balanced, ICalculationManager calculationManager)
        {
            this.balanced = balanced;

            this.calculationManager = calculationManager;

            this.inPutFiber = null;

            this.attenuation = new BalancedAttenuation(null);

            this.outPutFiber = this.OutPuts();

            this.totalLoss = 0;

            this.calculationManager.Add(this);
        }
        public BalancedSplitter(BalancedType balanced)
        {
            this.balanced = balanced;

            this.inPutFiber = null;

            this.attenuation = new BalancedAttenuation(null);

            this.totalLoss = 0;

        }
        public void AddCalculationManager(ICalculationManager calculationManager)
        {

            if(this.outPutFiber is null  && this.calculationManager is null)
            {
                this.calculationManager = calculationManager;

                this.outPutFiber = this.OutPuts();

                this.calculationManager.Add(this);
            }
          
        }
        public void AddInputFiber(IFiber fiber)
        {
            VerifyCalculationManager();

            this.inPutFiber = fiber;

            this.inPutFiber.Lock();

            this.calculationManager.Calculate();
        }
        public void RemoveInPutFiber()
        {
            VerifyCalculationManager();

            this.inPutFiber.UnLock();

            this.inPutFiber = null;

            this.calculationManager.Calculate();
        }
        public bool HasPower()
        {
            return this.inPutFiber == null ? false : true;
        }
        public void Calculate() 
        {
            VerifyCalculationManager();

            if(this.inPutFiber is not null)
                this.attenuation = GetAttenuation(this.inPutFiber.OutPutPower);
           
            for (int count=0; count < this.outPutFiber.Count; count++)
            {
                if (this.inPutFiber == null)
                    this.outPutFiber[count].InPutPower = null;
                else
                    this.outPutFiber[count].InPutPower = attenuation.Loss;
          
                this.outPutFiber[count].Calculate();
            }
        }
        private  BalancedAttenuation GetAttenuation(double? value)
        {
            switch (this.balanced)
            {
                case BalancedType._1_TO_2:
                    this.totalLoss = 3.844;
                    return new BalancedAttenuation((value - this.totalLoss));
                case BalancedType._1_TO_4:
                    this.totalLoss = 7.377;
                    return new BalancedAttenuation((value - this.totalLoss));
                case BalancedType._1_TO_8:
                    this.totalLoss = 10.783;
                    return new BalancedAttenuation((value - this.totalLoss));
                case BalancedType._1_TO_16:
                    this.totalLoss = 14.075;
                    return new BalancedAttenuation((value - this.totalLoss));
                case BalancedType._1_TO_32:
                    this.totalLoss = 17.521;
                    return new BalancedAttenuation((value - this.totalLoss));
                case BalancedType._1_TO_64:
                    this.totalLoss = 20.900;
                    return new BalancedAttenuation((value - this.totalLoss));
                case BalancedType._1_TO_128:
                    this.totalLoss = 24.000;
                    return new BalancedAttenuation((value - this.totalLoss));
                default:
                    goto case BalancedType._1_TO_2;
            }
        }
        private  int CalculateNumberOfOutPuts()
        {
            switch (this.balanced)
            {
                case BalancedType._1_TO_2:
                    return 2;
                case BalancedType._1_TO_4:
                    return 4;
                case BalancedType._1_TO_8:
                    return 8;
                case BalancedType._1_TO_16:
                    return 16;
                case BalancedType._1_TO_32:
                    return 32;
                case BalancedType._1_TO_64:
                    return 64;
                case BalancedType._1_TO_128:
                    return 128;
                default:
                    goto case BalancedType._1_TO_2;
            }
        }
        private List<IFiber> OutPuts()
        {
            List<IFiber> fibers = new List<IFiber>();
            
            for (int count = 0; count < CalculateNumberOfOutPuts(); count++)
            {
                int value = count;
                string reference = (value + 1).ToString();
                fibers.Add(new Fiber(this.calculationManager, reference: reference));
            }
           
            return fibers;
        }
        private void VerifyCalculationManager()
        {
            if (this.calculationManager is null)
                throw new ArgumentNullException("CalculationManager deve ser atribuído.");

        }
        public void Dispose()
        {
            this.inPutFiber = null;

            this.totalLoss = 0;

            foreach (IFiber fiber in this.outPutFiber)
                fiber.Dispose();

            this.outPutFiber.Clear();

            this.calculationManager.Calculate();

            this.calculationManager.Remove(this);

            GC.SuppressFinalize(this);

        }
    }
}
