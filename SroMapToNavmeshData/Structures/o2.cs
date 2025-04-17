using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SroMapToNavmeshData.Structures
{
	// Token: 0x02000006 RID: 6
	internal class o2
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00004364 File Offset: 0x00002564
		public static o2 Load(int sectionX, int sectionY)
		{
			o2 o = new o2();
			o.filepath = string.Format("{0}\\Map\\{1}\\{2}.o2", Directory.GetCurrentDirectory(), sectionY.ToString(), sectionX.ToString());
			bool flag = !File.Exists(o.filepath);
			if (flag)
			{
				throw new InvalidOperationException(string.Format("File {0} not found.", o.filepath));
			}
			o2 result;
			using (Stream stream = new FileStream(o.filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
				{
					o.Header = new string(binaryReader.ReadChars(12));
					List<o2.Element> list = new List<o2.Element>();
					for (int i = 0; i < 144; i++)
					{
						ushort num = binaryReader.ReadUInt16();
						for (int j = 0; j < (int)num; j++)
						{
							o2.Element element = new o2.Element();
							element.id = binaryReader.ReadUInt32();
							element.x = binaryReader.ReadSingle();
							element.y = binaryReader.ReadSingle();
							element.z = binaryReader.ReadSingle();
							element.visibility = binaryReader.ReadUInt16();
							element.angle = binaryReader.ReadSingle();
							element.unique = binaryReader.ReadUInt32();
							element.scale = binaryReader.ReadUInt16();
							element.regionX = binaryReader.ReadByte();
							element.regionY = binaryReader.ReadByte();
							element.x += (float)(((int)element.regionX - sectionX) * 1920);
							element.z += (float)(((int)element.regionY - sectionY) * 1920);
							bool flag2 = !list.Any((o2.Element item) => item.unique == element.unique);
							if (flag2)
							{
								list.Add(element);
							}
							string text = GlobalData.objectIfo[Convert.ToInt32(element.id)].Replace("\\", "\\\\");
							bool flag3 = text != null;
							if (flag3)
							{
								bool flag4 = text.Contains(".bsr");
								if (flag4)
								{
									element.Bsr = bsr.Load(text);
								}
								else
								{
									element.Bsr = null;
								}
							}
							else
							{
								element.Bsr = null;
							}
						}
					}
					o.elements = list.ToArray();
					result = o;
				}
			}
			return result;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00004664 File Offset: 0x00002864
		public nvm.NavigationEntry[] exportElementsToNVMNavigationEntries()
		{
			List<nvm.NavigationEntry> list = new List<nvm.NavigationEntry>();
			for (int i = 0; i < this.elements.Length; i++)
			{
				nvm.NavigationEntry navigationEntry = new nvm.NavigationEntry
				{
					id = this.elements[i].id,
					x = this.elements[i].x,
					y = this.elements[i].y,
					z = this.elements[i].z,
					collisionFlag = ushort.MaxValue,
					angle = this.elements[i].angle,
					unique = (ushort)this.elements[i].unique,
					scale = this.elements[i].scale,
					eventZoneFlag = 0,
					regionX = this.elements[i].regionX,
					regionY = this.elements[i].regionY,
					mountPointCount = 0,
					Bsr = this.elements[i].Bsr
				};
				bool flag = navigationEntry.Bsr != null;
				if (flag)
				{
					bool flag2 = GlobalData.EventZoneFlagObjects.Contains(navigationEntry.Bsr.filename);
					if (flag2)
					{
						navigationEntry.eventZoneFlag = 256;
					}
					bool flag3 = GlobalData.NoCollisionFlagObjects.Contains(navigationEntry.Bsr.filename);
					if (flag3)
					{
						navigationEntry.collisionFlag = 0;
					}
					bool flag4 = navigationEntry.Bsr.Bms != null;
					if (flag4)
					{
						Console.WriteLine(navigationEntry.Bsr.Bms.pointers.Collision);
						bool flag5 = navigationEntry.Bsr.Bms.pointers.Collision > 0U;
						if (flag5)
						{
							list.Add(navigationEntry);
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x0400000E RID: 14
		public string filepath;

		// Token: 0x0400000F RID: 15
		public string Header;

		// Token: 0x04000010 RID: 16
		public int[] count;

		// Token: 0x04000011 RID: 17
		public o2.Element[] elements;

		// Token: 0x0200000D RID: 13
		public class Element
		{
			// Token: 0x0400003C RID: 60
			public uint id;

			// Token: 0x0400003D RID: 61
			public float x;

			// Token: 0x0400003E RID: 62
			public float y;

			// Token: 0x0400003F RID: 63
			public float z;

			// Token: 0x04000040 RID: 64
			public ushort visibility;

			// Token: 0x04000041 RID: 65
			public float angle;

			// Token: 0x04000042 RID: 66
			public uint unique;

			// Token: 0x04000043 RID: 67
			public ushort scale;

			// Token: 0x04000044 RID: 68
			public byte regionX;

			// Token: 0x04000045 RID: 69
			public byte regionY;

			// Token: 0x04000046 RID: 70
			public bsr Bsr;
		}
	}
}
