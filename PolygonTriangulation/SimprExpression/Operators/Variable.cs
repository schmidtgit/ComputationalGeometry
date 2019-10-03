using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimprExpression.Operators
{
    public class Variable : IExpression {
		public char name { get; set; }
		public double Compute(IDictionary<char, double> var) {
			return var[name];
		}

		public int Presedence() {
			return -1;
		}

        public bool CanCompute(ISet<char> variables) {
            return variables.Contains(name);
        }

        public override string ToString() {
			return name.ToString();
		}
	}
}
