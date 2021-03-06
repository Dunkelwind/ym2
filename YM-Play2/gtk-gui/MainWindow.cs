
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;

	private global::Gtk.Action OpenAction;

	private global::Gtk.Action InfoAction;

	private global::Gtk.Action DateiAction;

	private global::Gtk.Action InfoAction1;

	private global::Gtk.Action AboutAction;

	private global::Gtk.Fixed fixed1;

	private global::Gtk.MenuBar menubar3;

	private global::Gtk.ScrolledWindow TrackScrolledWindow;

	private global::Gtk.DrawingArea DrawingTrack;

	private global::Gtk.DrawingArea drawingarea1;

	private global::Gtk.VScrollbar vscrollbarTrack;

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup("Default");
		this.OpenAction = new global::Gtk.Action("OpenAction", global::Mono.Unix.Catalog.GetString("Open"), null, null);
		this.OpenAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Open");
		w1.Add(this.OpenAction, null);
		this.InfoAction = new global::Gtk.Action("InfoAction", global::Mono.Unix.Catalog.GetString("Info"), null, null);
		this.InfoAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Info");
		w1.Add(this.InfoAction, null);
		this.DateiAction = new global::Gtk.Action("DateiAction", global::Mono.Unix.Catalog.GetString("Datei"), null, null);
		this.DateiAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Datei");
		w1.Add(this.DateiAction, null);
		this.InfoAction1 = new global::Gtk.Action("InfoAction1", global::Mono.Unix.Catalog.GetString("Info"), null, null);
		this.InfoAction1.ShortLabel = global::Mono.Unix.Catalog.GetString("Info");
		w1.Add(this.InfoAction1, null);
		this.AboutAction = new global::Gtk.Action("AboutAction", global::Mono.Unix.Catalog.GetString("About"), null, null);
		this.AboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString("About");
		w1.Add(this.AboutAction, null);
		this.UIManager.InsertActionGroup(w1, 0);
		this.AddAccelGroup(this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.fixed1 = new global::Gtk.Fixed();
		this.fixed1.Name = "fixed1";
		this.fixed1.HasWindow = false;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.UIManager.AddUiFromString("<ui><menubar name=\'menubar3\'><menu name=\'DateiAction\' action=\'DateiAction\'/><menu" +
				" name=\'InfoAction1\' action=\'InfoAction1\'><menuitem name=\'AboutAction\' action=\'Ab" +
				"outAction\'/></menu></menubar></ui>");
		this.menubar3 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget("/menubar3")));
		this.menubar3.Name = "menubar3";
		this.fixed1.Add(this.menubar3);
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.TrackScrolledWindow = new global::Gtk.ScrolledWindow();
		this.TrackScrolledWindow.WidthRequest = 702;
		this.TrackScrolledWindow.HeightRequest = 227;
		this.TrackScrolledWindow.Name = "TrackScrolledWindow";
		this.TrackScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child TrackScrolledWindow.Gtk.Container+ContainerChild
		global::Gtk.Viewport w3 = new global::Gtk.Viewport();
		w3.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child GtkViewport.Gtk.Container+ContainerChild
		this.DrawingTrack = new global::Gtk.DrawingArea();
		this.DrawingTrack.WidthRequest = 1000;
		this.DrawingTrack.HeightRequest = 262;
		this.DrawingTrack.Events = ((global::Gdk.EventMask)(2));
		this.DrawingTrack.ExtensionEvents = ((global::Gdk.ExtensionMode)(1));
		this.DrawingTrack.Name = "DrawingTrack";
		w3.Add(this.DrawingTrack);
		this.TrackScrolledWindow.Add(w3);
		this.fixed1.Add(this.TrackScrolledWindow);
		global::Gtk.Fixed.FixedChild w6 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.TrackScrolledWindow]));
		w6.X = 28;
		w6.Y = 52;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.drawingarea1 = new global::Gtk.DrawingArea();
		this.drawingarea1.WidthRequest = 782;
		this.drawingarea1.HeightRequest = 218;
		this.drawingarea1.Name = "drawingarea1";
		this.fixed1.Add(this.drawingarea1);
		global::Gtk.Fixed.FixedChild w7 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.drawingarea1]));
		w7.X = 26;
		w7.Y = 315;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.vscrollbarTrack = new global::Gtk.VScrollbar(null);
		this.vscrollbarTrack.HeightRequest = 209;
		this.vscrollbarTrack.Name = "vscrollbarTrack";
		this.vscrollbarTrack.Adjustment.Upper = 100D;
		this.vscrollbarTrack.Adjustment.PageIncrement = 1D;
		this.vscrollbarTrack.Adjustment.PageSize = 1D;
		this.vscrollbarTrack.Adjustment.StepIncrement = 1D;
		this.fixed1.Add(this.vscrollbarTrack);
		global::Gtk.Fixed.FixedChild w8 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.vscrollbarTrack]));
		w8.X = 831;
		w8.Y = 339;
		this.Add(this.fixed1);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.DefaultWidth = 1131;
		this.DefaultHeight = 582;
		this.Show();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
		this.AboutAction.Activated += new global::System.EventHandler(this.OnAboutActionActivated);
		this.DrawingTrack.ExposeEvent += new global::Gtk.ExposeEventHandler(this.OnDrawingTrackExposeEvent);
		this.DrawingTrack.DragBegin += new global::Gtk.DragBeginHandler(this.OnDrawingTrackDragBegin);
		this.drawingarea1.ExposeEvent += new global::Gtk.ExposeEventHandler(this.OnDrawingarea1ExposeEvent);
		this.vscrollbarTrack.ValueChanged += new global::System.EventHandler(this.OnVscrollbarTrackValueChanged);
	}
}
