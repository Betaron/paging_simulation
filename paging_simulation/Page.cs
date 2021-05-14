using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paging_simulation
{
	public class Page
	{
		public int pageSize = 0;
		public byte[] pageComposition;

		public static List<Page> CutIntoPages(VirtualApp app, int pageSize)
		{
			List<Page> pages = new();

			int pagesCount = app.size >= pageSize ? (int)Math.Ceiling((double)app.size / pageSize) : 1;

			byte[] appComposition = VirtualApp.CreateRandomAppComposition(app.size);

			for (int i = 0; i < pagesCount; i++)
			{
				pages.Add(new() { 
					pageSize = pageSize,
					pageComposition = appComposition.Skip(i * pageSize).Take(pageSize).ToArray()
				});
			}

			return pages;
		}
	}
}
