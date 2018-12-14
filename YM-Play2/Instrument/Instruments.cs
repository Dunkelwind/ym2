using System;

namespace YM2
{
    public class Instruments
    {
        const int MAX_INSTR = 64;
        public Instrument[] instruments = new Instrument[MAX_INSTR+1];
        public int n;
        public readonly int dummy;

        public Instruments()
        {
            int i;

            for (i = 0; i < MAX_INSTR; i++)
            {
                instruments[i] = new Instrument();
                instruments[i].script[0] = 0xE1;// end
            }                                   // default instr
            instruments[MAX_INSTR] = new Instrument();
            instruments[MAX_INSTR].script[0] = 1;
            instruments[MAX_INSTR].script[1] = 0;
            instruments[MAX_INSTR].script[2] = 0;
            instruments[MAX_INSTR].script[3] = 0;
            instruments[MAX_INSTR].script[4] = 0;
            instruments[MAX_INSTR].script[5] = 0;
            instruments[MAX_INSTR].script[6] = 0;
            instruments[MAX_INSTR].script[7] = 0xE1;
            n = MAX_INSTR - 1;
            dummy = MAX_INSTR;
        }
    }
}
