using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using SroMapToNavmeshData.Structures;

namespace SroMapToNavmeshData
{
	// Token: 0x02000003 RID: 3
	internal class Program
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00003620 File Offset: 0x00001820
		private static void Main(string[] args)
		{
			Program.ConsoleWriteColor("--- Tool made by Mixizi (gigola123 on epvp). Version 0.0.4 \r\n", ConsoleColor.Green);
			Console.WriteLine("");
			bool flag = !Directory.Exists(string.Format("{0}\\Map", Directory.GetCurrentDirectory())) && !Directory.Exists(string.Format("{0}\\Data", Directory.GetCurrentDirectory()));
			if (flag)
			{
				throw new InvalidOperationException("Folder Map and Data not found, you should extract Data.pk2 and Map.pk2 .");
			}
			GlobalData.LoadObjectIfo();
			Console.WriteLine("Welcome to SROMapToNavmesh, the purpose of this tool is to convert .m/.o2 to .nvm. 100% of .m and .o2 are convertible.");
			Console.WriteLine("But some navmesh aren't completely 100% compatible with this tool, it'll will crash intentionally without saving if there is a problem so don't worry. \r\n");
			Program.ConsoleWriteColor("Options:\r\n", ConsoleColor.DarkYellow);
			Program.ConsoleWriteColor("     watch", ConsoleColor.Blue);
			Program.ConsoleWriteColor("               Watch Map directory for any change and convert automaticaly.\r\n", ConsoleColor.White);
			Program.ConsoleWriteColor("     convert", ConsoleColor.Green);
			Program.ConsoleWriteColor("             Convert only a certain region.\r\n", ConsoleColor.White);
			Console.WriteLine("");
			Console.Write("So you would like to ");
			Program.ConsoleWriteColor("watch", ConsoleColor.Blue);
			Program.ConsoleWriteColor(" or ", ConsoleColor.White);
			Program.ConsoleWriteColor("convert", ConsoleColor.Green);
			Program.ConsoleWriteColor(" ? ", ConsoleColor.White);
			string a = Console.ReadLine();
			bool flag2 = a == "watch";
			if (flag2)
			{
				Console.WriteLine("");
				Console.WriteLine("Waiting for change in Map folder...");
				FileSystemWatcher watcher = new FileSystemWatcher();
				watcher.IncludeSubdirectories = true;
				watcher.Path = string.Format("{0}\\Map", Directory.GetCurrentDirectory());
				watcher.NotifyFilter = NotifyFilters.LastWrite;
				watcher.Filter = "*.o2";
				watcher.Changed += delegate(object source, FileSystemEventArgs e)
				{
					try
					{
						watcher.EnableRaisingEvents = false;
						string[] array5 = e.FullPath.Split(new char[]
						{
							'\\'
						});
						Console.WriteLine("");
						int num = Convert.ToInt32(array5[array5.Length - 1].Replace(".o2", ""));
						int num2 = Convert.ToInt32(array5[array5.Length - 2]);
						Console.WriteLine(string.Format("Change detected on {0}/{1}.o2", num2, num));
						Thread.Sleep(1000);
						Program.ConvertMapToNvm(num, num2);
						Console.WriteLine("");
					}
					finally
					{
						watcher.EnableRaisingEvents = true;
					}
				};
				watcher.EnableRaisingEvents = true;
				new AutoResetEvent(false).WaitOne();
			}
			else
			{
				bool flag3 = a == "convert";
				if (flag3)
				{
					Console.WriteLine("");
					Console.WriteLine("Please write down which region you want to convert, you've 2 solutions: ");
					Console.WriteLine("");
					Console.WriteLine("First solution: The regionX and regionY like map just write -> 100 172");
					Console.WriteLine("Second solution: The full region in decimal -> 25772");
					Console.WriteLine("Example Jangan -> 97 168 or 25000 ");
					Console.Write("Region : ");
					string text = Console.ReadLine();
					bool flag4 = !text.Contains(" ") && text.Length == 5;
					if (flag4)
					{
						string text2 = Convert.ToInt32(text).ToString("X");
						bool flag5 = text2.Length == 4;
						if (flag5)
						{
							Console.WriteLine(text);
							Program.ConvertMapToNvm(Convert.ToInt32(text2.Substring(2, 2), 16), Convert.ToInt32(text2.Substring(0, 2), 16));
						}
						else
						{
							Program.ConsoleWriteColor("ERRORR ### Invalid region.", ConsoleColor.Red);
						}
					}
					else
					{
						bool flag6 = text.Contains(" ");
						if (flag6)
						{
							string[] array = text.Split(new char[]
							{
								' '
							});
							bool flag7 = array.Count<string>() == 2;
							if (flag7)
							{
								Program.ConvertMapToNvm(Convert.ToInt32(array[1]), Convert.ToInt32(array[0]));
							}
							else
							{
								Program.ConsoleWriteColor("ERRORR ### Invalid region.", ConsoleColor.Red);
							}
						}
					}
					Console.ReadKey();
				}
				else
				{
					bool flag8 = a == "copy";
					if (!flag8)
					{
						bool flag9 = a == "analyse";
						if (flag9)
						{
							string value = Console.ReadLine();
							string text3 = Convert.ToInt32(value).ToString("X");
							nvm nvm = nvm.Load(Convert.ToInt32(text3.Substring(2, 2), 16), Convert.ToInt32(text3.Substring(0, 2), 16));
							List<nvm.NavigationEntry> list = new List<nvm.NavigationEntry>();
							for (int i = 0; i < nvm.navigationEntries.Length; i++)
							{
								nvm.NavigationEntry navigationEntry = nvm.navigationEntries[i];
								Program.ConsoleWriteColor(" =================================== ", ConsoleColor.Yellow);
								Console.WriteLine(string.Format("id => {0}", navigationEntry.id));
								Console.WriteLine(string.Format("x => {0}", navigationEntry.x));
								Console.WriteLine(string.Format("y => {0}", navigationEntry.y));
								Console.WriteLine(string.Format("z => {0}", navigationEntry.z));
								Console.WriteLine(string.Format("collisionFlag => {0}", navigationEntry.collisionFlag));
								Console.WriteLine(string.Format("angle => {0}", navigationEntry.angle));
								Console.WriteLine(string.Format("unique => {0}", navigationEntry.unique));
								Console.WriteLine(string.Format("scale => {0}", navigationEntry.scale));
								Console.WriteLine(string.Format("eventZoneFlag => {0}", navigationEntry.eventZoneFlag));
								Console.WriteLine(string.Format("regionX => {0}", navigationEntry.regionX));
								Console.WriteLine(string.Format("regionY => {0}", navigationEntry.regionY));
								Console.WriteLine(string.Format("mountPointCount => {0}", navigationEntry.mountPointCount));
							}
							Console.ReadKey();
						}
						else
						{
							bool flag10 = a == "random-height";
							if (flag10)
							{
								string value2 = Console.ReadLine();
								string text4 = Convert.ToInt32(value2).ToString("X");
								m m = m.Load(Convert.ToInt32(text4.Substring(2, 2), 16), Convert.ToInt32(text4.Substring(0, 2), 16));
								m.Save(true);
								Program.ConvertMapToNvm(Convert.ToInt32(text4.Substring(2, 2), 16), Convert.ToInt32(text4.Substring(0, 2), 16));
								Console.ReadKey();
							}
							else
							{
								bool flag11 = a == "extract-event-objects";
								if (flag11)
								{
									string[] files = Directory.GetFiles("C:\\Users\\Emir\\Documents\\Visual Studio 2015\\Projects\\SroMapToNavmeshData\\SroMapToNavmeshData\\bin\\Debug\\Data\\navmesh", "*.nvm");
									List<string> list2 = new List<string>();
									foreach (string path in files)
									{
										try
										{
											nvm nvm2 = nvm.LoadFromFileName(Path.GetFileName(path));
											foreach (nvm.NavigationEntry navigationEntry2 in nvm2.navigationEntries)
											{
												bool flag12 = navigationEntry2.eventZoneFlag == 256;
												if (flag12)
												{
													list2.Add(Path.GetFileName(navigationEntry2.Bsr.filepath));
												}
											}
										}
										catch (Exception ex)
										{
										}
									}
									string[] array3 = list2.Distinct<string>().ToArray<string>();
									Console.WriteLine("-- Done : --");
									foreach (string value3 in array3)
									{
										Console.WriteLine(value3);
									}
									Console.ReadKey();
								}
								else
								{
									Program.ConsoleWriteColor("/!\\ Something went wrong. /!\\", ConsoleColor.Red);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00003D2C File Offset: 0x00001F2C
		public static void ConvertMapToNvm(int regionX, int regionY)
		{
			o2 o = o2.Load(regionX, regionY);
			Program.ConsoleWriteColor(string.Format("~## INFO: {0} elements analysed from .o2 ##~\r\n", o.elements.Length), ConsoleColor.Magenta);
			nvm nvm = nvm.Load(regionX, regionY);
			Program.ConsoleWriteColor(string.Format("#~# INFO: {0} elements analysed from .nvm #~#\r\n", nvm.navigationEntries.Length), ConsoleColor.Cyan);
			nvm.navigationEntries = o.exportElementsToNVMNavigationEntries();
			Program.ConsoleWriteColor(string.Format("##~ INFO: {0} elements imported from .o2 to .nvm (skipped {1} from .O2 file, many elements can be skipped because there is no collision on those items.) ~##\r\n", nvm.navigationEntries.Length, o.elements.Length - nvm.navigationEntries.Length), ConsoleColor.DarkYellow);
			m m = m.Load(regionX, regionY);
			float[] array = m.exportHeightMapToNVMHeightMap();
			Program.ConsoleWriteColor(string.Format("##~ INFO: heightmap extracted, height map are same in .m and .nvm ? {0} ) ~##\r\n", nvm.heightsMap.SequenceEqual(array)), ConsoleColor.DarkCyan);
			nvm.heightsMap = array;
			nvm.Save();
			Program.ConsoleWriteColor("#~# Success. #~#\r\n", ConsoleColor.Green);
			Program.ConsoleWriteColor(string.Format("~## INFO: Navmesh file generated-> ${0} ##~ \r\n", nvm.filepath), ConsoleColor.Magenta);
			Directory.CreateDirectory(string.Format("{0}\\SR_GameServer", Directory.GetCurrentDirectory()));
			Directory.CreateDirectory(string.Format("{0}\\SR_GameServer\\Data", Directory.GetCurrentDirectory()));
			Directory.CreateDirectory(string.Format("{0}\\SR_GameServer\\Data\\navmesh", Directory.GetCurrentDirectory()));
			File.Delete(string.Format("{0}\\SR_GameServer\\Data\\navmesh\\{1}", Directory.GetCurrentDirectory(), nvm.filename));
			File.Delete(string.Format("{0}\\SR_GameServer\\Data\\navmesh\\object.ifo", Directory.GetCurrentDirectory()));
			File.Copy(nvm.filepath, string.Format("{0}\\SR_GameServer\\Data\\navmesh\\{1}", Directory.GetCurrentDirectory(), nvm.filename));
			File.Copy(string.Format("{0}\\Map\\object.ifo", Directory.GetCurrentDirectory()), string.Format("{0}\\SR_GameServer\\Data\\navmesh\\object.ifo", Directory.GetCurrentDirectory()));
			Program.ConsoleWriteColor(string.Format("#~# INFO: SR_GameServer data files generated-> ${0} #~# \r\n", nvm.filepath), ConsoleColor.Magenta);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00003EF8 File Offset: 0x000020F8
		public static void ConsoleWriteColor(string str, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.Write(str);
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}
