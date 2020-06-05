using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS4laba
{
	public class FatRecord
	{
		public bool isBusy { get; set; }
		public int next { get; set; }

		public FatRecord()
		{
			isBusy = false;
			next = 0;
		}
	}
}
