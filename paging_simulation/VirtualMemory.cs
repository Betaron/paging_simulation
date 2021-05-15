using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paging_simulation
{
	public class PageTableEntry
	{
		public int virtualAddress;
		public int physicalAddress;
		public bool presence;
		public bool reffering;
	}

	public static class VirtualMemory
	{
		public static List<(VirtualApp app, List<PageTableEntry> entries)?> pagesTables = new();
		public static object interrupt = new();
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
				if (pagesTables[i] == null)
					continue;
				for (int j = 0; j < pagesTables[i].Value.entries.Count; j++)
				{
					if (pagesTables[i].Value.entries[j].physicalAddress == physicalPageName)
					{
						string tablePath = Directory.CreateDirectory(Path.Combine(path, i.ToString())).FullName;
						virtualPagePath = Path.Combine(tablePath, pagesTables[i].Value.entries[j].virtualAddress.ToString());

						if (File.Exists(RAM_PagePath))
							File.Move(RAM_PagePath, virtualPagePath, true);

						RAM.freeAddresses.Enqueue(physicalPageName);

						pagesTables[i].Value.entries[j].physicalAddress = -1;
						pagesTables[i].Value.entries[j].presence = false;
						pagesTables[i].Value.entries[j].reffering = false;


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

			for (int i = 0; i < pagesTables[tableNum].Value.entries.Count; i++)
			{
				var item = pagesTables[tableNum].Value.entries[i];
				if (item.virtualAddress == virtualPageNum)
				{
					if (File.Exists(virtualPagePath))
						File.Move(virtualPagePath, RAM_PagePath, true);

					item.physicalAddress = physicalAddress;
					item.presence = true;
					item.reffering = true;

					RAM.substitutionQueue.Enqueue(physicalAddress);

					break;
				}
			}
		}

		public static void CloseApp(int tableNum)
		{
			var table = pagesTables[tableNum].Value.entries;

			foreach (var item in table)
			{
				string RAM_FilePath = Path.Combine(RAM.path, item.physicalAddress.ToString());
				if (File.Exists(RAM_FilePath))
				{
					File.Delete(RAM_FilePath);
					RAM.freeAddresses.Enqueue(item.physicalAddress);
				}
			}
			string tablePath = Path.Combine(path, tableNum.ToString());
			if (Directory.Exists(tablePath))
				Directory.Delete(tablePath, true);

			pagesTables[tableNum] = null;
		}

		public static byte GetByteFromMemory(int tableNum, int pageNum, int offset)
		{
			byte val;

			lock (interrupt)
			{

			if (!pagesTables[tableNum].Value.entries[pageNum].presence)
				LoadPageIntoRAM(tableNum, pageNum);

			using (FileStream stream = new FileStream(Path.Combine(
				RAM.path, pagesTables[tableNum].Value.entries[pageNum].physicalAddress.ToString()), 
				FileMode.Open))
			{
				stream.Seek(offset, SeekOrigin.Begin);
				val = (byte)stream.ReadByte();
			}
			pagesTables[tableNum].Value.entries[pageNum].reffering = false;
			}
			return val;
		}
	}
}
