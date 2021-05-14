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
		public byte[] appComposition;

		public VirtualApp(int size)
		{
			this.size = size;
			appComposition = new byte[size];

			Random rand = new Random();
			for (int i = 0; i < size; i++)
			{
				appComposition[i] = Convert.ToByte(rand.Next(0, 255));
			}
		}
	}
}
