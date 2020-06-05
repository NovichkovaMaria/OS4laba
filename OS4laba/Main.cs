using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS4laba
{
	class Main
	{
		private List<FatRecord> fat;
		private List<DiskCluster> dataRegion;
		private Dictionary<string, int> fileNames;

		public Main()
		{
			fat = new List<FatRecord>();
			dataRegion = new List<DiskCluster>();
			for (int i = 0; i < (int)SystemInfo.CLUSTERS; i++)
			{
				fat.Insert(i, new FatRecord());
				dataRegion.Insert(i, new DiskCluster());
			}
			fileNames = new Dictionary<string, int>();
		}

		private int getFreeSpace()
		{
			int freeClusters = 0;
			foreach (FatRecord rec in fat)
			{
				if (!rec.isBusy)
				{
					freeClusters++;
				}
			}
			return freeClusters * (int)SystemInfo.CLUSTER_SIZE;
		}

		public void writeFile(File newFile)
		{
			char[] data = newFile.data;
			string fileName = newFile.name;
			if (getFreeSpace() < data.Length)
			{
				Console.WriteLine("Not enougn space on disk");
			}
			else
			{
				int fileStart = firstFreeCluster();
				fileNames.Add(fileName, fileStart);
				if (data.Length > 0)
				{
					write(data, fileStart);
				}
				else
				{
					Console.WriteLine("File data is empty");
				}
			}
		}

		private void write(char[] data, int start)
		{
			fat[start].isBusy = true;
			if (data.Length > (int)SystemInfo.CLUSTER_SIZE)
			{
				char[] next = data.Skip((int)SystemInfo.CLUSTER_SIZE).ToArray();
				data = data.Take((int)SystemInfo.CLUSTER_SIZE).ToArray();
				int nextFree = firstFreeCluster();
				fat[start].next = nextFree;
				write(next, nextFree);
			}
			else
			{
				fat[start].next = (int)SystemInfo.EOC;
			}
			Console.WriteLine("Writing block " + data + "...");
			dataRegion[start].data = data;
			Console.WriteLine("Complete!");
		}

		private int firstFreeCluster()
		{
			for (int i = 0; i < fat.Count; i++)
			{
				if (!fat[i].isBusy)
				{
					return i;
				}
			}
			return -1;
		}

		public File getFile(string fileName)
		{
			char[] data = new char[0];
			int cluster;
			try
			{
				cluster = fileNames[fileName];
			}
			catch (Exception ex)
			{
				Console.WriteLine("File not found");
				return null;
			}

			char[] block;
			char[] temp;

			while (fat[cluster].next != (int)SystemInfo.EOC)
			{
				block = dataRegion[cluster].data;
				temp = new char[data.Length + block.Length];
				for (int i = 0; i < data.Length; i++)
				{
					temp[i] = data[i];
				}
				for (int i = data.Length, j = 0; i < temp.Length; i++, j++)
				{
					temp[i] = block[j];
				}
				data = temp;
				cluster = fat[cluster].next;
			}

			block = dataRegion[cluster].data;
			temp = new char[data.Length + block.Length];

			for (int i = 0; i < data.Length; i++)
			{
				temp[i] = data[i];
			}
			for (int i = data.Length, j = 0; i < temp.Length; i++, j++)
			{
				temp[i] = block[j];
			}
			data = temp;
			return new File(fileName, data);
		}

		public void deleteFile(string fileName)
		{
			int fileStart;
			try
			{
				fileStart = fileNames[fileName];
			}
			catch (Exception ex)
			{
				Console.WriteLine("File not found");
				return;
			}
			while (fat[fileStart].next != (int)SystemInfo.EOC)
			{
				fat[fileStart].isBusy = false;
				fileStart = fat[fileStart].next;
			}
			fat[fileStart].isBusy = false;
			fileNames.Remove(fileName);
			Console.WriteLine("File removed");
		}
	}
}
