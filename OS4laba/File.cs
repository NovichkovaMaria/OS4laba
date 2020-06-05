using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS4laba
{
	public class File
	{
		public string name { get; private set; }

		public File(string name)
		{
			this.name = name;
		}
	}
}
