using Optical.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optical
{
    public sealed class OLT : IOLT
    {

        public List<IFiber> OutPutFiber { get => this.outPutFiber; }

        public double? Power { get => this.power; }

        public double? TotalLoss { get => 0; }

        private int numberPorts;

        private ICalculationManager calculationManager;

        private List<IFiber> outPutFiber;

        private double? power;

        public string Name { get; set; }

        public bool LockedOutput => this.outPutFiber.Count(fiber => fiber.Locked == true) == this.outPutFiber.Count;

        public OLT(double? power, ICalculationManager calculationManager, int numberPorts = 2)
        {
            if (power <= 0)
                throw new Exception("O valor de power deve ser maior ou igual a 1.");

            this.power = power;

            this.calculationManager = calculationManager;

            this.numberPorts = numberPorts;

            this.outPutFiber = this.OutPuts();

            calculationManager.Add(this);
        }

        public OLT(double? power, int numberPorts = 2)
        {
            if (power <= 0)
                throw new Exception("O valor de power deve ser maior ou igual a 1.");

            this.power = power;

            this.numberPorts = numberPorts;
        }
        public void AddCalculationManager(ICalculationManager calculationManager)
        {
            this.calculationManager = calculationManager;
            this.outPutFiber = this.OutPuts();
            this.calculationManager.Add(this);
        }

        public void ChangePower(double? power)
        {
            if (power <= 0)
                throw new Exception("O valor de power deve ser maior ou igual a 1.");
           
            this.power = power;

            this.calculationManager.Calculate();
        }
        public void Calculate()
        {
            for (int count = 0; count < this.outPutFiber.Count; count++)
            {
                this.outPutFiber[count].InPutPower = this.power;
                this.outPutFiber[count].Calculate();
            }
        }
        private List<IFiber> OutPuts()
        {
            List<IFiber> fibers = new List<IFiber>();

            for (int count = 0; count < this.numberPorts; count++)
            {
                int value = count;
                string reference = (value + 1).ToString();
                fibers.Add(new Fiber(this.calculationManager, reference: reference) { InPutPower = this.power });
            }
              
            this.calculationManager.Calculate();

            return fibers;
        }
        public void Dispose()
        {

            this.power = null;

            foreach (IFiber fiber in this.outPutFiber)
                this.calculationManager.Remove(fiber);

            this.outPutFiber.Clear();

            this.calculationManager.Calculate();

            this.calculationManager.Remove(this);

            GC.SuppressFinalize(this);
        }
    }
}
