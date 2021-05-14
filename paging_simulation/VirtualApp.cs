using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paging_simulation
{
	public class VirtualApp
	{
		public int size { private set; get; }

		public VirtualApp(int size)
		{
			this.size = size;
		}

		public static byte[] CreateRandomAppComposition(int size)
		{
			byte[] appComposition = new byte[size];
			Random rand = new();

			for (int i = 0; i < size; i++)
			{
				appComposition[i] = Convert.ToByte(rand.Next(0, 255));
			}

			return appComposition;
		}

		public void MemoryRequest(int tableNum)
		{
			int fullAddress = new Random().Next(size);
			int pageNum = fullAddress >> (int)Math.Log2(VirtualMemory.pageSize);
			int offset = fullAddress & ((1 << (int)Math.Log2(VirtualMemory.pageSize)) - 1);

			byte data = VirtualMemory.GetByteFromMemory(tableNum, pageNum, tableNum);
		}
	}
}