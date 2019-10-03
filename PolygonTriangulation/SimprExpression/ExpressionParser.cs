using SimprExpression.Operators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimprExpression
{
    public class ExpressionParser {
		public static IExpression Parse(string s) {
			try {
                s = s.ToLower();
                var e = new List<IExpression>();
				int skip;
				double result;

				// Translate string into objects
				for (int i = 0; i < s.Length; i++) {
					switch (s[i]) {
						case '+':
							e.Add(new Addition());
							break;
						case '-':
							if (i == 0 || !char.IsDigit(s[i - 1])) {
								if (char.IsDigit(s[i])) {
									i++;
									skip = ParseDouble(s, i, out result);
									e.Add(new Constant { value = -result });
									i += skip;
									break;
								}
							}
							e.Add(new Subtraction());
							break;
						case '*':
							e.Add(new Multiplication());
							break;
						case '/':
							e.Add(new Division());
							break;
						case '^':
							e.Add(new Exponentiation());
							break;
						case 's':
							if (s.Length > i + 4 && s[i + 1] == 'i' && s[i + 2] == 'n' && s[i + 3] == '(') {
								i += 4;
								skip = FindClosingParentes(s, i);
								e.Add(new Sine() { fst = Parse(s.Substring(i, skip)) });
								i += skip;
							} else if (s.Length > i + 5 && s[i + 1] == 'q' && s[i + 2] == 'r' && s[i + 3] == 't' && s[i + 4] == '(') {
								i += 5;
								skip = FindClosingParentes(s, i);
								e.Add(new SquareRoot() { fst = Parse(s.Substring(i, skip)) });
								i += skip;
							} else {
								e.Add(new Variable { name = s[i] });
							}
							break;
						case 'c':
							if (s.Length > i + 4 && s[i + 1] == 'o' && s[i + 2] == 's' && s[i + 3] == '(') {
								i += 4;
								skip = FindClosingParentes(s, i);
								e.Add(new Cosine() { fst = Parse(s.Substring(i, skip)) });
								i += skip;
							} else {
								e.Add(new Variable { name = s[i] });
							}
							break;
						case 't':
							if (s.Length > i + 4 && s[i + 1] == 'a' && s[i + 2] == 'n' && s[i + 3] == '(') {
								i += 4;
								skip = FindClosingParentes(s, i);
								e.Add(new Tangent() { fst = Parse(s.Substring(i, skip)) });
								i += skip;
							} else {
								e.Add(new Variable { name = s[i] });
							}
							break;
						case '(':
							i++;
							skip = FindClosingParentes(s, i);
							e.Add(Parse(s.Substring(i, skip)));
							i += skip;
							break;
						case ')':
							throw new Exception("Something went wrong...");
						default:
							if (char.IsLetter(s[i]))
								e.Add(new Variable { name = s[i] });
							else if (char.IsDigit(s[i])) {
								skip = ParseDouble(s, i, out result);
								e.Add(new Constant { value = result });
								i += skip;
							}
							break;
					}
				}

				// Add implicit multiplications
				for (int i = 0; i < e.Count - 1; i++) {
					if (e[i].Presedence() == -1 && e[i + 1].Presedence() == -1) {
						e[i] = new Multiplication { fst = e[i], snd = e[i + 1] };
						e.RemoveAt(i + 1);
						i--; // Check again, in case of xyz
					}
				}

				// Put the expression together!
				while (e.Count != 1) {
					int maxPrecedence = 0, index = 0;
					for (int i = 0; i < e.Count; i++)
						if (e[i].Presedence() > maxPrecedence) {
							maxPrecedence = e[i].Presedence();
							index = i;
						}

					// Special -x case
					if (index == 0 && e[index] is Subtraction) {
						e[index] = new Multiplication { fst = new Constant { value = -1 }, snd = e[index + 1] };
						e.RemoveAt(index + 1);
						continue;
					}

					(e[index] as IOperator).Setup(e[index - 1], e[index + 1]);
					e.RemoveAt(index + 1);
					e.RemoveAt(index - 1);
				}
			return e.FirstOrDefault();
			} catch (Exception) { throw new ArgumentException(); }
		}

		private static int ParseDouble(string s, int index, out double result) {
			int skip = -1;
			for (int i = index; i < s.Length; i++) {
				if (char.IsDigit(s[i]) || s[i] == '.' || s[i] == ',')
					skip++;
				else
					break;
			}
			result = double.Parse(s.Substring(index, skip+1));
			return skip;
		}

		private static int FindClosingParentes(string s, int index) {
			var level = 0;
			for (int i = index; i < s.Length; i++) {
				if (s[i] == '(')
					level++;
				else if (s[i] == ')' && level == 0)
					return i - index;
				else if (s[i] == ')')
					level--;
			}
			throw new ArgumentException();
		}
    }
}
