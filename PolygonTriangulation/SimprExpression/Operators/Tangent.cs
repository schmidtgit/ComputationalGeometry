using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimprExpression.Operators
{
    public class Tangent : IExpression {
		public IExpression fst { get; set; }
		public double Compute(IDictionary<char, double> var) {
			return Math.Tan(fst.Compute(var));
		}

		public int Presedence() {
			return 0;
		}

        public bool CanCompute(ISet<char> variables) {
            return fst.CanCompute(variables);
        }

        public override string ToString() {
			return $"Tan({fst.ToString()})";
		}
	}

}
