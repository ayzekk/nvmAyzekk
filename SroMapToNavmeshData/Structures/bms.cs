using System;
using System.IO;

namespace SroMapToNavmeshData.Structures
{
	// Token: 0x02000004 RID: 4
	internal class bms
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00003F1C File Offset: 0x0000211C
		public static bms Load(string path)
		{
			bms bms = new bms();
			bms.filepath = string.Format("{0}\\Data\\{1}", Directory.GetCurrentDirectory(), path);
			bool flag = !File.Exists(bms.filepath);
			if (flag)
			{
				throw new InvalidOperationException(string.Format("File {0} not found.", bms.filepath));
			}
			bms result;
			using (BinaryReader binaryReader = new BinaryReader(File.Open(bms.filepath, FileMode.Open)))
			{
				bms.Header = new string(binaryReader.ReadChars(12));
				bms.pointers = new bms.Pointers();
				bms.pointers.Verticies = binaryReader.ReadUInt32();
				bms.pointers.Bones = binaryReader.ReadUInt32();
				bms.pointers.Faces = binaryReader.ReadUInt32();
				bms.pointers.Unknown3 = binaryReader.ReadUInt32();
				bms.pointers.Unknown4 = binaryReader.ReadUInt32();
				bms.pointers.BoundingBox = binaryReader.ReadUInt32();
				bms.pointers.Gates = binaryReader.ReadUInt32();
				bms.pointers.Collision = binaryReader.ReadUInt32();
				bms.pointers.Unknown8 = binaryReader.ReadUInt32();
				bms.pointers.Unknown9 = binaryReader.ReadUInt32();
				result = bms;
			}
			return result;
		}

		// Token: 0x04000004 RID: 4
		public string filepath;

		// Token: 0x04000005 RID: 5
		public string Header;

		// Token: 0x04000006 RID: 6
		public bms.Pointers pointers;

		// Token: 0x0200000A RID: 10
		public class Pointers
		{
			// Token: 0x04000027 RID: 39
			public uint Verticies;

			// Token: 0x04000028 RID: 40
			public uint Bones;

			// Token: 0x04000029 RID: 41
			public uint Faces;

			// Token: 0x0400002A RID: 42
			public uint Unknown3;

			// Token: 0x0400002B RID: 43
			public uint Unknown4;

			// Token: 0x0400002C RID: 44
			public uint BoundingBox;

			// Token: 0x0400002D RID: 45
			public uint Gates;

			// Token: 0x0400002E RID: 46
			public uint Collision;

			// Token: 0x0400002F RID: 47
			public uint Unknown8;

			// Token: 0x04000030 RID: 48
			public uint Unknown9;
		}
	}
}
