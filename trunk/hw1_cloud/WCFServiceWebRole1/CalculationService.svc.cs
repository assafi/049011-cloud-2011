using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CalculatorWS
{   
    public class CalculatorService : ICalculator
    {
        public double add(double lOperand, double rOperand)
        {
            return lOperand + rOperand;
        }

        public double sub(double lOperand, double rOperand)
        {
            return lOperand - rOperand;
        }

        public double mul(double lOperand, double rOperand)
        {
            return lOperand * rOperand;
        }

        public double div(double lOperand, double rOperand)
        {
            if (double.Equals(rOperand,0))
                return 0; //This is our convension for dividing by 0
            return lOperand / rOperand;
        }
    }
}
