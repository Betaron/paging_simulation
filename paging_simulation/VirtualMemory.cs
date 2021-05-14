using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paging_simulation
{
	public struct PageTableEntry
	{
		public int virtualAddress;
		public int physicalAddress;
		public bool presence;
		public bool reffering;
	}

	public static class VirtualMemory
	{
		public static List<(VirtualApp app, List<PageTableEntry> entries)> pagesTables = new();

		public static Queue<int> freeAddresses = new();
		public static readonly string path = @"D:\PageMemSimulation\ExternalStorage";
		public static Page[] virtualPages;
		public static int pageSize = 8;

		public static void UnloadPageFromRAM(int physicalPageName)
		{
			string RAM_PagePath = Path.Combine(RAM.path, physicalPageName.ToString());

			string virtualPagePath;

			for (int i = 0; i < pagesTables.Count; i++)
			{
				for (int j = 0; j < pagesTables[i].entries.Count; j++)
				{
					var item = pagesTables[i].entries[j];
					if (item.physicalAddress == physicalPageName)
					{
						string tablePath = Directory.CreateDirectory(Path.Combine(path, i.ToString())).FullName;
						virtualPagePath = Path.Combine(tablePath, item.virtualAddress.ToString());

						if (File.Exists(RAM_PagePath))
							File.Move(RAM_PagePath, virtualPagePath, true);

						RAM.freeAddresses.Enqueue(physicalPageName);

						item.physicalAddress = -1;
						item.presence = false;
						item.reffering = false;

						i = pagesTables.Count;
						break;
					}
				}
			}
		}

		public static void LoadPageIntoRAM(int tableNum, int virtualPageNum)
		{
			while (RAM.freeAddresses.Count == 0)
				UnloadPageFromRAM(RAM.substitutionQueue.Dequeue());
			int physicalAddress = RAM.freeAddresses.Dequeue();

			string RAM_PagePath = Path.Combine(RAM.path, physicalAddress.ToString());
			string virtualPagePath = Path.Combine(path, tableNum.ToString(), virtualPageNum.ToString());

			for (int i = 0; i < pagesTables[i].entries.Count; i++)
			{
				var item = pagesTables[tableNum].entries[i];
				if (item.virtualAddress == virtualPageNum)
				{
					if (File.Exists(virtualPagePath))
						File.Move(virtualPagePath, RAM_PagePath,true);

					item.physicalAddress = physicalAddress;
					item.presence = true;
					item.reffering = true;

					RAM.substitutionQueue.Enqueue(physicalAddress);

					break;
				}
			}
		}

		public static byte GetByteFromMemory(int tableNum, int pageNum, int offset)
		{
			PageTableEntry entry = pagesTables[tableNum].entries[pageNum];

			if (!entry.presence)
				LoadPageIntoRAM(tableNum, pageNum);

			byte val;
			using (FileStream stream = new FileStream(Path.Combine(RAM.path, entry.physicalAddress.ToString()), FileMode.Open))
			{
				stream.Seek(offset, SeekOrigin.Begin);
				val = (byte)stream.ReadByte();
			}
			entry.reffering = false;
			return val;
		}
	}
}
