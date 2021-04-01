using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optical.Math;

namespace Optical
{
    public sealed class UnBalancedSplitter : ISplitter
    {
        private  UnBalancedAttenuation attenuation;

        private  List<IFiber> outPutFiber;

        private  IFiber inPutFiber;

        private  ICalculationManager calculationManager;

        private readonly UnBalancedType unbalanced;

        private double? totalLoss1;

        private double? totalLoss2;
        
        public UnBalancedType Unbalanced => this.unbalanced;

        public IFiber InPutFiber { get { VerifyCalculationManager(); return this.inPutFiber; } }

        public List<IFiber> OutPutFiber { get { VerifyCalculationManager(); return this.outPutFiber; } }

        public UnBalancedAttenuation Attenuation => this.attenuation;

        public double? TotalLoss => (this.totalLoss1 + this.totalLoss2);

        public string Name { get; set; }

        public IFiber CurrentInPutFiber { get { VerifyCalculationManager(); return this.inPutFiber; } }

        public bool LockedInput { get { VerifyCalculationManager(); return this.inPutFiber is not null; } }

        public bool LockedOutput { get { VerifyCalculationManager(); return this.outPutFiber.Count(fiber => fiber.Locked == true) == this.outPutFiber.Count; } }

        public UnBalancedSplitter(UnBalancedType unbalanced, ICalculationManager calculationManager)
        {
            this.unbalanced = unbalanced;

            this.calculationManager = calculationManager;

            this.inPutFiber = null;

            this.attenuation = new UnBalancedAttenuation(null, null);

            this.outPutFiber = this.OutPuts();

            this.totalLoss1 = 0;

            this.totalLoss2 = 0;

            this.calculationManager.Add(this);
        }

        public UnBalancedSplitter(UnBalancedType unbalanced)
        {
            this.unbalanced = unbalanced;

            this.inPutFiber = null;

            this.attenuation = new UnBalancedAttenuation(null, null);

            this.totalLoss1 = 0;

            this.totalLoss2 = 0;

        }

