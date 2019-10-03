using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimprExpression {
    public interface IExpression {
		double Compute(IDictionary<char, double> var);
		int Presedence();
        bool CanCompute(ISet<char> variables);
    }
}
