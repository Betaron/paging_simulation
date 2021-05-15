using System;
using System.Collections.Generic;
using System.IO;

namespace paging_simulation
{
	class Program
	{
		static void Main(string[] args)
		{
			if (Directory.Exists(@"D:\PageMemSimulation\RAM"))
				Directory.Delete(@"D:\PageMemSimulation\RAM", true);
			if (Directory.Exists(@"D:\PageMemSimulation\ExternalStorage"))
				Directory.Delete(@"D:\PageMemSimulation\ExternalStorage", true);
			for (int i = 0; i < 10; i++)
			{
				RAM.freeAddresses.Enqueue(i);
			}
			AppsManager.RunCommandPrompt();
			AppsManager.executionThread.Join();
		}
	}
}
