using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace paging_simulation
{
	public static class AppsManager
	{
		public static Thread executionThread;
		public static void RunCommandPrompt()
		{
			executionThread = new Thread(()=> {
				bool exitFlag = false;
				string command = string.Empty;
				string arg = string.Empty;

				Console.WriteLine("Enter '/help' command for more info.");
				while (!exitFlag)
				{
					Console.Write("> ");
					string input = Console.ReadLine();
					int separatorPos = input.IndexOf(' ');
					int intArg = -1;
					if (separatorPos != -1)
					{
						command = input.Substring(0, separatorPos);
						arg = input.Substring(separatorPos + 1);

						if (!int.TryParse(arg, out intArg))
						{
							Console.WriteLine("Invalid argument.");
							continue;
						}
					}
					else
					{
						command = input;
						arg = string.Empty;
					}

					switch (command)
					{
						case "/help":
							Console.WriteLine(Resource1.help);
							break;
						case "/exit":
							exitFlag = true;
							for (int i = 0; i < VirtualMemory.pagesTables.FindAll(x => x != null).Count; i++)
							{
								VirtualMemory.pagesTables[i].Value.app.isStopWorking = true;
								VirtualMemory.pagesTables[i].Value.app.workingThread.Join();
								VirtualMemory.CloseApp(i);
							}
							break;
						case "/createApp":
							if (intArg < 1)
							{
								Console.WriteLine("Invalid argument.");
								break;
							}
							int freeIndex = VirtualMemory.pagesTables.IndexOf(null);
							if (freeIndex == -1)
							{
								VirtualMemory.pagesTables.Add((new(intArg), new()));
								freeIndex = VirtualMemory.pagesTables.Count - 1;
							}
							else
								VirtualMemory.pagesTables[freeIndex] = (new(intArg), new());
							RAM.LoadNewApp(Page.CutIntoPages(
								VirtualMemory.pagesTables[freeIndex].Value.app, 
								VirtualMemory.pageSize),
								VirtualMemory.pagesTables[freeIndex].Value.entries);
							VirtualMemory.pagesTables[freeIndex].Value.app.workingThread = VirtualApp.GetWorkingThread(freeIndex);
							VirtualMemory.pagesTables[freeIndex].Value.app.workingThread.Start();
							Console.WriteLine($"App was started with {(freeIndex != -1 ? freeIndex : VirtualMemory.pagesTables.Count - 1)} index");
							break;
						case "/closeApp":
							if (intArg > VirtualMemory.pagesTables.Count - 1 || intArg < 0)
							{
								Console.WriteLine("Invalid argument.");
								break;
							}
							VirtualMemory.pagesTables[intArg].Value.app.isStopWorking = true;
							VirtualMemory.pagesTables[intArg].Value.app.workingThread.Join();
							VirtualMemory.CloseApp(intArg);
							Console.WriteLine($"App {intArg} was closed.");
							break;
						case "/pauseApp":
							if (intArg < 0)
							{
								Console.WriteLine("Invalid argument.");
								break;
							}
							VirtualMemory.pagesTables[intArg].Value.app.isStopWorking = true;
							VirtualMemory.pagesTables[intArg].Value.app.workingThread.Join();
							break;
						case "/resumeApp":
							if (intArg < 0)
							{
								Console.WriteLine("Invalid argument.");
								break;
							}
							VirtualMemory.pagesTables[intArg].Value.app.isStopWorking = false;
							VirtualMemory.pagesTables[intArg].Value.app.workingThread = VirtualApp.GetWorkingThread(intArg);
							VirtualMemory.pagesTables[intArg].Value.app.workingThread.Start();
							break;
						default:
							Console.WriteLine("Invalid command.");
							break;
					}
				}
			});
			executionThread.Start();
		}
	}
}
