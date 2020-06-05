using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS4laba
{
    class Program
    {
		public static void Main(string[] args)
		{
			Main main = new Main();
			while (true)
			{
				string input = Console.ReadLine();
				if (input.Equals("stop"))
				{
					break;
				}
				string[] splitted = input.Split(' ');
				if (splitted[0].Equals("create") && splitted.Length == 3)
				{
					File newFile = new File(splitted[1], splitted[2].ToCharArray());
					main.writeFile(newFile);
					continue;
				}
				if (splitted[0].Equals("delete") && splitted.Length == 2)
				{
					main.deleteFile(splitted[1]);
					continue;
				}
				if (splitted[0].Equals("get") && splitted.Length == 2)
				{
					File file = main.getFile(splitted[1]);
					if (file != null)
						Console.WriteLine(file.data);
					continue;
				}
				Console.WriteLine("Wrong format!");
			}
		}
	}
}
