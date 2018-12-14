using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YM2
{
    public class Sequences
    {
        const int MAX_SEQ = 256;
        public Sequence[] seqs = new Sequence[257];
        public int n = MAX_SEQ - 1;
        public Sequences()
        {
            int i, j;
            SeqEntry se = new SeqEntry();

           

            se.note = -1;
            se.shape = 0;
            se.xtra = 0;

            for (i = 0; i <= MAX_SEQ - 1 + 1; i++)
            {
                seqs[i] = new Sequence();
                seqs[i].seq = new SeqEntry[256]; // seqs.A
                for (j = 0; j <= 255; j++)
                    seqs[i].seq[j] = se;
            }

            // temporäre Dummy-Sequence

            int temp = MAX_SEQ - 1 + 1;

            for (i = 0; i <= 30; i++)
            {
                seqs[temp].seq[i].note = 0;
                seqs[temp].seq[i].shape = 0;
                seqs[temp].seq[i].xtra = 0;
            }
            i += 1;
            seqs[temp].seq[i].note = 1 * 12;
            seqs[temp].seq[i].shape = 0;
            seqs[temp].seq[i].xtra = 0;
            i += 1;
            seqs[temp].seq[i].note = 0xFF;               // End
            seqs[temp].seq[i].shape = 0;
            seqs[temp].seq[i].xtra = 0;
        }

        public void clear()
        {
            int i, j;
            SeqEntry se = new SeqEntry();

            se.note = -1;
            se.shape = 0;
            se.xtra = 0;

            for (i = 0; i <= MAX_SEQ - 1 + 1; i++)
            {
                for (j = 0; j <= 255; j++)
                    seqs[i].seq[j] = se;
            }
        }
        public int depack(int n, byte[] packed)
        {
            int i = 0;
            int pi = 0;
            byte b;
            short counter = 0;
            short counterInit = 0;

            foreach (SeqEntry se in seqs[n].seq)
            {
                se.note = -1;
                se.shape = 0;
                se.xtra = 0;
            }
            Boolean f = true;
            do {
                counter -= 1;
                if (counter < 0)
                {
                    counter = counterInit;
                    do
                    {
                        b = packed[pi];
                        pi += 1;
                        switch (b)
                        {
                            case 0xFF:
                                {
                                    seqs[n].seq[i].note = 0xFF;
                                    return (i);
                                }

                            case 0xFE:
                                {
                                    counter = packed[pi];
                                    pi += 1;
                                    counterInit = counter;
                                    break;
                                }

                            case 0xFD:
                                {
                                    counter = packed[pi];
                                    pi += 1;
                                    counterInit = counter;
                                    f = false;
                                    break;
                                }

                            default:
                                {
                                    seqs[n].seq[i].note = b;
                                    b = packed[pi];
                                    pi += 1;
                                    seqs[n].seq[i].shape = b;
                                    if ((b & 0xE0) > 0)
                                    {
                                        seqs[n].seq[i].xtra = packed[pi];
                                        pi += 1;
                                    }
                                    f = false;
                                    break;
                                }
                        }
                    } while (f);
                }

                i += 1;
            }
            while (true);// nach &Hfe, &Hxx folgt sofort Note// Pause
        }
    }
}
