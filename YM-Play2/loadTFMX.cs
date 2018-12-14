using System;
namespace YM2
{
    public class loadTFMX
    {




        public loadTFMX()
        {
        }

        public void LoadTFMX( string fname)
        {
            AtariTFMX atariTFMX = new AtariTFMX();


            string t1 = "", t2 = "", t3 = "";

            atariTFMX.Open(fname);
            atariTFMX.LoadShapes(ref MainClass.shapes);
            atariTFMX.LoadTracks(ref MainClass.tracks);
            atariTFMX.LoadInstr(ref MainClass.instrs);
            atariTFMX.LoadSeq(ref MainClass.sequences);
            atariTFMX.LoadSndInfo(ref MainClass.sndInfo);


            atariTFMX.type(ref t1, ref t2, ref t3);
            atariTFMX.close();

            //            interpreter.init(t2, instrs, sequences, shapes, tracks);
            //            interpreter.init_voices(sndInfo(0), interpreter.Mode.PlayTrack, 0);


        }
    }
}