        public void AddCalculationManager(ICalculationManager calculationManager)
        {
            if (this.outPutFiber is null && this.calculationManager is null)
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
        private  Dictionary<string, string> GetReference()
        {
            Dictionary<string, string> references = new Dictionary<string, string>();
             
            switch (this.unbalanced)
            {
                case UnBalancedType._50X50:
                    references.Add("Loss1", "50%");
                    references.Add("Loss2", "50%");
                    return references;
                case UnBalancedType._40X60:
                    references.Add("Loss1", "40%");
                    references.Add("Loss2", "60%");
                    return references;
                case UnBalancedType._30X70:
                    references.Add("Loss1", "30%");
                    references.Add("Loss2", "70%");
                    return references;
                case UnBalancedType._20X80:
                    references.Add("Loss1", "20%");
                    references.Add("Loss2", "80%");
                    return references;
                case UnBalancedType._15X85:
                    references.Add("Loss1", "15%");
                    references.Add("Loss2", "85%");
                    return references;
                case UnBalancedType._10X90:
                    references.Add("Loss1", "10%");
                    references.Add("Loss2", "90%");
                    return references;
                case UnBalancedType._5X95:
                    references.Add("Loss1", "5%");
                    references.Add("Loss2", "95%");
                    return references;
                case UnBalancedType._2X98:
                    references.Add("Loss1", "2%");
                    references.Add("Loss2", "98%");
                    return references;
                case UnBalancedType._1X99:
                    references.Add("Loss1", "1%");
                    references.Add("Loss2", "99%");
                    return references;
                default:
                    goto case UnBalancedType._50X50;
            }
        }
        public bool HasPower()
        {
            return this.inPutFiber == null ? false : true;
        }
        public void Calculate()
        {
            VerifyCalculationManager();
            if (this.inPutFiber == null)
            {
                this.outPutFiber[0].InPutPower = null;
                this.outPutFiber[1].InPutPower = null;
            }
            else
            {
                this.attenuation = GetAttenuation(this.inPutFiber.OutPutPower);
                this.outPutFiber[0].InPutPower = attenuation.Loss1;
                this.outPutFiber[1].InPutPower = attenuation.Loss2;
            }
            this.outPutFiber[0].Calculate();
            this.outPutFiber[1].Calculate();
        }
        private  UnBalancedAttenuation GetAttenuation(double? value)
        {
            double? result = value / 100;
            
            switch (this.unbalanced)
            {
                case UnBalancedType._50X50:
                    this.totalLoss1 = (result * 50);
                    this.totalLoss2 = (result * 50);
                    return new UnBalancedAttenuation((value - totalLoss1), (value - totalLoss2));
                case UnBalancedType._40X60:
                    this.totalLoss1 = (result * 40);
                    this.totalLoss2 = (result * 60);
                    return new UnBalancedAttenuation((value - totalLoss1), (value - totalLoss1));
                case UnBalancedType._30X70:
                    this.totalLoss1 = (result * 30);
                    this.totalLoss2 = (result * 70);
                    return new UnBalancedAttenuation((value - totalLoss1), (value - totalLoss2));
                case UnBalancedType._20X80:
                    this.totalLoss1 = (result * 20);
                    this.totalLoss2 = (result * 80);
                    return new UnBalancedAttenuation((value - totalLoss1), (value - totalLoss2));
                case UnBalancedType._15X85:
                    this.totalLoss1 = (result * 15);
                    this.totalLoss2 = (result * 85);
                    return new UnBalancedAttenuation((value - totalLoss1), (value - totalLoss2));
                case UnBalancedType._10X90:
                    this.totalLoss1 = (result * 10);
                    this.totalLoss2 = (result * 90);
                    return new UnBalancedAttenuation((value - totalLoss1), (value - totalLoss2));
                case UnBalancedType._5X95:
                    this.totalLoss1 = (result * 5);
                    this.totalLoss2 = (result * 95);
                    return new UnBalancedAttenuation((value - totalLoss1), (value - totalLoss2));
                case UnBalancedType._2X98:
                    this.totalLoss1 = (result * 2);
                    this.totalLoss2 = (result * 98);
                    return new UnBalancedAttenuation((value - totalLoss1), (value - totalLoss2));
                case UnBalancedType._1X99:
                    this.totalLoss1 = (result * 1);
                    this.totalLoss2 = (result * 99);
                    return new UnBalancedAttenuation((value - totalLoss1), (value - totalLoss2));
                default:
                    goto case UnBalancedType._50X50;
            }
        }
        private int CalculateNumberOfOutPuts()
        {
            switch (this.unbalanced)
            {
                case UnBalancedType._50X50:
                    return 2;
                case UnBalancedType._40X60:
                    return 2;
                case UnBalancedType._30X70:
                    return 2;
                case UnBalancedType._20X80:
                    return 2;
                case UnBalancedType._15X85:
                    return 2;
                case UnBalancedType._10X90:
                    return 2;
                case UnBalancedType._5X95:
                    return 2;
                case UnBalancedType._2X98:
                    return 2;
                case UnBalancedType._1X99:
                    return 2;
                default:
                    goto case UnBalancedType._50X50;
            }
        }
        private List<IFiber> OutPuts()
        {
            List<IFiber> fibers = new List<IFiber>();
            var reference = GetReference();

            for (int count = 0; count < CalculateNumberOfOutPuts(); count++)
            {
                if (count % 2 == 0)
                    fibers.Add(new Fiber(this.calculationManager,reference: string.Concat(count, ": ", reference["Loss1"])));
                else
                    fibers.Add(new Fiber(this.calculationManager, reference: string.Concat(count, ": ", reference["Loss2"])));
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

            this.totalLoss1 = 0;

            this.totalLoss2 = 0;

            foreach (IFiber fiber in this.outPutFiber)
                fiber.Dispose();

            this.outPutFiber.Clear();

            this.calculationManager.Calculate();

            this.calculationManager.Remove(this);

            GC.SuppressFinalize(this);
        }

    }


}
