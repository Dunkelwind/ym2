using System;


namespace YM2
{
    public class Interpreter
    {
        public enum Mode
        {
            PlayTrack,
            PlayTrackOnce,
            PlaySeq,
            PlayShape
        }
        enum _fType
        {
            TFMX,
            MMME
        }

        public struct _VOICE_SET
        {
            public short nr;
            public short instr_count;
            public short instr_current;
            public short instr_index;
            public short instr_note;
            public short shape_count;
            public short shape_current;
            public short shape_index;
            public short shape_time;
            public short shape_time_init;
            public byte shape_amplitude;
            public byte seq_shape;
            public byte seq_instr;
            public byte seq_note;
            public short seq_time;
            public short seq_time_init;
            public short seq_current;
            public short seq_index;
            public short track_current;
            public short track_index;
            public short track_count;
            public short track_note;
            public byte track_shape;
            public short track_reduce;
            public byte flags;
            public short delta_f;
            public short mod_f_work;
            public short mod_f_max;
            public byte tune_noise;
            public byte noise_freq;
            public byte triangle_fine;
            public bool triangle_restart;
            public short tfmx_count;
            public int bend_var;
            public short reduce;
            public short div;
            public byte shape_sync;
            public byte sid_mode;
            public short digi_drum;
        }

        [Serializable()]
        public struct _SND_INFO
        {
            public short start;
            public short last;
            public short speed;
        }

        private short track_length;
        private short play_speed;
        private short play_speed_count;

        private Instruments instr;
        private Sequences seq;
        private Shapes shape;
        private Tracks track;

        public _VOICE_SET[] VoiceSet = new _VOICE_SET[3];
        public short trackPos = 0;

