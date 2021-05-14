using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paging_simulation
{
	public struct PageTableEntry
	{
		public int virtualAddress;
		public int physicatAddress;
		public bool presence;
		public bool reffering;
	}

	public static class VirtualMemory
	{
		public static List<(VirtualApp, List<PageTableEntry>)> pagesTables = new();

		public static Queue<int> freeAddresses = new();
		public static readonly string path = @"D:\ExternalStorage";
		public static Page[] virtualPages;

		public static void Substitution()
		{

		}

		public static void MakeNewPageTableEntry(int physicalAddress)
		{

		}
	}
}
