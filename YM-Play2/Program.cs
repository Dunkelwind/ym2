using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Gtk;

namespace YM2
{

    class MainClass
    {
        public static Shapes shapes = new Shapes();
        public static Tracks tracks = new Tracks(128);
        public static Instruments instrs = new Instruments();
        public static Sequences sequences = new Sequences();
        public static Interpreter._SND_INFO[] sndInfo;


        public static void Main(string[] args)
        {
            Application.Init();


            loadTFMX ltfmx = new loadTFMX();
            ltfmx.LoadTFMX("/home/dunkelwind/Projects/SOUNDS.DAT/M18.MUS");

            MainWindow win = new MainWindow();
            win.Show();
//            LayoutSample ls = new LayoutSample();
            Application.Run();
        }



    }
}
