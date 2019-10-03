using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimprExpression.Operators {
	public class SquareRoot : IExpression {
		public IExpression fst { get; set; }
		public double Compute(IDictionary<char, double> var) {
			return Math.Sqrt(fst.Compute(var));
		}

		public int Presedence() {
			return fst == null ? 3 : 0;
		}

        public bool CanCompute(ISet<char> variables) {
            return fst.CanCompute(variables);
        }

        public override string ToString() {
			return $"Sqrt({fst.ToString()})";
		}
	}
}
