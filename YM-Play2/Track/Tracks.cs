using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YM2
{
    public class Tracks
    {
        public Track[] track = new Track[3];
        int pMaxRow;

        public Tracks(short size)
        {
            track[0] = new Track(size);
            track[1] = new Track(size);
            track[2] = new Track(size);
        }

        public int MaxRow
        {
            get
            {
                return pMaxRow;
            }
            set
            {
                pMaxRow = value;
            }
        }



        public void set_entry(int nr, int row, byte[] rawData)
        {
            int s16;

            track[nr].entries[row].seq = rawData[0];
            s16 = rawData[1];
            if ((s16 & 0x80) != 0)
                s16 = s16 | 0xFF00;
            track[nr].entries[row].note = (short)s16;
            track[nr].entries[row].instr = rawData[2];
            track[nr].entries[row].cmd = rawData[3];
        }

        public byte GetSeq(short nr, short row)
        {
            return track[nr].entries[row].seq;
        }

        public void SetSeq(short nr, short row, byte v)
        {
            track[nr].entries[row].seq = v;
        }

        public short GetNote(short nr, short row)
        {
            return Convert.ToInt16(track[nr].entries[row].note);
        }

        public void SetNote(short nr, short row, short v)
        {
            track[nr].entries[row].note = v;
        }

        public byte GetInstr(short nr, short row)
        {
            return track[nr].entries[row].instr;
        }

        public void SetInstr(short nr, short row, byte v)
        {
            track[nr].entries[row].instr = v;
        }

        public byte GetCmd(short nr, short row)
        {
            return track[nr].entries[row].cmd;
        }

        public void SetCmd(short nr, short row, byte v)
        {
            track[nr].entries[row].cmd = v;
        }

    }
}