        private readonly short[] div = { 3822, 3607, 3405, 3214, 3033, 2863, 2702, 2551, 2407, 2272, 2145, 2024, 1911, 1803, 1702, 1607, 1516, 1431, 1351, 1275, 1203, 1136, 1072, 1012, 955, 901, 851, 803, 758, 715, 675, 637, 601, 568, 536, 506, 477, 450, 425, 401, 379, 357, 337, 318, 300, 284, 268, 253, 238, 225, 212, 200, 189, 178, 168, 159, 150, 142, 134, 126, 119, 112, 106, 100, 94, 89, 84, 79, 75, 71, 67, 63, 59, 56, 53, 50, 47, 44, 42, 39, 37, 35, 33, 31, 29, 28, 26, 25, 23, 22, 21, 19, 18, 17, 16, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


        private readonly byte[] mask = { 0xFE, 0xF7, 0xFD, 0xEF, 0xFB, 0xDF };

        private byte ym_enable, ym_noise_f;

        private _fType ftype;
        public Mode PlayMode;

        public void init(string f, ref Instruments i, ref Sequences se, ref Shapes sh, ref Tracks t)
        {
            switch (f)
            {
                case "TFMX":
                    {
                        ftype = _fType.TFMX;
                        break;
                    }

                case "MMME":
                    {
                        ftype = _fType.MMME;
                        break;
                    }
            }
            instr = i;
            seq = se;
            shape = sh;
            track = t;
        }

        //public void tick(ref WaveGen._YM ym)
        //{
        //    short i;
        //    short ym_div;
        //    byte amplitude;
        //    byte b;

        //    // ym.eshape = &HFF


        //    for (i = 0; i <= 2; i++)
        //    {
        //        VoiceSet[i].triangle_fine = 0xFF;
        //        VoiceSet[i].triangle_restart = false;

        //        work_voice(ref VoiceSet[i], ref ym_div, ref amplitude);
        //        ym.chn(i).coars = ym_div >> 8;
        //        ym.chn(i).fine = ym_div & 0xFF;
        //        ym.chn(i).volume = amplitude;

        //        if (VoiceSet[i].triangle_fine != 0xFF)
        //            ym.efine = VoiceSet[i].triangle_fine;

        //        if (VoiceSet[i].triangle_restart)
        //        {
        //            ym.ecoars = 0;
        //            ym.eshape = 10;      // Triangle
        //            ym.shape.counter = 0;    // Restart
        //        }

        //        // ---SID on Channel A

        //        if (i == 0)
        //        {
        //            b = VoiceSet[0].sid_mode;
        //            switch (b)
        //            {
        //                case 0:
        //                    {
        //                        ym.sidvol = 0xFF;                // SID OFF
        //                        break;
        //                    }

        //                case 4:
        //                    {
        //                        ym.sidvol = amplitude;
        //                        break;
        //                    }

        //                case 1:
        //                case 2:
        //                    {
        //                        ym_div = VoiceSet[b - 1].div;
        //                        if (ym_div <= 16)
        //                            ym.sidvol = 0xFF;                // SID OFF
        //                        else
        //                            ym.sidfreq = ym_div;
        //                        break;
        //                    }
        //            }
        //        }

        //        // --- Digi-Drum on Channel C
        //        if (i == 2)
        //        {
        //            if (VoiceSet[i].digi_drum >= 0)
        //            {
        //                ym.digidrum = VoiceSet[i].digi_drum;
        //                VoiceSet[i].digi_drum = -1;
        //                ym.digipos = 0;
        //            }
        //        }
        //    }
        //    ym.enable = ym_enable;
        //    ym.noisep = ym_noise_f;

        //    if (PlayMode != Mode.PlayShape)
        //    {
        //        play_speed_count -= 1;
        //        if (play_speed_count == 0)
        //        {
        //            play_speed_count = play_speed;
        //            for (i = 0; i <= 2; i++)
        //                sequencer(ref VoiceSet[i]);
        //        }
        //    }
        //}


        public void work_voice(ref _VOICE_SET vs, ref short ym_div, ref byte amplitude)
        {
            byte b;
            short note;
            int bender;
            short f_max, f_work;
            byte flag;
            short i;
            int l1, l2;

        icount:
            ;
            if (vs.instr_count > 0)
                vs.instr_count -= 1;
            else
            {
            next_i:
                ;
                i = vs.instr_index;
                b = instr.instruments[vs.instr_current].script[i];
                i += 1;
                if (b < 0xE0)
                {
                    vs.instr_note = b;
                    vs.instr_index += 1;
                }
                else
                    switch (b)
                    {
                        case 0xE0:   // absolute JMP
                            {
                                vs.instr_index = instr.instruments[vs.instr_current].script[i];
                                goto next_i;
                            }

                        case 0xE1:   // End
                            {
                                vs.instr_note = instr.instruments[vs.instr_current].script[i-2]; // stay on last note
                                break;
                            }

                        case 0xE2: // reset shape index
                            {
                                vs.shape_index = 0;
                                vs.shape_time = 1;
                                vs.instr_index += 1;
                                goto next_i;
                            }

                        case 0xE3: // parameters for f modulation
                            {
                                vs.delta_f = instr.instruments[vs.instr_current].script[i];
                                i += 1;
                                vs.mod_f_max = instr.instruments[vs.instr_current].script[i];
                                i += 1;
                                vs.instr_index += 3;
                                goto next_i;
                            }

                        case 0xE4:   // noise & tune on
                            {
                                vs.tune_noise = 0;
                                vs.noise_freq = instr.instruments[vs.instr_current].script[i];
                                i++;
                                vs.instr_index += 2;
                                goto next_i;
                            }

                        case 0xE5:   // noise on, tune off
                            {
                                vs.tune_noise = 1;
                                vs.instr_index += 1;
                                goto next_i;
                            }

                        case 0xE6:   // noise off, tune on
                            {
                                vs.tune_noise = 2;
                                vs.instr_index += 1;
                                goto next_i;
                            }

                        case 0xE7:   // select instr group
                            {
                                vs.instr_index += 2;
                                // MsgBox("InstrGroup")
                                goto next_i;
                            }

                        case 0xE8: // set instr counter
                            {
                                vs.instr_count = instr.instruments[vs.instr_current].script[i];
                                vs.instr_index += 2;
                                goto icount;
                            }

                        case 0xE9:   // triangle evelope
                            {
                                vs.triangle_fine = instr.instruments[vs.instr_current].script[i];
                                vs.triangle_restart = true;
                                vs.instr_index += 2;
                                goto next_i;
                            }

                        case 0xEA:   // ???
                            {
                                vs.seq_shape = 0x20;
                                vs.seq_instr = instr.instruments[vs.instr_current].script[i];
                                vs.instr_index += 2;
                                goto next_i;
                            }

                        case 0xEB:       // shape vibrato
                            {
                                vs.shape_sync = instr.instruments[vs.instr_current].script[i];
                                i++;
                                vs.instr_index += 2;
                                goto next_i;
                            }

                        case 0xEC:      // digi drum
                            {
                                vs.digi_drum = instr.instruments[vs.instr_current].script[i];
                                vs.tune_noise = 3;   // noise off, tune off
                                vs.instr_index += 4; // 4
                                goto next_i;
                             }

                        case 0xEE:       // sid mode
                            {
                                vs.sid_mode = instr.instruments[vs.instr_current].script[i];
                                i++;
                                vs.instr_index += 2;
                                goto next_i;
                        }
                    }
            }

        // --- Shape handling ---

        scount:
            ;
            if (vs.shape_count > 0)
                vs.shape_count -= 1;
            else
            {
                vs.shape_time -= 1;
                if (vs.shape_time == 0)
                {
                    vs.shape_time = vs.shape_time_init;
                next_s:
                    ;
                    i = vs.shape_index;
                    b = shape.shape_set[vs.shape_current].data[i];           
                    i += 1;
                    if (b < 0xE0)
                    {
                        vs.shape_amplitude = b;
                        vs.shape_index += 1;
                    }
                    else
                        switch (b)
                        {
                            case 0xE0:   // absolute JMP
                                {
                                    i = shape.shape_set[vs.shape_current].data[i];     // dest
                                    vs.shape_index = i;
                                    goto next_s;
                                }

                            case 0xE1:   // end
                                {
                                    vs.shape_amplitude = shape.shape_set[vs.shape_current].data[i-2];  // stay on last amplitude
                                    break;
                                }

                            case object _ when 0xE2 <= b && b <= 0xE7:   // not supported
                                {
                                    return;
                                }

                            case 0xE8:   // set shape_counter
                                {
                                    vs.shape_count = shape.shape_set[vs.shape_current].data[i];
                                    vs.shape_index += 2;
                                    goto scount;
                                }
                        }
                }
            }

            // --- divider for ym freq ---

            note = vs.instr_note;
            if ((note & 0x80) == 0)
                note += (short)(vs.seq_note + vs.track_note);

            note = (short)(note & 0x7F);
            ym_div = div[note];

            // --- Tune/Noise on/off ---

            switch (vs.tune_noise)
            {
                case 0:  // tune & noise on
                    {
                        ym_enable = (byte)(ym_enable & mask[2 * vs.nr]);   // tune on
                        ym_enable = (byte)(ym_enable & mask[2 * vs.nr + 1]); // noise on
                        break;
                    }

                case 1: // tune off, noise on
                    {
                        ym_enable = (byte)(ym_enable | ~mask[2 * vs.nr]);   // tune off
                        ym_enable = (byte)(ym_enable & mask[2 * vs.nr + 1]); // noise on
                        vs.noise_freq = vs.seq_note;
                        note = vs.instr_note;
                        if ((note & 0x80)  > 0)
                            note = (short)(note | 0xFF00);
                        if (note < 0)
                            vs.noise_freq = (byte)(note & 0x7F);
                        else
                            vs.noise_freq += (byte)note;
                        break;
                    }
                case 2: // tune on, noise off
                    {
                        ym_enable = (byte)(ym_enable & mask[2 * vs.nr]);   // tune on
                        ym_enable = (byte)(ym_enable | ~mask[2 * vs.nr + 1]); // noise off
                        break;
                    }
            }

            if (vs.noise_freq != 0)
                ym_noise_f = (byte)((~vs.noise_freq) & 0x1F);

            // --- TFMX ---

            if (ftype == _fType.TFMX)
            {
                if (vs.tfmx_count == 0)
                {
                    flag = vs.flags;

                    f_max = vs.mod_f_max;

                    // Wertebereich mod_f_max: 0...127   entspricht 0...254 in 2er Schritten
                    // 128...255 entspricht 0... 127

                    if ((f_max & 0x80) == 0x80)
                        f_max = (short)(f_max & 0x7F);
                    else
                        f_max *= 2;

                    f_work = vs.mod_f_work;
                    if (((flag & 0x80) == 0x0) | ((flag & 0x81) == 0x80))
                    {
                        if ((flag & 0x20) == 0)
                        {
                            // b5=0, subtract
                            f_work -= vs.delta_f;
                            if (f_work < 0)
                            {
                                f_work = 0;
                                flag = (byte)(flag | 0x20); // b5=1 -> add
                            }
                        }
                        else
                        {
                            // b5=1, add
                            f_work += vs.delta_f;
                            if (f_work > f_max)
                            {
                                f_work = f_max;
                                flag = (byte)(flag & ~0x20); // b5=0 -> sub
                            }
                        }
                        vs.mod_f_work = f_work;
                    }

                    f_work -= (short)(f_max / 2);

                    while (note < 4 * 12)
                    {
                        f_work += f_work;
                        note += 12;
                    }
                    ym_div += f_work;
                    vs.flags = (byte)(flag ^ 1);
                }
                else
                    vs.tfmx_count -= 1;

                // freq-bender

                if ((vs.seq_shape & 0x20) == 0x20)
                {
                    bender = vs.seq_instr;
                    bender *= 4096;
                    if ((bender & 0x80000) > 0)
                        bender = (int)(bender | 0xFFF00000);
                    bender += vs.bend_var;
                    vs.bend_var = bender;
                    ym_div -= (short) (bender / 65536);
                }

                // --- amplitude ---

                i = vs.shape_amplitude;
                i -= vs.track_reduce;
                i -= vs.reduce;
                if (i < 0)
                    i = 0;
                amplitude = (byte)i;
            }
            else
            {
                // --- MMME ---------------------------------------------------------

                if (vs.tfmx_count == 0)
                {
                    flag = vs.flags;
                    f_max = vs.mod_f_max;


                    f_work = vs.mod_f_work;
                    if ((flag & 0x20) == 0x0)
                    {
                        f_work += vs.delta_f;
                        if (f_work > 2 * f_max)
                        {
                            f_work = (short)(2 * f_max);
                            flag = (byte)(flag | 0x20);     // auf Sub umschalten
                        }
                    }
                    else
                    {
                        f_work -= vs.delta_f;
                        if (f_work < 0)
                        {
                            f_work = 0;
                            flag = (byte)(flag & 0xDF);    // auf Add umschalten
                        }
                    }
                    vs.mod_f_work = f_work;
                    vs.flags = flag;

                    l1 = f_work - f_max;             // convert Short to Integer
                    l1 = (l1 * ym_div) >> 10;
                    ym_div += (short)l1;
                }
                else
                    vs.tfmx_count -= 1;


                // --- Bender
                if ((vs.seq_shape & 0x20) == 0x20)
                {
                    b = vs.seq_instr;           // convert unsigned Byte to Integer
                    l2 = b;
                    if ((b & 0x80) > 0)
                        l2 -= 256;
                    vs.bend_var += l2;
                    l1 = ym_div;
                    ym_div = (short) (l1 - ((l1 * vs.bend_var) >> 10));
                }

                // --- amplitude ---

                i = vs.shape_amplitude;
                i -= vs.track_reduce;
                i -= vs.reduce;
                if (i < 0)
                    i = 0;
                amplitude = (byte)i;

                // --- shapeform sync

                b = vs.shape_sync;
                if (b != 0)
                {
                    if ((b & 0x80) > 0)
                        vs.triangle_restart = true;

                    if ((b & 0x2) > 0)
                        ym_div = 0;// Ton aus

                    if ((b & 0x8) > 0)
                        vs.triangle_fine = (byte)(ym_div / 8);
                    else
                        vs.triangle_fine = (byte)(ym_div / 16);
                }
            }
            vs.div = ym_div;
        }

        // sequencer for uncompressed data

        public void sequencer(ref _VOICE_SET vs)
        {
            short j;
            byte b;
            SeqEntry se;


            do
            {
                se = seq.seqs[vs.seq_current].seq[vs.seq_index];
                vs.seq_index += 1;
                switch (se.note)
                {
                    case -1:
                        {
                            return;
                        }

                    case 0xFF:
                        {
                            // --- Track-Interpeter ---

                            if (PlayMode == Mode.PlaySeq)
                            {
                                vs.track_note = 0;
                                vs.track_shape = 0;
                                vs.seq_current = 0;
                                vs.seq_index = 0;
                                //Form1.WaveGen.reset();
                                //Form1.WaveGen.PlayFlag = false;
                            }

                            if (vs.nr == 0)
                            {
                                vs.track_count -= 1;
                                if (vs.track_count < 0)
                                {
                                    if (PlayMode == Mode.PlayTrackOnce)
                                    {
                                        vs.track_note = 0;
                                        vs.track_shape = 0;
                                        vs.seq_current = 0;
                                        vs.seq_index = 0;
                                        //Form1.WaveGen.reset();
                                        //Form1.WaveGen.PlayFlag = false;
                                    }

                                    vs.track_count = track_length;
                                    VoiceSet[0].track_index = 0;
                                    VoiceSet[1].track_index = 0;
                                    VoiceSet[2].track_index = 0;
                                }
                            }

                            j = (short)(vs.track_current + vs.track_index);
                            trackPos = j;
                            vs.track_note = track.GetNote(vs.nr, j);
                            vs.track_shape = track.GetInstr(vs.nr, j);
                            b = track.GetCmd(vs.nr, j);
                            if ((b & 0xF0) == 0xF0)
                                vs.track_reduce = (short)(b & 0xF);
                            else if ((b & 0xE0) == 0xE0)
                                play_speed = (short)(b & 0xF);
                            vs.seq_current = track.GetSeq(vs.nr, j);
                            vs.seq_index = 0;
                            vs.track_index += 1;
                            break;
                        }

                    default:
                        {
                            vs.seq_note = (byte)se.note;
                            b = se.shape;
                            vs.seq_shape = b;
                            if ((b & 0xE0) > 0)
                                vs.seq_instr = se.xtra;
                            vs.bend_var = 0;
                            if ((vs.seq_note & 0x80) == 0)
                            {
                                b = (byte)((vs.seq_shape & 0x1F) + vs.track_shape);
                                vs.shape_time = shape.shape_set[b].para1;
                                vs.shape_time_init = shape.shape_set[b].para1;
                                vs.delta_f = shape.shape_set[b].para3;
                                vs.flags = 0x40;
                                vs.mod_f_work = shape.shape_set[b].para4;
                                vs.mod_f_max = shape.shape_set[b].para4;
                                vs.tfmx_count = shape.shape_set[b].para5;
                                vs.shape_current = b;
                                vs.shape_index = 0;
                                if ((vs.seq_shape & 0x40) > 0)
                                    b = vs.seq_instr;
                                else
                                    b = shape.shape_set[b].para2;
                                vs.instr_current = b;
                                vs.instr_count = 0;
                                vs.instr_index = 0;
                                vs.shape_count = 0;
                            }

                            return;
                        }
                }
            }
            while (true)// nix tun// end of sequence// seq 0 must be always "pause"// seq 0 must be always "pause"// play it again// Tonh�he// hkurve// Instrument
    ;
        }

        public void init_voices(ref _SND_INFO snd_info, Mode pm, short seq)
        {
            short i, j;
            byte b;

            PlayMode = pm;

            track_length = (short)(snd_info.last - snd_info.start);
            play_speed = snd_info.speed;

            for (i = 0; i <= 2; i++)
            {
                VoiceSet[i].instr_current = (short)instr.dummy;                 // Instrument 64
                VoiceSet[i].instr_index = 0;
                VoiceSet[i].shape_current = (short)shape.dummy;
                VoiceSet[i].shape_index = 0;
                VoiceSet[i].seq_note = 0;
                VoiceSet[i].seq_shape = 0;
                VoiceSet[i].instr_note = 0;
                VoiceSet[i].shape_amplitude = 0;
                VoiceSet[i].shape_time_init = 1;
                VoiceSet[i].shape_time = 1;
                VoiceSet[i].shape_count = 0;
                VoiceSet[i].instr_count = 0;
                VoiceSet[i].delta_f = 0;
                VoiceSet[i].mod_f_max = 0;
                VoiceSet[i].mod_f_work = 0;
                VoiceSet[i].tfmx_count = 0;
                VoiceSet[i].seq_instr = 0;
                VoiceSet[i].noise_freq = 0;
                VoiceSet[i].nr = i;

                VoiceSet[i].track_current = snd_info.start;
                VoiceSet[i].track_index = 1;
                VoiceSet[i].track_count = track_length;

                VoiceSet[i].reduce = 0;
                VoiceSet[i].track_reduce = 0;

                VoiceSet[i].seq_index = 0;
                VoiceSet[i].seq_time_init = 0;
                VoiceSet[i].seq_time = 0;

                VoiceSet[i].shape_sync = 0;
                VoiceSet[i].sid_mode = 0;

                VoiceSet[i].digi_drum = -1;

                switch (pm)
                {
                    case Mode.PlayTrack:
                        {
                            j = VoiceSet[i].track_current;
                            trackPos = j;
                            VoiceSet[i].seq_current = track.GetSeq(i, j);
                            VoiceSet[i].track_note = track.GetNote(i, j);
                            VoiceSet[i].track_shape = track.GetInstr(i, j);
                            b = track.GetCmd(i, j);
                            if ((b & 0xF0) == 0xF0)
                                VoiceSet[i].track_reduce = (short)(b & 0xF);
                            break;
                        }

                    case Mode.PlaySeq:
                        {
                            if (i == 0)
                                VoiceSet[i].seq_current = seq;
                            else
                            {
                                VoiceSet[i].seq_current = 0;
                                VoiceSet[i].track_shape = 0;
                            }
                            VoiceSet[i].track_note = 0;
                            VoiceSet[i].track_shape = 0;
                            break;
                        }
                }

                VoiceSet[i].bend_var = 0;
            }
            play_speed_count = 1;
            ym_enable = 0xFF;
        }
    }
}