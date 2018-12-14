using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YM2
{
    public class Track
    {
        public TrackEntry[] entries;

        public Track()
        {
            entries = new TrackEntry[1];
        }
        public Track(short size)
        {
            entries = new TrackEntry[size];
            for ( int i = 0; i < size; i++ )
                entries[i] = new TrackEntry();
        }
    }
}
