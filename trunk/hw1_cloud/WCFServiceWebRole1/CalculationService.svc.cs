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
    }
}
