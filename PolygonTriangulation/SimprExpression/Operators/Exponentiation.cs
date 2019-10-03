using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimprExpression.Operators
{
    public class Exponentiation : IExpression, IOperator {
		public IExpression fst { get; set; }
		public IExpression snd { get; set; }
		public double Compute(IDictionary<char, double> var) {
			return Math.Pow(fst.Compute(var),snd.Compute(var));
		}
		public void Setup(IExpression first, IExpression second) {
			fst = first; snd = second;
		}

		public int Presedence() {
			return fst == null ? 3 : 0;
		}

        public bool CanCompute(ISet<char> variables) {
            return fst.CanCompute(variables) && snd.CanCompute(variables);
        }

        public override string ToString() {
			return $"{fst.ToString()}^({snd.ToString()})";
		}
	}
}
