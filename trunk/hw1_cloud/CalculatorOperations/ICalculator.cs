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
        int add(int lOperand, int rOperand);

        [OperationContract]
        int sub(int lOperand, int rOperand);

        [OperationContract]
        int mul(int lOperand, int rOperand);

        [OperationContract]
        int div(int lOperand, int rOperand);
    }
}
