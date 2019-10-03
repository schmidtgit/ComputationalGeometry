using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimprExpression {
    public interface IOperator {
		void Setup(IExpression first, IExpression second);
    }
}
