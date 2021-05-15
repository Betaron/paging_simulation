using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paging_simulation
{
	public static class RAM
	{
		public static Queue<int> freeAddresses = new();
		public static Queue<int> substitutionQueue = new();
		public static readonly string path = @"D:\PageMemSimulation\RAM";

		public static void LoadNewApp(List<Page> pages, List<PageTableEntry> table)
		{
			Directory.CreateDirectory(RAM.path);
			for (int i = 0; i < pages.Count; i++)
			{
				lock (VirtualMemory.interrupt)
				{
					while (freeAddresses.Count == 0)
						VirtualMemory.UnloadPageFromRAM(substitutionQueue.Dequeue());

					int address = freeAddresses.Dequeue();

					File.WriteAllBytes(Path.Combine(path, address.ToString()), pages[i].pageComposition);

					substitutionQueue.Enqueue(address);

					table.Add(new()
					{
						virtualAddress = i,
						physicalAddress = address,
						presence = true,
						reffering = true
					});
				}
			}
		}
	}
}
