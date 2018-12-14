using System;
using System.IO;


// imports an Atari TFMX music file
namespace YM2
{
    class AtariTFMX
    {
        private string type1, type2, type3;
        private int[] offsets = new int[7];
        private short[] sizes = new short[14];

        private FileStream fs;
        private BinaryReader br;

        private static UInt32 htonl(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        public static UInt16 htons(UInt16 value)
        {
            return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }


        public void type(ref string t1, ref string t2, ref string t3)
        {
            t1 = type1;
            t2 = type2;
            t3 = type3;
        }

        public void Open(string fname)
        {
            short i;

            fs = new FileStream(fname, FileMode.Open, FileAccess.Read);
            br = new BinaryReader(fs);
            type1 = new string( br.ReadChars(4));
            type2 = "";

            if (type1 == "COSO" | type1 == "MMME")
            {
                for (i = 0; i <= offsets.GetUpperBound(0); i++)
                    offsets[i] = (int)htonl(br.ReadUInt32());

                type2 = new string(br.ReadChars(4));
            }
            for (i = 0; i <= sizes.GetUpperBound(0); i++)
                sizes[i] = (short)htons(br.ReadUInt16());
        }

        public bool LoadShapes(ref Shapes shapes)
        {
            short i, j, s;
            byte b;
            bool f;

            if (sizes[1] > shapes.n)
            {
                return false;
            }

            int[] s_offsets = new int[sizes[1] + 1 + 1];    // sizes(1): Shape-Offsets-1

            switch (type1)
            {
                case "COSO":
                case "MMME":
                    {
                        readOffsets(offsets[1], ref s_offsets, sizes[1]);
                        s_offsets[sizes[1] + 1] = offsets[2]; // end of shapes is start of sequences
                        break;
                    }

                case "TFMX":
                    {
                        fs.Position = 32 + 64 * (sizes[0] + 1);

                        for (i = 0; i <= sizes[1] + 1; i++)   // shapes always 64 Bytes long
                            s_offsets[i] = i * 64;
                        break;
                    }
            }

            for (i = 0; i <= sizes[1]; i++)
            {
                shapes.shape_set[i].para1 = br.ReadByte();
                shapes.shape_set[i].para2 = br.ReadByte();
                shapes.shape_set[i].para3 = br.ReadByte();
                shapes.shape_set[i].para4 = br.ReadByte();
                shapes.shape_set[i].para5 = br.ReadByte();
                s = 0;   // data size
                f = true;
                for (j = 0; j <= s_offsets[i + 1] - (s_offsets[i] + 5) - 1; j++)
                {
                    b = br.ReadByte();
                    shapes.shape_set[i].data[i] = b;
                    if (f & b < 0xE0)
                        s += 1;
                    else
                        f = false;
                }
                shapes.shape_set[i].size = s;
                shapes.shape_set[i].name = "Shape " + string.Format("{0:00}", i);
            }

            return true;
        }

        public bool LoadInstr(ref Instruments instrs)
        {
            short i, j;

            if (sizes[0] > instrs.n)
                return false;
            int[] i_offsets = new int[sizes[0] + 1 + 1];     // Instruments-Offsets

            switch (type1)
            {
                case "COSO":
                case "MMME":
                    {
                        readOffsets(offsets[0], ref i_offsets, sizes[0]);
                        i_offsets[sizes[0] + 1] = offsets[1]; // end of instruments is start of shapes
                        break;
                    }

                case "TFMX":
                    {
                        fs.Position = 32;

                        for (i = 0; i <= sizes[0]; i++)   // instr always 64 Bytes long
                            i_offsets[i] = i * 64;
                        break;
                    }
            }

            for (i = 0; i <= sizes[0]; i++)
            {
                for (j = 0; j <= i_offsets[i + 1] - i_offsets[i] - 1; j++)
                    instrs.instruments[i].script[j] = br.ReadByte();
            }
            return true;
        }

        public bool LoadSeq(ref Sequences seqs)
        {
            short i, j;
            int size;
            byte b;
            byte[] buffer = new byte[256];
            short m16_32;

            if (sizes[2] > seqs.n)
                return false;
            int[] s_offsets = new int[sizes[2] + 1 + 1];    // seq-Offsets

            seqs.clear();

            switch (type1)
            {
                case "COSO":
                case "MMME":
                    {
                        readOffsets(offsets[2], ref s_offsets, sizes[2]);
                        s_offsets[sizes[2] + 1] = offsets[3]; // end of last seq is start of tracks
                        break;
                    }

                case "TFMX":
                    {
                        fs.Position = 32 + 64 * (sizes[0] + 1) + 64 * (sizes[1] + 1);
                        for (i = 0; i <= sizes[2] + 1; i++)   // seqs always 64 Bytes long
                            s_offsets[i] = i * 64;
                        break;
                    }
            }

            for (i = 0; i <= sizes[2]; i++)
            {
                for (j = 0; j <= s_offsets[i + 1] - s_offsets[i] - 1; j++)
                {
                    b = br.ReadByte();
                    // seqs.Seq(i, j) = b
                    buffer[j] = b;
                }
                seqs.depack(i, buffer);
            }

            // If type1 = "TFMX" Then
            // seqs.compress(sizes(2))
            // End If

            return true;
        }

        private void readOffsets(int pos, ref int[] o, short max)
        {
            short i;
            ushort m16_32;

            fs.Position = pos;
            m16_32 = htons(br.ReadUInt16());
            fs.Position = pos;

            if (m16_32 == 0)
            {
                for (i = 0; i <= max; i++)
                    o[i] = (int) htonl(br.ReadUInt32());
            }
            else
                for (i = 0; i <= max; i++)
                    o[i] = htons(br.ReadUInt16());
        }

        public void LoadTracks(ref Tracks tracks)
        {
            int n=0, i, j, k;
            byte[] d = new byte[4];

            switch (type1)
            {
                case "COSO":
                case "MMME":
                    {
                        fs.Position = offsets[3];
                        n = (offsets[4] - offsets[3]) / (3 * 4);
                        break;
                    }

                case "TFMX":
                    {
                        fs.Position = 32 + (64 * (sizes[0] + 1 + sizes[1] + 1 + sizes[2] + 1));
                        n = sizes[3];
                        break;
                    }
            }

            for (i = 0; i <= n - 1; i++)
            {
                for (j = 0; j <= 2; j++)
                {
                    for (k = 0; k <= 3; k++)
                        d[k] = br.ReadByte();
                    tracks.set_entry(j, i, d);
                }
            }
            tracks.MaxRow = n - 1;
        }

        public void LoadSndInfo(ref Interpreter._SND_INFO[] si)
        {
            int n, i;
            int d;

            switch (type1)
            {
                case "COSO":
                case "MMME":
                    {
                        fs.Position = offsets[4];                    // Soundinfo Table
                        if (offsets[5] == 0)
                            n = (int)(fs.Length - offsets[4]);
                        else
                            n = (offsets[5] - offsets[4]);

                        d = (int)(fs.Length - offsets[4]);

                        if (d < n)
                        {
//                            Interaction.MsgBox("Missing " + Strings.Format(n - d, "0") + " Bytes");
                            n = d;
                        }

                        n = n / (3 * 2) - 1;
                        si = new Interpreter._SND_INFO[n + 1];
                        for (i = 0; i <= n; i++)
                        {
                            si[i].start = (short)htons((ushort)br.ReadInt16());
                            si[i].last = (short)htons((ushort)br.ReadInt16());
                            si[i].speed = (short)htons((ushort)br.ReadInt16());
                        }

                        break;
                    }

                case "TFMX":
                    {
                        si = new Interpreter._SND_INFO[1];
                        si[0].start = 0;
                        si[0].last = 100;
                        si[0].speed = 3;
                        break;
                    }
            }
        }


        public void close()
        {
            br.Close();
        }


        // Destructor

        ~AtariTFMX()
        {
//            if (!Information.IsNothing(br))
//                br.Close();
//            base.Finalize();
        }
    }
}
