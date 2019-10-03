using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.Launchers
{
	/// <summary>
	/// Small interface to let us get all implementations from the assembly
	/// </summary>
	public interface IProgramLauncher {
		bool Run();
	}
}
