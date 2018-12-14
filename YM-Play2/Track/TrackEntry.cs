using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YM2
{
    public class TrackEntry
    {
        public Byte seq;
        public short note;
        public Byte instr;
        public Byte cmd;

        public TrackEntry()
        {
            note = 0;
        }
    }
}
