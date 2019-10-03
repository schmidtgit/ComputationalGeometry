using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.Launchers {
	class CowLauncher : BaseLauncher, IProgramLauncher {
		public bool Run() {
			return true;
		}
	}
}
