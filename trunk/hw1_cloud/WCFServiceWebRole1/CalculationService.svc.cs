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
        public int add(int lOperand, int rOperand)
        {
            return lOperand + rOperand;
        }

        public int sub(int lOperand, int rOperand)
        {
            return lOperand - rOperand;
        }

        public int mul(int lOperand, int rOperand)
        {
            return lOperand * rOperand;
        }

        public int div(int lOperand, int rOperand)
        {
            if (rOperand == 0)
                return 0; //This is our convension for dividing by 0
            return lOperand / rOperand;
        }
    }
}
