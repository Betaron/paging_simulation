using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paging_simulation
{
	public class Page
	{
		public int pageNumber;
		public bool presence = false;
		public bool reffering = false;
		public int pageSize = 0;
		public byte[] pageComposition;

		public static List<Page> CutIntoPages(VirtualApp app, int pageSize)
		{
			List<Page> pages = new List<Page>();

			int pagesCount = app.size >= pageSize ? (int)Math.Ceiling((double)app.size / pageSize) : 1;

			for (int i = 0; i < pagesCount; i++)
			{
				pages.Add(new Page() { 
					pageNumber = RAM.freeAddresses.Dequeue(),
					pageSize = pageSize,
					pageComposition = app.appComposition.Skip(i * pageSize).Take(pageSize).ToArray()
				});
			}

			return pages;
		}
	}
}
