using System;
using System.Collections.Generic;
using System.IO;

namespace SroMapToNavmeshData.Structures
{
	// Token: 0x02000007 RID: 7
	internal class m
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00004850 File Offset: 0x00002A50
		public static m Load(int sectionX, int sectionY)
		{
			m m = new m();
			m.filepath = string.Format("{0}\\Map\\{1}\\{2}.m", Directory.GetCurrentDirectory(), sectionY.ToString(), sectionX.ToString());
			bool flag = !File.Exists(m.filepath);
			if (flag)
			{
				throw new InvalidOperationException(string.Format("File {0} not found.", m.filepath));
			}
			using (Stream stream = new FileStream(m.filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
				{
					m.Header = binaryReader.ReadChars(12);
					int num = 0;
					m.blocks = new m.Block[36];
					m.points = new float[102, 102];
					for (int i = 0; i < 6; i++)
					{
						for (int j = 0; j < 6; j++)
						{
							m.blocks[num] = new m.Block();
							m.blocks[num].name = binaryReader.ReadBytes(6);
							m.blocks[num].cells = new m.Block.Cell[289];
							int num2 = 0;
							for (int k = 0; k < 17; k++)
							{
								for (int l = 0; l < 17; l++)
								{
									m.blocks[num].cells[num2] = new m.Block.Cell();
									m.blocks[num].cells[num2].x = l;
									m.blocks[num].cells[num2].y = k;
									m.blocks[num].cells[num2].height = binaryReader.ReadSingle();
									m.blocks[num].cells[num2].texture = binaryReader.ReadUInt16();
									m.blocks[num].cells[num2].brightness = binaryReader.ReadByte();
									int num3 = l + j * 16;
									int num4 = k + i * 16;
									m.points[num4, num3] = m.blocks[num].cells[num2].height;
									num2++;
								}
							}
							m.blocks[num].density = binaryReader.ReadByte();
							m.blocks[num].unkByte0 = binaryReader.ReadByte();
							m.blocks[num].waterLevel = binaryReader.ReadSingle();
							m.blocks[num].cellsExtra = new m.Block.CellExtra[256];
							for (int n = 0; n < 256; n++)
							{
								m.blocks[num].cellsExtra[n] = new m.Block.CellExtra();
								m.blocks[num].cellsExtra[n].min = binaryReader.ReadByte();
								m.blocks[num].cellsExtra[n].max = binaryReader.ReadByte();
							}
							m.blocks[num].heightMax = binaryReader.ReadSingle();
							m.blocks[num].heightMin = binaryReader.ReadSingle();
							m.blocks[num].unkBuffer0 = binaryReader.ReadBytes(20);
							num++;
						}
					}
				}
			}
			return m;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00004BE0 File Offset: 0x00002DE0
		public float[] exportHeightMapToNVMHeightMap()
		{
			int num = 0;
			float[,] array = new float[102, 102];
			List<float> list = new List<float>();
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					int num2 = 0;
					for (int k = 0; k < 17; k++)
					{
						for (int l = 0; l < 17; l++)
						{
							int num3 = l + j * 16;
							int num4 = k + i * 16;
							array[num4, num3] = this.blocks[num].cells[num2].height;
							num2++;
						}
					}
				}
			}
			for (int m = 0; m < 97; m++)
			{
				for (int n = 0; n < 97; n++)
				{
					list.Add(this.points[m, n]);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00004CF4 File Offset: 0x00002EF4
		public void Save(bool random)
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(this.filepath, FileMode.Create)))
			{
				binaryWriter.Write(this.Header);
				int num = 0;
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < 6; j++)
					{
						binaryWriter.Write(this.blocks[num].name);
						int num2 = 0;
						float num3 = 0f;
						float num4 = 0f;
						for (int k = 0; k < 17; k++)
						{
							for (int l = 0; l < 17; l++)
							{
								float num5 = this.blocks[num].cells[num2].height;
								if (random)
								{
									Random random2 = new Random();
									num5 = (float)(random2.NextDouble() * -60.0);
									Console.WriteLine(num5);
								}
								bool flag = num5 > num3;
								if (flag)
								{
									num3 = num5;
								}
								bool flag2 = num5 < num4;
								if (flag2)
								{
									num4 = num5;
								}
								binaryWriter.Write(num5);
								binaryWriter.Write(this.blocks[num].cells[num2].texture);
								binaryWriter.Write(this.blocks[num].cells[num2].brightness);
								num2++;
							}
						}
						binaryWriter.Write(this.blocks[num].density);
						binaryWriter.Write(this.blocks[num].unkByte0);
						binaryWriter.Write(this.blocks[num].waterLevel);
						for (int m = 0; m < 256; m++)
						{
							binaryWriter.Write(this.blocks[num].cellsExtra[m].min);
							binaryWriter.Write(this.blocks[num].cellsExtra[m].max);
						}
						binaryWriter.Write(num3);
						binaryWriter.Write(num4);
						binaryWriter.Write(this.blocks[num].unkBuffer0);
						num++;
					}
				}
			}
		}

		// Token: 0x04000012 RID: 18
		public string filepath;

		// Token: 0x04000013 RID: 19
		public char[] Header;

		// Token: 0x04000014 RID: 20
		public m.Block[] blocks;

		// Token: 0x04000015 RID: 21
		public float[,] points;

		// Token: 0x0200000F RID: 15
		public class Block
		{
			// Token: 0x04000048 RID: 72
			public byte[] name;

			// Token: 0x04000049 RID: 73
			public byte density;

			// Token: 0x0400004A RID: 74
			public byte unkByte0;

			// Token: 0x0400004B RID: 75
			public float waterLevel;

			// Token: 0x0400004C RID: 76
			public m.Block.Cell[] cells;

			// Token: 0x0400004D RID: 77
			public m.Block.CellExtra[] cellsExtra;

			// Token: 0x0400004E RID: 78
			public float heightMax;

			// Token: 0x0400004F RID: 79
			public float heightMin;

			// Token: 0x04000050 RID: 80
			public byte[] unkBuffer0;

			// Token: 0x02000015 RID: 21
			public class Cell
			{
				// Token: 0x0400007D RID: 125
				public int x;

				// Token: 0x0400007E RID: 126
				public int y;

				// Token: 0x0400007F RID: 127
				public float height;

				// Token: 0x04000080 RID: 128
				public ushort texture;

				// Token: 0x04000081 RID: 129
				public byte brightness;
			}

			// Token: 0x02000016 RID: 22
			public class CellExtra
			{
				// Token: 0x04000082 RID: 130
				public byte min;

				// Token: 0x04000083 RID: 131
				public byte max;
			}
		}
	}
}
