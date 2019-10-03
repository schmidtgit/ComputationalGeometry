using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimprExpression.Operators
{
    public class Constant : IExpression {
		public double value { get; set; }
		public double Compute(IDictionary<char, double> var) {
			return value;
		}

		public int Presedence() {
			return -1;
		}

        public bool CanCompute(ISet<char> variables) {
            return true;
        }

        public override string ToString() {
			return value.ToString();
		}
	}
}
