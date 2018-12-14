using System;
using Gtk;
using Cairo;
using YM2;


public partial class MainWindow : Gtk.Window
{
    FontExtents fe;
    int y_mid;
    int DisplayLines;
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {

        Build();
        //this.DrawingTrack.ExposeEvent += OnDrawingTrackExposeEvent;
        Context g = Gdk.CairoHelper.Create(this.GdkWindow);
        g.SelectFontFace("DejaVu Sans Mono", FontSlant.Normal, FontWeight.Normal);
        g.SetFontSize(14);
        fe = g.FontExtents;
        g.Dispose();
        this.DoubleBuffered = true;
        this.DrawingTrack.HeightRequest = (int)(MainClass.tracks.MaxRow * fe.Height);

        this.vscrollbarTrack.Adjustment.Lower = 0;
        this.vscrollbarTrack.Adjustment.Value = 0;
        this.vscrollbarTrack.Adjustment.Upper = (MainClass.tracks.MaxRow+1.0);

        DisplayLines = (int)((double)this.drawingarea1.HeightRequest / fe.Height +1.0);
        if ((DisplayLines & 1)==0)
            DisplayLines++;             // muss ungerade sein
        this.drawingarea1.HeightRequest = DisplayLines * (int)fe.Height;
        Console.WriteLine(DisplayLines);

        y_mid = this.drawingarea1.HeightRequest / 2;
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnAboutActionActivated(object sender, EventArgs e)
    {
        Gtk.AboutDialog about = new Gtk.AboutDialog
        {
            ProgramName = "Your Music",
            Version = "0.1",
            Copyright = "(c) Dunkelwind"
        };
        about.Run();
        about.Destroy();
    }
    static void DrawCurvedRectangle(Cairo.Context gr, double x, double y, double width, double height)
    {
        gr.Save();
        gr.MoveTo(x, y + height / 2);
        gr.CurveTo(x, y, x, y, x + width / 2, y);
        gr.CurveTo(x + width, y, x + width, y, x + width, y + height / 2);
        gr.CurveTo(x + width, y + height, x + width, y + height, x + width / 2, y + height);
        gr.CurveTo(x, y + height, x, y + height, x, y + height / 2);
        gr.Restore();
    }

    protected void OnDrawingTrackExposeEvent(object o, ExposeEventArgs args)
    {
        using (Context g = Gdk.CairoHelper.Create(args.Event.Window))
        {
            //DrawCurvedRectangle(g, 30, 30, 40, 40);
            //g.SetSourceColor( new Color(0.1, 0.6, 1, 1));
            //g.FillPreserve();
            //g.SetSourceColor( new Color(0.2, 0.8, 1, 1));
            //g.LineWidth = 5;
            //g.Stroke();
            short i=0, k=0;

            g.SelectFontFace("DejaVu Sans Mono", FontSlant.Normal, FontWeight.Normal);
            g.SetFontSize(14);

            for (k = 0; k < MainClass.tracks.MaxRow; k++)
            {

                string s = "";
                s += string.Format("{0,2} ", MainClass.tracks.GetSeq(i, k));
                s += string.Format("{0,3} ", MainClass.tracks.GetNote(i, k));
                s += string.Format("{0:X2} ", MainClass.tracks.GetInstr(i, k));
                s += string.Format("{0:X2} ", MainClass.tracks.GetCmd(i, k));

                TextExtents te = g.TextExtents(s);
                g.MoveTo(0.5 - te.XBearing+100-te.Width , 0.5 - te.YBearing + y_mid + fe.Height * k);
                g.ShowText(s);
            }
            g.MoveTo(0, y_mid + this.TrackScrolledWindow.Vadjustment.Value);
            g.LineTo(100, y_mid + this.TrackScrolledWindow.Vadjustment.Value);
            g.Stroke();





            g.Dispose();
        }
    }

    protected void OnDrawingTrackDragBegin(object o, DragBeginArgs args)
    {
    }

    protected void OnDrawingarea1ExposeEvent(object o, ExposeEventArgs args)
    {
        DrawTrack();
    }

    protected void OnVscrollbarTrackValueChanged(object sender, EventArgs e)
    {
        DrawTrack();
    }

    void DrawTrack( )
    {
        drawingarea1.GdkWindow.Clear();                     // clear Background
        using (Context g = Gdk.CairoHelper.Create(this.drawingarea1.GdkWindow))
        {
            short i = 0, curRow, k = 0;

            int y_print;

            g.SelectFontFace("DejaVu Sans Mono", FontSlant.Normal, FontWeight.Normal);
            g.SetFontSize(14);






            curRow = (short)vscrollbarTrack.Adjustment.Value;
            int upper = DisplayLines / 2;
            y_print = upper * (int)fe.Height;

            int upper2 = upper;
            upper2 -= curRow;
            if (upper2 < 0)             
            {
                y_print = 0;
                curRow -= (short)upper;                 // beginnt am oberen Fensterrand
            }
            else
            {
                y_print -= (int)fe.Height * (curRow);
                curRow = 0;
            }

            //            Console.WriteLine(j);

            for (k = 0; curRow <= MainClass.tracks.MaxRow; k++)
            {

                string s = "";
                s += string.Format("{0,3}:   ", curRow);                    // Zeilennummer
                for (i = 0; i < 3; i++)
                {
                    s += string.Format("{0,2} ", MainClass.tracks.GetSeq(i, curRow));
                    s += string.Format("{0,3} ", MainClass.tracks.GetNote(i, curRow));
                    s += string.Format("{0:X2} ", MainClass.tracks.GetInstr(i, curRow));
                    s += string.Format("{0:X2}    ", MainClass.tracks.GetCmd(i, curRow));
                }
                curRow++;
                TextExtents te = g.TextExtents(s);
                g.MoveTo(0.5 - te.XBearing + te.XAdvance - te.Width, 0.5 - te.YBearing + y_print + fe.Height * k);
                Console.WriteLine(te.Width  + " " + te.XAdvance);
                g.ShowText(s);
            }
            //g.MoveTo(0, y_start);
            //g.LineTo(this.drawingarea1.WidthRequest, y_start);
            g.Rectangle(0, upper * (int)fe.Height, this.drawingarea1.WidthRequest, fe.Height);
            g.SetSourceColor(new Color(1, 0, 0, 0.10));
            g.Fill();

            g.Stroke();

            g.Dispose();
        }

    }
}
