using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CalculatorWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICalculator
    {       
        [OperationContract]
        double add(double lOperand, double rOperand);

        [OperationContract]
        double sub(double lOperand, double rOperand);

        [OperationContract]
        double mul(double lOperand, double rOperand);

        [OperationContract]
        double div(double lOperand, double rOperand);
    }
}
