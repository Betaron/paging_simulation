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
			List<PageTableEntry> appTable = new();

			for (int i = 0; i < 10; i++)
			{
				RAM.freeAddresses.Enqueue(i);
			}

			Directory.CreateDirectory(RAM.path);
			RAM.LoadNewApp(Page.CutIntoPages(app1, 8), appTable, 0);

		}
	}
}
