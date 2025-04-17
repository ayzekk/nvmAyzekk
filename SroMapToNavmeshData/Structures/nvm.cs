using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SroMapToNavmeshData.Structures
{
	// Token: 0x02000008 RID: 8
	internal class nvm
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00004F50 File Offset: 0x00003150
		public static nvm LoadFromFileName(string filename)
		{
			int sectionY = (int)Convert.ToInt16(filename.Substring(3, 2), 16);
			int sectionX = (int)Convert.ToInt16(filename.Substring(5, 2), 16);
			return nvm.Load(sectionX, sectionY);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00004F8C File Offset: 0x0000318C
		public static nvm Load(int sectionX, int sectionY)
		{
			nvm nvm = new nvm();
			nvm.filename = string.Format("nv_{0}{1}.nvm", ((sectionY.ToString("X").Length == 1) ? "0" : "") + sectionY.ToString("X"), ((sectionX.ToString("X").Length == 1) ? "0" : "") + sectionX.ToString("X"));
			nvm.filepath = string.Format("{0}\\Data\\navmesh\\{1}", Directory.GetCurrentDirectory(), nvm.filename);
			bool flag = !File.Exists(nvm.filepath);
			if (flag)
			{
				throw new InvalidOperationException(string.Format("File {0} not found.", nvm.filepath));
			}
			nvm result;
			using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(nvm.filepath)))
			{
				nvm.Header = binaryReader.ReadChars(12);
				nvm.navigationEntriesCount = (uint)binaryReader.ReadUInt16();
				nvm.navigationEntries = new nvm.NavigationEntry[nvm.navigationEntriesCount];
				int num = 0;
				while ((long)num < (long)((ulong)nvm.navigationEntriesCount))
				{
					nvm.navigationEntries[num] = default(nvm.NavigationEntry);
					nvm.navigationEntries[num].id = binaryReader.ReadUInt32();
					nvm.navigationEntries[num].x = binaryReader.ReadSingle();
					nvm.navigationEntries[num].y = binaryReader.ReadSingle();
					nvm.navigationEntries[num].z = binaryReader.ReadSingle();
					nvm.navigationEntries[num].collisionFlag = binaryReader.ReadUInt16();
					nvm.navigationEntries[num].angle = binaryReader.ReadSingle();
					nvm.navigationEntries[num].unique = binaryReader.ReadUInt16();
					nvm.navigationEntries[num].scale = binaryReader.ReadUInt16();
					nvm.navigationEntries[num].eventZoneFlag = binaryReader.ReadUInt16();
					nvm.navigationEntries[num].regionX = binaryReader.ReadByte();
					nvm.navigationEntries[num].regionY = binaryReader.ReadByte();
					nvm.navigationEntries[num].mountPointCount = binaryReader.ReadUInt16();
					bool flag2 = nvm.navigationEntries[num].mountPointCount > 0;
					if (flag2)
					{
						throw new InvalidOperationException("NavMesh with MontPoint isn't avaible yet");
					}
					nvm.navigationEntries[num].mountPoints = new byte[0];
					nvm.navigationEntries[num].Bsr = bsr.Load(GlobalData.objectIfo[Convert.ToInt32(nvm.navigationEntries[num].id)].Replace("\\", "\\\\"));
					num++;
				}
				nvm.navigationCellsCount = binaryReader.ReadUInt32();
				nvm.navigationCellsExtraCount = binaryReader.ReadUInt32();
				nvm.navigationCells = new nvm.NavigationCell[nvm.navigationCellsCount];
				int num2 = 0;
				while ((long)num2 < (long)((ulong)nvm.navigationCellsCount))
				{
					nvm.navigationCells[num2] = default(nvm.NavigationCell);
					nvm.navigationCells[num2].minX = binaryReader.ReadSingle();
					nvm.navigationCells[num2].minY = binaryReader.ReadSingle();
					nvm.navigationCells[num2].maxX = binaryReader.ReadSingle();
					nvm.navigationCells[num2].maxY = binaryReader.ReadSingle();
					nvm.navigationCells[num2].entryCount = binaryReader.ReadByte();
					nvm.navigationCells[num2].entries = new ushort[(int)nvm.navigationCells[num2].entryCount];
					for (int i = 0; i < (int)nvm.navigationCells[num2].entryCount; i++)
					{
						nvm.navigationCells[num2].entries[i] = binaryReader.ReadUInt16();
					}
					num2++;
				}
				nvm.regionLinkCount = binaryReader.ReadUInt32();
				nvm.regionLinks = new nvm.RegionLink[nvm.regionLinkCount];
				int num3 = 0;
				while ((long)num3 < (long)((ulong)nvm.regionLinkCount))
				{
					nvm.regionLinks[num3] = default(nvm.RegionLink);
					nvm.regionLinks[num3].minX = binaryReader.ReadSingle();
					nvm.regionLinks[num3].minY = binaryReader.ReadSingle();
					nvm.regionLinks[num3].maxX = binaryReader.ReadSingle();
					nvm.regionLinks[num3].maxY = binaryReader.ReadSingle();
					nvm.regionLinks[num3].lineFlag = binaryReader.ReadByte();
					nvm.regionLinks[num3].lineSource = binaryReader.ReadByte();
					nvm.regionLinks[num3].lineDestination = binaryReader.ReadByte();
					nvm.regionLinks[num3].cellSource = binaryReader.ReadUInt16();
					nvm.regionLinks[num3].cellDestination = binaryReader.ReadUInt16();
					nvm.regionLinks[num3].regionSource = binaryReader.ReadUInt16();
					nvm.regionLinks[num3].regionDestination = binaryReader.ReadUInt16();
					num3++;
				}
				nvm.cellLinkCount = binaryReader.ReadUInt32();
				nvm.cellLinks = new nvm.CellLink[nvm.cellLinkCount];
				int num4 = 0;
				while ((long)num4 < (long)((ulong)nvm.cellLinkCount))
				{
					nvm.cellLinks[num4] = default(nvm.CellLink);
					nvm.cellLinks[num4].minX = binaryReader.ReadSingle();
					nvm.cellLinks[num4].minY = binaryReader.ReadSingle();
					nvm.cellLinks[num4].maxX = binaryReader.ReadSingle();
					nvm.cellLinks[num4].maxY = binaryReader.ReadSingle();
					nvm.cellLinks[num4].lineFlag = binaryReader.ReadByte();
					nvm.cellLinks[num4].lineSource = binaryReader.ReadByte();
					nvm.cellLinks[num4].lineDestination = binaryReader.ReadByte();
					nvm.cellLinks[num4].cellSource = binaryReader.ReadUInt16();
					nvm.cellLinks[num4].cellDestination = binaryReader.ReadUInt16();
					num4++;
				}
				nvm.textures = new nvm.TextureMap[9216];
				for (int j = 0; j < 9216; j++)
				{
					nvm.textures[j] = default(nvm.TextureMap);
					nvm.textures[j].w1 = binaryReader.ReadUInt16();
					nvm.textures[j].w2 = binaryReader.ReadUInt16();
					nvm.textures[j].w3 = binaryReader.ReadUInt16();
					nvm.textures[j].w4 = binaryReader.ReadUInt16();
				}
				nvm.heightsMap = new float[9409];
				for (int k = 0; k < 9409; k++)
				{
					nvm.heightsMap[k] = binaryReader.ReadSingle();
				}
				long num5 = binaryReader.BaseStream.Length - binaryReader.BaseStream.Position;
				nvm.unkBuffer0 = binaryReader.ReadBytes(36);
				nvm.unkBuffer1 = new float[36];
				for (int l = 0; l < 36; l++)
				{
					nvm.unkBuffer1[l] = binaryReader.ReadSingle();
				}
				binaryReader.Close();
				result = nvm;
			}
			return result;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000057B0 File Offset: 0x000039B0
		public void GenerateNavigationCellsFromNavigationEntry()
		{
			this.navigationCellsExtraCount = 0U;
			for (int i = 0; i < this.navigationCells.Length; i++)
			{
				RectangleF rectangleF = new RectangleF(Math.Min(this.navigationCells[i].minX, this.navigationCells[i].maxX), Math.Min(this.navigationCells[i].minY, this.navigationCells[i].maxY), Math.Abs(this.navigationCells[i].minX - this.navigationCells[i].maxX), Math.Abs(this.navigationCells[i].minY - this.navigationCells[i].maxY));
				List<ushort> list = new List<ushort>();
				for (int j = 0; j < this.navigationEntries.Length; j++)
				{
					nvm.NavigationEntry navigationEntry = this.navigationEntries[j];
					bool flag = navigationEntry.Bsr != null && navigationEntry.collisionFlag > 0;
					if (flag)
					{
						PointF pointF = new PointF(navigationEntry.x - navigationEntry.Bsr.box.Width / 2f, navigationEntry.z - navigationEntry.Bsr.box.Height / 2f);
						PointF pointF2 = new PointF(navigationEntry.x + navigationEntry.Bsr.box.Width / 2f, navigationEntry.z + navigationEntry.Bsr.box.Height / 2f);
						RectangleF rect = new RectangleF(Math.Min(pointF.X, pointF2.X), Math.Min(pointF.Y, pointF2.Y), Math.Abs(pointF.X - pointF2.X), Math.Abs(pointF.Y - pointF2.Y));
						bool flag2 = rectangleF.IntersectsWith(rect);
						if (flag2)
						{
							list.Add(Convert.ToUInt16(j));
						}
					}
				}
				this.navigationCells[i].entryCount = Convert.ToByte(list.Count);
				this.navigationCells[i].entries = list.ToArray();
				this.navigationCellsExtraCount += (uint)this.navigationCells[i].entryCount;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00005A30 File Offset: 0x00003C30
		public void Save()
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(this.filepath, FileMode.Create)))
			{
				binaryWriter.Write(this.Header);
				binaryWriter.Write((ushort)this.navigationEntries.Length);
				for (int i = 0; i < this.navigationEntries.Length; i++)
				{
					nvm.NavigationEntry navigationEntry = this.navigationEntries[i];
					binaryWriter.Write(navigationEntry.id);
					binaryWriter.Write(navigationEntry.x);
					binaryWriter.Write(navigationEntry.y);
					binaryWriter.Write(navigationEntry.z);
					binaryWriter.Write(navigationEntry.collisionFlag);
					binaryWriter.Write(navigationEntry.angle);
					binaryWriter.Write(navigationEntry.unique);
					binaryWriter.Write(navigationEntry.scale);
					binaryWriter.Write(navigationEntry.eventZoneFlag);
					binaryWriter.Write(navigationEntry.regionX);
					binaryWriter.Write(navigationEntry.regionY);
					binaryWriter.Write(navigationEntry.mountPointCount);
					for (int j = 0; j < (int)navigationEntry.mountPointCount; j++)
					{
						binaryWriter.Write(navigationEntry.mountPoints[j]);
					}
				}
				this.GenerateNavigationCellsFromNavigationEntry();
				binaryWriter.Write(this.navigationCellsCount);
				binaryWriter.Write(this.navigationCellsExtraCount);
				for (int k = 0; k < this.navigationCells.Length; k++)
				{
					nvm.NavigationCell navigationCell = this.navigationCells[k];
					binaryWriter.Write(navigationCell.minX);
					binaryWriter.Write(navigationCell.minY);
					binaryWriter.Write(navigationCell.maxX);
					binaryWriter.Write(navigationCell.maxY);
					binaryWriter.Write((byte)navigationCell.entries.Length);
					for (int l = 0; l < navigationCell.entries.Length; l++)
					{
						binaryWriter.Write(navigationCell.entries[l]);
					}
				}
				binaryWriter.Write((uint)this.regionLinks.Length);
				for (int m = 0; m < this.regionLinks.Length; m++)
				{
					binaryWriter.Write(this.regionLinks[m].minX);
					binaryWriter.Write(this.regionLinks[m].minY);
					binaryWriter.Write(this.regionLinks[m].maxX);
					binaryWriter.Write(this.regionLinks[m].maxY);
					binaryWriter.Write(this.regionLinks[m].lineFlag);
					binaryWriter.Write(this.regionLinks[m].lineSource);
					binaryWriter.Write(this.regionLinks[m].lineDestination);
					binaryWriter.Write(this.regionLinks[m].cellSource);
					binaryWriter.Write(this.regionLinks[m].cellDestination);
					binaryWriter.Write(this.regionLinks[m].regionSource);
					binaryWriter.Write(this.regionLinks[m].regionDestination);
				}
				binaryWriter.Write((uint)this.cellLinks.Length);
				for (int n = 0; n < this.cellLinks.Length; n++)
				{
					binaryWriter.Write(this.cellLinks[n].minX);
					binaryWriter.Write(this.cellLinks[n].minY);
					binaryWriter.Write(this.cellLinks[n].maxX);
					binaryWriter.Write(this.cellLinks[n].maxY);
					binaryWriter.Write(this.cellLinks[n].lineFlag);
					binaryWriter.Write(this.cellLinks[n].lineSource);
					binaryWriter.Write(this.cellLinks[n].lineDestination);
					binaryWriter.Write(this.cellLinks[n].cellSource);
					binaryWriter.Write(this.cellLinks[n].cellDestination);
				}
				using (BinaryWriter binaryWriter2 = new BinaryWriter(File.Open("./texture.nvm", FileMode.Create)))
				{
					for (int num = 0; num < 9216; num++)
					{
						binaryWriter.Write(this.textures[num].w1);
						binaryWriter.Write(this.textures[num].w2);
						binaryWriter.Write(this.textures[num].w3);
						binaryWriter.Write(this.textures[num].w4);
						binaryWriter2.Write(this.textures[num].w4);
					}
				}
				for (int num2 = 0; num2 < 9409; num2++)
				{
					binaryWriter.Write(this.heightsMap[num2]);
				}
				binaryWriter.Write(this.unkBuffer0);
				for (int num3 = 0; num3 < 36; num3++)
				{
					binaryWriter.Write(this.unkBuffer1[num3]);
				}
			}
		}

		// Token: 0x04000016 RID: 22
		public string filepath;

		// Token: 0x04000017 RID: 23
		public string filename;

		// Token: 0x04000018 RID: 24
		public char[] Header;

		// Token: 0x04000019 RID: 25
		public uint navigationEntriesCount;

		// Token: 0x0400001A RID: 26
		public nvm.NavigationEntry[] navigationEntries;

		// Token: 0x0400001B RID: 27
		public uint navigationCellsCount;

		// Token: 0x0400001C RID: 28
		public uint navigationCellsExtraCount;

		// Token: 0x0400001D RID: 29
		public nvm.NavigationCell[] navigationCells;

		// Token: 0x0400001E RID: 30
		public uint regionLinkCount;

		// Token: 0x0400001F RID: 31
		public nvm.RegionLink[] regionLinks;

		// Token: 0x04000020 RID: 32
		public uint cellLinkCount;

		// Token: 0x04000021 RID: 33
		public nvm.CellLink[] cellLinks;

		// Token: 0x04000022 RID: 34
		public nvm.TextureMap[] textures;

		// Token: 0x04000023 RID: 35
		public float[] heightsMap;

		// Token: 0x04000024 RID: 36
		public byte[] unkBuffer0;

		// Token: 0x04000025 RID: 37
		public float[] unkBuffer1;

		// Token: 0x02000010 RID: 16
		public struct NavigationEntry
		{
			// Token: 0x04000051 RID: 81
			public uint id;

			// Token: 0x04000052 RID: 82
			public float x;

			// Token: 0x04000053 RID: 83
			public float y;

			// Token: 0x04000054 RID: 84
			public float z;

			// Token: 0x04000055 RID: 85
			public ushort collisionFlag;

			// Token: 0x04000056 RID: 86
			public float angle;

			// Token: 0x04000057 RID: 87
			public ushort unique;

			// Token: 0x04000058 RID: 88
			public ushort scale;

			// Token: 0x04000059 RID: 89
			public ushort eventZoneFlag;

			// Token: 0x0400005A RID: 90
			public byte regionX;

			// Token: 0x0400005B RID: 91
			public byte regionY;

			// Token: 0x0400005C RID: 92
			public ushort mountPointCount;

			// Token: 0x0400005D RID: 93
			public byte[] mountPoints;

			// Token: 0x0400005E RID: 94
			public bsr Bsr;
		}

		// Token: 0x02000011 RID: 17
		public struct NavigationCell
		{
			// Token: 0x0400005F RID: 95
			public float minX;

			// Token: 0x04000060 RID: 96
			public float minY;

			// Token: 0x04000061 RID: 97
			public float maxX;

			// Token: 0x04000062 RID: 98
			public float maxY;

			// Token: 0x04000063 RID: 99
			public byte entryCount;

			// Token: 0x04000064 RID: 100
			public ushort[] entries;
		}

		// Token: 0x02000012 RID: 18
		public struct RegionLink
		{
			// Token: 0x04000065 RID: 101
			public float minX;

			// Token: 0x04000066 RID: 102
			public float minY;

			// Token: 0x04000067 RID: 103
			public float maxX;

			// Token: 0x04000068 RID: 104
			public float maxY;

			// Token: 0x04000069 RID: 105
			public byte lineFlag;

			// Token: 0x0400006A RID: 106
			public byte lineSource;

			// Token: 0x0400006B RID: 107
			public byte lineDestination;

			// Token: 0x0400006C RID: 108
			public ushort cellSource;

			// Token: 0x0400006D RID: 109
			public ushort cellDestination;

			// Token: 0x0400006E RID: 110
			public ushort regionSource;

			// Token: 0x0400006F RID: 111
			public ushort regionDestination;
		}

		// Token: 0x02000013 RID: 19
		public struct CellLink
		{
			// Token: 0x04000070 RID: 112
			public float minX;

			// Token: 0x04000071 RID: 113
			public float minY;

			// Token: 0x04000072 RID: 114
			public float maxX;

			// Token: 0x04000073 RID: 115
			public float maxY;

			// Token: 0x04000074 RID: 116
			public byte lineFlag;

			// Token: 0x04000075 RID: 117
			public byte lineSource;

			// Token: 0x04000076 RID: 118
			public byte lineDestination;

			// Token: 0x04000077 RID: 119
			public ushort cellSource;

			// Token: 0x04000078 RID: 120
			public ushort cellDestination;
		}

		// Token: 0x02000014 RID: 20
		public struct TextureMap
		{
			// Token: 0x04000079 RID: 121
			public ushort w1;

			// Token: 0x0400007A RID: 122
			public ushort w2;

			// Token: 0x0400007B RID: 123
			public ushort w3;

			// Token: 0x0400007C RID: 124
			public ushort w4;
		}
	}
}
