using System;
using System.Drawing;
using System.IO;

namespace SroMapToNavmeshData.Structures
{
	// Token: 0x02000005 RID: 5
	internal class bsr
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000406C File Offset: 0x0000226C
		public static bsr Load(string path)
		{
			bsr bsr = new bsr();
			bsr.filepath = Path.Combine(new string[]
			{
				string.Format("{0}\\\\Data\\\\{1}", Directory.GetCurrentDirectory(), path)
			});
			bsr.filename = Path.GetFileName(bsr.filepath);
			bool flag = !File.Exists(bsr.filepath);
			if (flag)
			{
				throw new InvalidOperationException(string.Format("File {0} not found.", bsr.filepath));
			}
			bsr result;
			using (BinaryReader binaryReader = new BinaryReader(File.Open(bsr.filepath, FileMode.Open)))
			{
				bsr.Header = new string(binaryReader.ReadChars(12));
				bsr.pointers = new bsr.Pointers();
				bsr.pointers.Material = binaryReader.ReadUInt32();
				bsr.pointers.Mesh = binaryReader.ReadUInt32();
				bsr.pointers.Skeleton = binaryReader.ReadUInt32();
				bsr.pointers.Animation = binaryReader.ReadUInt32();
				bsr.pointers.MeshGroup = binaryReader.ReadUInt32();
				bsr.pointers.AnimationGroup = binaryReader.ReadUInt32();
				bsr.pointers.SoundEffect = binaryReader.ReadUInt32();
				bsr.pointers.BoundingBox = binaryReader.ReadUInt32();
				binaryReader.BaseStream.Position = (long)((ulong)bsr.pointers.BoundingBox);
				bsr.boundingBox = new bsr.BoundingBox();
				uint value = binaryReader.ReadUInt32();
				bsr.boundingBox.RootMesh = new string(binaryReader.ReadChars(Convert.ToInt32(value)));
				bool flag2 = bsr.boundingBox.RootMesh != "";
				if (flag2)
				{
					bsr.Bms = bms.Load(bsr.boundingBox.RootMesh);
				}
				bsr.boundingBox.BoundingBox0[0] = binaryReader.ReadSingle();
				bsr.boundingBox.BoundingBox0[1] = binaryReader.ReadSingle();
				bsr.boundingBox.BoundingBox0[2] = binaryReader.ReadSingle();
				bsr.boundingBox.BoundingBox1[0] = binaryReader.ReadSingle();
				bsr.boundingBox.BoundingBox1[1] = binaryReader.ReadSingle();
				bsr.boundingBox.BoundingBox1[2] = binaryReader.ReadSingle();
				PointF pointF = new PointF(bsr.boundingBox.BoundingBox0[0], bsr.boundingBox.BoundingBox0[2]);
				PointF pointF2 = new PointF(bsr.boundingBox.BoundingBox1[0], bsr.boundingBox.BoundingBox1[2]);
				bsr.box = new RectangleF(Math.Min(pointF.X, pointF2.X), Math.Min(pointF.Y, pointF2.Y), Math.Abs(pointF.X - pointF2.X), Math.Abs(pointF.Y - pointF2.Y));
				binaryReader.BaseStream.Position = (long)((ulong)bsr.pointers.Material);
				result = bsr;
			}
			return result;
		}

		// Token: 0x04000007 RID: 7
		public string filepath;

		// Token: 0x04000008 RID: 8
		public string filename;

		// Token: 0x04000009 RID: 9
		public string Header;

		// Token: 0x0400000A RID: 10
		public bsr.Pointers pointers;

		// Token: 0x0400000B RID: 11
		public bsr.BoundingBox boundingBox;

		// Token: 0x0400000C RID: 12
		public RectangleF box;

		// Token: 0x0400000D RID: 13
		public bms Bms;

		// Token: 0x0200000B RID: 11
		public class Pointers
		{
			// Token: 0x04000031 RID: 49
			public uint Material;

			// Token: 0x04000032 RID: 50
			public uint Mesh;

			// Token: 0x04000033 RID: 51
			public uint Skeleton;

			// Token: 0x04000034 RID: 52
			public uint Animation;

			// Token: 0x04000035 RID: 53
			public uint MeshGroup;

			// Token: 0x04000036 RID: 54
			public uint AnimationGroup;

			// Token: 0x04000037 RID: 55
			public uint SoundEffect;

			// Token: 0x04000038 RID: 56
			public uint BoundingBox;
		}

		// Token: 0x0200000C RID: 12
		public class BoundingBox
		{
			// Token: 0x04000039 RID: 57
			public string RootMesh;

			// Token: 0x0400003A RID: 58
			public float[] BoundingBox0 = new float[3];

			// Token: 0x0400003B RID: 59
			public float[] BoundingBox1 = new float[3];
		}
	}
}
