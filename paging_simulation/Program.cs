using System;
using System.Collections.Generic;
using System.IO;

namespace paging_simulation
{
	class Program
	{
		static void Main(string[] args)
		{
			VirtualApp app1 = new(50);
			List<PageTableEntry> appTable1 = new();

			VirtualMemory.pagesTables.Add((app1, appTable1));

			VirtualApp app2 = new(50);
			List<PageTableEntry> appTable2 = new();

			VirtualMemory.pagesTables.Add((app2, appTable2));

			for (int i = 0; i < 10; i++)
			{
				RAM.freeAddresses.Enqueue(i);
			}

			Directory.CreateDirectory(RAM.path);
			RAM.LoadNewApp(Page.CutIntoPages(app1, 8), appTable1);
			RAM.LoadNewApp(Page.CutIntoPages(app2, 8), appTable2);
		}
	}
}
