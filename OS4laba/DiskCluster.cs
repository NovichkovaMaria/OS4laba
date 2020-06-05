using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS4laba
{
	public class DiskCluster
	{
		public char[] data { get; set; }
		public DiskCluster()
		{
			data = new char[(char)SystemInfo.CLUSTER_SIZE];
		}
	}
}
