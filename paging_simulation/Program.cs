using System;
using System.IO;

namespace paging_simulation
{
	class Program
	{
		static void Main(string[] args)
		{
			VirtualApp app1 = new VirtualApp(15);

			for (int i = 0; i < 10; i++)
			{
				RAM.freeAddresses.Enqueue(i);
			}
			
			Page.CutIntoPages(app1, 10);

			//Directory.CreateDirectory(RAM.path);
			//File.WriteAllBytes(Path.Combine(RAM.path, "app1.app"), app1.appComposition);
		}
	}
}
