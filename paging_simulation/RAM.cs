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
		public static readonly string path = @"D:\RAM";

		public static void LoadNewApp(List<Page> pages, List<PageTableEntry> table)
		{
			for (int i = 0; i < pages.Count; i++)
			{
				while (freeAddresses.Count == 0)
					VirtualMemory.Substitution();

				int address = freeAddresses.Dequeue();

				File.WriteAllBytes(Path.Combine(path, address.ToString()), pages[i].pageComposition);
				
				table.Add(new()
				{
					virtualAddress = i,
					physicatAddress = address,
					presence = true,
					reffering = true
				});
			}
		}
	}
}
