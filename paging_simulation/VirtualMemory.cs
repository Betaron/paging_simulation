using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paging_simulation
{
	public static class VirtualMemory
	{
		public static (int, int)[] pagesTable;

		public static Queue<int> freeAddresses = new Queue<int>();
		public static readonly string path = @"D:\VirtualMemory";
		public static Page[] virtualPages;
	}
}
