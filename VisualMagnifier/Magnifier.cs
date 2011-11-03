using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using WobbrockLib;
using WobbrockLib.Devices;

namespace PointingMagnifier
{
    public partial class Magnifier : Form
    {
        #region Fields
        //Magnifier
        private int _radius;                    // Width of magnifier in non-magnified state
        private int _scale;                     // How much the magnifier will enlarge
        private double _opacity;                // Stores the opacity of the lens (unmagnified)
        private Bitmap _bmp;                    // Scraped screenshot of the magnified area

        //Magnifier State
        private bool _active;                   // True if the magnifier is currently in use
        private bool _magnified;
        private bool _clickthrough;             // If true, the magnifier will only handle low level mouse events
        private Point _transpose;               // A vector storing the distance the center of the magnifier is offset when
        //it is magnified so that the whole lens will be drawn within the screen bounds.
        private Point _center;                  // The center of the unmagnified lens
        //private LowLevelMouseHook mHook;        // A global low level mouse hook for hooking mouse events when the magnifier does not have focus.

        private RawInputMouse _rim;
        private bool _lowerMouseGain;           // If true, on magnification, the mouse gain will drop.
        private bool _crosshairs;               // If true, crosshairs will be drawn in the center of the non-magnified state.

        //Mouse State
        private int _mouseDown;                 // Each time a mouse button is pressed, this is incremented. 
        private bool _synthMouseDown;             // A flag to let the mouse hook know that an event is synthesized.
        private bool _synthMouseUp;             // A flag to let the mouse hook know that an event is synthesized.
        private MouseButtons _pressedButton;    // The current Mouse Button which has been pressed.
        private Point _mouseDownPt;             // The point at which the mouse down occured.
        private bool _dragging;                 // Flag to enable a drag state.
        private System.Windows.Forms.Timer _dblclkTimer; // This timer is enabled after a click, allowing user interaction within the system-set double click time.
        private CursorWrapper _cursor;       // A wrapper class to control the visibility of the actual mouse cursor.

        //Animation
        private const int AnimationStep = 30;   //time in ms for each frame in animation
        private const double AnimationDuration = 100.0; //total time that the magnifier takes to animate
        private System.Windows.Forms.Timer _tAnimation; //timer used for animation
        private long _startTime;                //the time in ms which at which the animation begins 

        private Logger _logger;

        public event EventHandler Idle;
        private System.Timers.Timer _idleTimeout;
        private static int IDLELENGTH = 300000; //length of time before cursor goes idle ~5 minutes
        #endregion

        #region Contructor
        /// <summary>
        /// Pointing Magnifier replaces mouse cursor with an area cursor, which magnifies the underlying area on click.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public Magnifier(ref Preferences p, ref Logger l, ref CursorWrapper c)
        {
            InitializeComponent();

            _logger = l;

            //set default values
            _radius = p.Size;
            _scale = p.Magnification;
            _opacity = p.Transparency;
            _cursor = c;
            _lowerMouseGain = false;
            _crosshairs = true;

            _rim = new RawInputMouse("Raw Input Mouse");
            _rim.OnMouseMove += new EventHandler<MouseEventArgs>(OnRawInputMouseMove);
            _rim.OnMouseDown += new EventHandler<MouseEventArgs>(OnRawInputMouseDown);
            _rim.OnMouseUp += new EventHandler<MouseEventArgs>(OnRawInputMouseUp);
            _rim.OnMouseWheel += new EventHandler<MouseEventArgs>(OnRawInputMouseWheel);

            //Event Handlers
            //mHook = new LowLevelMouseHook("mouse");
            //mHook.OnMouseUp += new EventHandler<MouseEventArgs>(mHook_OnMouseUp);
            //mHook.OnMouseMove += new EventHandler<MouseEventArgs>(mHook_OnMouseMove);
            //mHook.OnMouseWheel += new EventHandler<MouseEventArgs>(mHook_OnMouseWheel);
            //mHook.OnMouseDown += new EventHandler<MouseEventArgs>(mHook_OnMouseDown); //strictly for mouse down

            //mHook.Install();
            this.MouseDown += new MouseEventHandler(PointingMagnifier_MouseDown);
            this.Paint += new PaintEventHandler(PointingMagnifier_Paint);

            //Timers
            _dblclkTimer = new System.Windows.Forms.Timer();
            _dblclkTimer.Interval = Win32.GetDoubleClickTime() + 10;
            _dblclkTimer.Tick += new EventHandler(_dblclkTimer_Tick);
            _dblclkTimer.Enabled = false;

            _tAnimation = new System.Windows.Forms.Timer();
            _tAnimation.Interval = AnimationStep;
            _tAnimation.Tick += new EventHandler(_tAnimation_Tick);
            _tAnimation.Enabled = false;

            _idleTimeout = new System.Timers.Timer(IDLELENGTH);
            _idleTimeout.AutoReset = false;
            _idleTimeout.Elapsed += new System.Timers.ElapsedEventHandler(_idleTimeout_Elapsed);
            _idleTimeout.Enabled = true;

            
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Win32.WM.INPUT)
            {
                bool processed = _rim.ProcessRawInput(m.LParam); // mouse messages
            }
            base.WndProc(ref m);
        }
        private void _idleTimeout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Idle != null)
                Idle(this, EventArgs.Empty);
        }
        #endregion

        #region Form Events

        /// <summary>
        /// Makes sure everything is set up when the Pointing Magnifier is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointingMagnifier_Load(object sender, EventArgs e)
        {
            bool success = _rim.Register(this.Handle, true);

            //ScrapePixels();
            this.Bounds = new Rectangle(this.Location, new Size(Diameter, Diameter));
            SetPath();
            ResetState(); //set state to defaults
        }

        /// <summary>
        /// On closing the Pointing Magnifier, release the mouse hook and set the cursor to visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointingMagnifier_FormClosing(object sender, FormClosingEventArgs e)
        {
            _rim.Unregister();
            _idleTimeout.Dispose();
            _tAnimation.Dispose();
            _dblclkTimer.Dispose();
            //mHook.Uninstall();
            _cursor.Visible = true;
        }

        /// <summary>
        /// When magnified, draw the underlying image so that it is stretched to fill the magnified space.
        /// Else paint crosshairs if they are enabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointingMagnifier_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                //RectangleF r = g.ClipBounds;

                Point pt = PointToClient(Center);

                Rectangle r = new Rectangle(pt.X - Radius, pt.Y - Radius, Radius * 2, Radius * 2);

                //RectangleF r = this.Region.GetBounds(g);
                if (Magnified)
                {
                    this.Opacity = 1D;
                    g.DrawImage(_bmp, r);
                    Pen p = new Pen(Brushes.Gray, 2);
                    g.DrawEllipse(p, r);
                    p.Dispose();
                }
                else
                {
                    if (!Clickthrough && _crosshairs)
                    {
                        g.DrawLine(Pens.Black, Radius, Radius - 10, Radius, Radius + 10);
                        g.DrawLine(Pens.Black, Radius - 10, Radius, Radius + 10, Radius);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region Animation
        /// <summary>
        /// Updates the region of the painting magnifier. While magnifying, this region will animate outwards 
        /// from the center.
        /// </summary>
        private void SetPath()
        {
            long elapsed = WobbrockLib.Extensions.TimeEx.NowMs - _startTime;
            GraphicsPath p = new GraphicsPath();
            if (_tAnimation.Enabled)
            {
                int r = Radius;
                if (elapsed < AnimationDuration)
                {
                    r = GetAnimatedRadius(elapsed);
                }
                else
                {
                    _tAnimation.Enabled = false;
                }
                Point pt = PointToClient(Center);
                p.AddEllipse(pt.X - r, pt.Y - r, r * 2, r * 2);
            }
            else
            {
                p.AddEllipse(0, 0, Diameter, Diameter);
            }
            this.Region = new Region(p);
            p.Dispose();
        }


        /// <summary>
        /// On tick, will update the drawing region.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tAnimation_Tick(object sender, EventArgs e)
        {
            SetPath();
        }
        /// <summary>
        /// Gets one or more target area regions to draw to be used in animating the
        /// metronome visualization for this trial's target. The animations will be
        /// drawn after the targets are drawn, i.e., on top of them. Because they are 
        /// GDI+ resources, the regions should be disposed of after being used.
        /// </summary>
        /// <param name="elapsed">The milliseconds elapsed since the last metronome 'tick'.
        /// This should be less than or equal to the interval passed into the trial's
        /// constructor.</param>
        /// <returns>The radius of the magnifier to draw in an animation sequence.</returns>
        public int GetAnimatedRadius(long elapsed)
        {
            double i = 1;

            i = (elapsed / AnimationDuration) * 100.0;

            i = (i > 100) ? 100 : i;
            i = (i < 0) ? 0 : i;

            double pct = WobbrockLib.Extensions.AnimationEx.SlowInPacer((int)i, 100);
            int aniRadius = (int)((Radius - RadiusNM) * pct);
            return (RadiusNM + aniRadius);
        }
        #endregion

        #region Properties

        public bool isIdle
        {
            get { return !_idleTimeout.Enabled; }
        }
        /// <summary>
        /// Gets or Sets the center of the Pointing Magnifier. When set, the location of the form is updated as well.
        /// </summary>
        public Point Center
        {
            get { return _center; }
            set
            {
                _center = value;
                this.Location = new Point(_center.X - Radius, _center.Y - Radius);
            }
        }

        /// <summary>
        /// Gets the current radius.
        /// </summary>
        public int Radius
        {
            get { return _radius * (_magnified ? MagnificationFactor : 1); }
        }

        /// <summary>
        /// Gets or Sets the non-magnified radius. When set, the location and size of the form is updated as well.
        /// </summary>
        public int RadiusNM
        {
            get { return _radius; }
            set
            {
                _radius = value;
                Center = Cursor.Position;
                this.Bounds = new Rectangle(this.Location, new Size(Diameter, Diameter));
                SetPath();
                _logger.Preferences("Radius", _radius.ToString());
            }
        }

        /// <summary>
        /// Gets the current diameter.
        /// </summary>
        public int Diameter
        {
            get
            {
                return Radius * 2;
            }
        }

        /// <summary>
        /// Gets or Sets the amount that the area cursor will magnify.
        /// </summary>
        public int MagnificationFactor
        {
            get { return _scale; }
            set { 
                _scale = value;
                _logger.Preferences("MagnificationFactor", _scale.ToString());
            }
        }

        /// <summary>
        /// Gets or Sets the bitmap of the screen underneath the area cursor.
        /// </summary>
        public Bitmap Image
        {
            get { return _bmp; }
            set { _bmp = value; }
        }

        /// <summary>
        /// Gets or Sets the transparency level.
        /// </summary>
        public double Transparency
        {
            get { return _opacity; }
            set { 
                _opacity = value;
                _logger.Preferences("Transparency", _opacity.ToString());
            }
        }

        /// <summary>
        /// Gets or Sets whether the mouse gain will drop when magnified.
        /// </summary>
        public bool LowerMouseGain
        {
            get { return _lowerMouseGain; }
            set { 
                _lowerMouseGain = value;
                _logger.Preferences("LowerMouseGain", _lowerMouseGain.ToString());
            }
        }

        /// <summary>
        /// Gets or Sets whether crosshairs will be drawn in the center of the area cursor.
        /// </summary>
        public bool Crosshairs
        {
            get { return _crosshairs; }
            set { 
                _crosshairs = value;
                _logger.Preferences("Crosshairs",_crosshairs.ToString());
            }

        }
        #endregion

        #region Magnifier State

        /// <summary>
        /// Gets or Sets whether the magnifier is active.
        /// </summary>
        public bool Active
        {
            get { return _active; }
            set
            {
                this.Visible = value;
                _active = value;
                _logger.ChangeState(_active);
                _cursor.Visible = !_active; //When magnifier is active, cursor should not be visible
                ResetState();
            }
        }

        public bool ActiveNSC
        {
            set
            {
                this.Visible = value;
                _active = value;
                _logger.ChangeState(_active);
                _cursor.Visible = !_active; //When magnifier is active, cursor should not be visible
            }
        }

        /// <summary>
        /// Gets whether the Pointing Magnifier is currently magnified.
        /// </summary>
        public bool Magnified
        {
            get { return _magnified; }
        }

        /// <summary>
        /// Gets or Sets whether or not the Pointing Magnifier captures mouse events.
        /// If true the magnifier will pass clicks through to underlying windows, else 
        /// mouse events will be captured by the magnifier.
        /// </summary>
        public bool Clickthrough
        {
            get { return _clickthrough; }
            set
            {
                _clickthrough = value;
                if (Active)
                {
                    if (_clickthrough)
                    {
                        _logger.ClickThrough(true);
                        _cursor.Visible = true;
                        // When clickthrough is enabled, set transparancy to 1/3 the defined value.
                        this.Opacity = Transparency / 3;

                        // Set window style to transparent. While it will still be visible, mouse events will be passed through.
                        int exStyle = Win32.GetWindowLong(this.Handle, (int)Win32.GWL.EXSTYLE);
                        Win32.SetWindowLong(this.Handle, (int)Win32.GWL.EXSTYLE, exStyle | (int)Win32.WS.EX_TRANSPARENT);

                        // Find window directly below the mouse (no longer the magnifier) and make it the foreground, active, and focused window. 
                        // Also give the window mouse capture as well.
                        Win32.POINT p = new Win32.POINT();
                        p.x = Cursor.Position.X;
                        p.y = Cursor.Position.Y;
                        IntPtr hWnd = Win32.WindowFromPoint(p);
                        Win32.SetForegroundWindow(hWnd);
                        Win32.SetActiveWindow(hWnd);
                        Win32.SetFocus(hWnd);
                        Win32.SetCapture(hWnd);
                    }
                    else
                    {
                        _logger.ClickThrough(false);
                        _cursor.Visible = false;
                        this.Opacity = Transparency;

                        // Set the window style to not transparent.
                        int exStyle = Win32.GetWindowLong(this.Handle, (int)Win32.GWL.EXSTYLE);
                        Win32.SetWindowLong(this.Handle, (int)Win32.GWL.EXSTYLE, exStyle & ~((int)Win32.WS.EX_TRANSPARENT));
                    }
                }
            }
        }

        /// <summary>
        /// Resets all properties of the Pointing Magnifier to their default values.
        /// </summary>
        public void ResetState()
        {
            if (_dblclkTimer.Enabled)
                _dblclkTimer.Enabled = false;

            //Pointing Magnifier State
            if (Magnified)
                DeMagnify();

            //Action Item
            Clickthrough = false;

            //Mouse State low level
            _mouseDown = 0;
            _synthMouseUp = false;
            _synthMouseDown = false;

            //Mouse State high level
            _dragging = false;

            _pressedButton = MouseButtons.None;
            _mouseDownPt = new Point();

            this.Invalidate();

            _logger.ResetEvent();
        }

        /// <summary>
        /// When timer event fires, the interval for allowing double clicks is over and the 
        /// Pointing Magnifier must be reset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _dblclkTimer_Tick(object sender, EventArgs e)
        {
            ResetState();
        }

        #endregion

        #region Magnification

        /// <summary>
        /// Performs the necessary steps to magnify the area cursor. 
        /// In most cases the magnifier must be transposed so that the magnified area will draw within the bounds of the screen. 
        /// </summary>
        public void Magnify()
        {
            Rectangle r = Screen.GetBounds(this.Bounds);
            _magnified = true;
            //Set the amount the magnifier must be transposed in the x-axis.
            if ((Center.X - Radius) < r.X)
            {
                _transpose.X = r.X - (Center.X - Radius);
                if (Center.X - RadiusNM < r.X)
                    _transpose.X = _transpose.X + (Center.X - RadiusNM - r.X) * MagnificationFactor;
            }
            else if ((Center.X + Radius) > (r.Width + r.X))
            {
                _transpose.X = (r.Width + r.X) - (Center.X + Radius);
                if (Center.X + RadiusNM > (r.Width + r.X))
                    _transpose.X = _transpose.X + (Center.X + RadiusNM - (r.Width + r.X)) * MagnificationFactor;
            }
            else
                _transpose.X = 0;

            //Set the amount the magnifier must be transposed in the y-axis.
            if ((Center.Y - Radius) < r.Y)
            {
                _transpose.Y = r.Y - (Center.Y - Radius);
                if (Center.Y - RadiusNM < r.Y)
                    _transpose.Y = _transpose.Y + (Center.Y - RadiusNM - r.Y) * MagnificationFactor;
            }
            else if ((Center.Y + Radius) > (r.Height + r.Y))
            {
                _transpose.Y = (r.Height + r.Y) - (Center.Y + Radius);
                if (Center.Y + RadiusNM > (r.Height + r.Y))
                    _transpose.Y = _transpose.Y + (Center.Y + RadiusNM - (r.Height + r.Y)) * MagnificationFactor;
            }
            else
                _transpose.Y = 0;

            Point p = new Point(Center.X + _transpose.X, Center.Y + _transpose.Y);
            Cursor.Position = p;
            Center = p;

            _cursor.Visible = true;

            //Set the time in milliseconds that the PM is magnified so that elapsed time can be used for animation.
            _startTime = WobbrockLib.Extensions.TimeEx.NowMs;

            //Start the animation and draw the first step.
            this.Bounds = new Rectangle(this.Location, new Size(Diameter, Diameter));
            _tAnimation.Enabled = true;
            
            //Updateds the system parameter for the mouse gain, slowing down the mouse speed when magnified.
            if (LowerMouseGain)
            {
                uint mouseSpeed = 10;
                bool fResult;
                fResult = Win32.SystemParametersInfo((uint)Win32.SPI.GETMOUSESPEED, 0, ref mouseSpeed, 0);
                if (fResult)
                {
                    mouseSpeed = mouseSpeed / (uint)3;
                    Win32.SystemParametersInfo((uint)Win32.SPI.SETMOUSESPEED, 0, mouseSpeed, (uint)Win32.SPIF.SENDCHANGE);
                }
            }
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("X", this.Bounds.X.ToString());
            nvc.Add("Y", this.Bounds.Y.ToString());
            nvc.Add("Diameter", Diameter.ToString());
            nvc.Add("TransposeX", _transpose.X.ToString());
            nvc.Add("TransposeY", _transpose.Y.ToString());
            _logger.LogEvent("Magnify", nvc);
            
        }

        /// <summary>
        /// When I mouse down event happens in the magnified state, the cursor position must be mapped to its underlying 
        /// screen coordinates.
        /// </summary>
        /// <returns>The underlying screen coordinates of a click happening in magnified space.</returns>
        public Point TransposeMouse()
        {
            Point p = new Point(
                (Cursor.Position.X - Center.X) / MagnificationFactor,
                (Cursor.Position.Y - Center.Y) / MagnificationFactor);
            p.X = Center.X - _transpose.X + p.X;
            p.Y = Center.Y - _transpose.Y + p.Y;
            _transpose = Point.Empty;
            return p;
        }

        /// <summary>
        /// Returns the PM from a magnified state to a normal area cursor.
        /// </summary>
        public void DeMagnify()
        {
            _logger.LogEvent("DeMagnify", null);
            _magnified = false;
            Center = Cursor.Position;
            SetPath();
            if (LowerMouseGain)
            {
                uint mouseSpeed = 10;
                bool fResult;
                fResult = Win32.SystemParametersInfo((uint)Win32.SPI.GETMOUSESPEED, 0, ref mouseSpeed, 0);
                if (fResult)
                {
                    mouseSpeed = mouseSpeed * (uint)3;
                    Win32.SystemParametersInfo((uint)Win32.SPI.SETMOUSESPEED, 0, mouseSpeed, (uint)Win32.SPIF.SENDCHANGE);
                }
            }
        }

        /// <summary>
        /// Updates the stored image with the current screen underneath the PM.
        /// </summary>
        private void ScrapePixels()
        {
            //Must hide the VMM to scape what is beneath it
            this.Visible = false;

            // capture the screen in a small sample area around the click point.
            Bitmap bmpCk = new Bitmap(Diameter, Diameter);
            Graphics gck = Graphics.FromImage(bmpCk);
            gck.CopyFromScreen(Location, new Point(0, 0), new Size(Diameter, Diameter));
            gck.Dispose();

            this.Visible = true;
            _bmp = bmpCk;
        }
        #endregion

        #region Mouse Events

       

        /// <summary>
        /// Captures all mouse movements, even if the mouse is not currently captured by the PM. Used to update the position of
        /// the PM so that it is centered about the cursor.
        /// 
        /// Movements are also used to trigger the drag state, when the mouse has moved and a mouse button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRawInputMouseMove(object sender, MouseEventArgs e)
        {
            _idleTimeout.Interval = IDLELENGTH;
            if (isIdle)
            {
                _idleTimeout.Enabled = true;
                if (Idle != null)
                    Idle(this, null);
            }

            if (Active)
            {
                if (!Magnified)
                    Center = e.Location;
                if (_dragging && _mouseDown > 0)
                {
                    _logger.MouseMove(e);
                }
                else if (WobbrockLib.Extensions.GeotrigEx.Distance((PointF)_mouseDownPt, (PointF)e.Location) > 10
                        && _mouseDown > 0 && !_dragging)
                {
                    _dragging = true;
                    _logger.MouseMove(e);
                    if (!_clickthrough)
                    {
                        //Release capture of mouse
                        _synthMouseUp = true;
                        Win32.INPUT i = new Win32.INPUT();
                        i.type = Win32.INPUTF.MOUSE;
                        i.mi.dx = 0;
                        i.mi.dy = 0;
                        i.mi.mouseData = 0;
                        i.mi.time = 0;
                        i.mi.dwExtraInfo = UIntPtr.Zero;

                        i.mi.dwFlags = MouseButton(_pressedButton, true);
                        Win32.SendInput(1, ref i, System.Runtime.InteropServices.Marshal.SizeOf(new Win32.INPUT()));

                        _synthMouseUp = false;
                        Cursor.Position = _mouseDownPt; //Move mouse back to original mouse down location
                        Clickthrough = true; // set the form transparent so that the click we synthesize will go through
                        _synthMouseDown = true;

                        i.mi.dwFlags = MouseButton(_pressedButton, false); // synthesize a mouse down in the mapped location, and since we want it to go through
                        Win32.SendInput(1, ref i, System.Runtime.InteropServices.Marshal.SizeOf(new Win32.INPUT()));
                        _synthMouseDown = false;

                        Cursor.Position = e.Location;//Reset cursor position to the current location
                    }
                }
            }
        }

        /// <summary>
        /// Mouse Wheel events are passed through to the underlying application. To do this, the cursor is set to clickthrough and the double click timeout 
        /// is started. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRawInputMouseWheel(object sender, MouseEventArgs e)
        {
            _logger.ME(e, "W", Magnified, _mouseDown, false);

            if (!Clickthrough)
                Clickthrough = true;

            _dblclkTimer.Stop();
            _dblclkTimer.Start();
        }

        /// <summary>
        /// Logs all mouse down events, regardless of whether or not the Magnifier is enabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRawInputMouseDown(object sender, MouseEventArgs e)
        {
            if (_dblclkTimer.Enabled)
            {
                //_mouseDown++; //let's see what happens here
                _dblclkTimer.Enabled = false;
                //_dblclkTimer.Start(); //Don't restart the timer until the mouse button is released!
            }
            _logger.ME(e, "D", Magnified, _mouseDown, _synthMouseDown);
        }

        /// <summary>
        /// Handles mouse down input that happens on the magnifier. Only handles events that are either in the magnified state, or was triggered 
        /// by and XButton.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointingMagnifier_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.XButton1
                    && e.Button != MouseButtons.XButton2)
            {
                if (Magnified && _mouseDown < 1) //only handle while magnified and no buttons are pressed
                {
                    // before moving the mouse, whose physical button is down, we want to release the
                    // button that was down, but we don't want our mouse up handler to receive this,
                    // so we first block the input from coming through.
                    _synthMouseUp = true;

                    Win32.INPUT i = new Win32.INPUT();
                    i.type = Win32.INPUTF.MOUSE;
                    i.mi.dx = 0;
                    i.mi.dy = 0;
                    i.mi.mouseData = 0;
                    i.mi.time = 0;
                    i.mi.dwExtraInfo = UIntPtr.Zero;
                    i.mi.dwFlags = MouseButton(e.Button, true);
                    Win32.SendInput(1, ref i, System.Runtime.InteropServices.Marshal.SizeOf(new Win32.INPUT()));

                    _synthMouseUp = false;

                    Cursor.Position = TransposeMouse(); // now move the mouse to the mapped click location.

                    // synthesize a mouse down in the mapped location, and since we want it to go through, unblock input
                    Clickthrough = true;
                    DeMagnify();

                    _synthMouseDown = true;
                    i.mi.dwFlags = MouseButton(e.Button, false);
                    Win32.SendInput(1, ref i, System.Runtime.InteropServices.Marshal.SizeOf(new Win32.INPUT()));
                    _synthMouseDown = false;
                }

                _pressedButton = e.Button;
                _mouseDown++;
                _mouseDownPt = Cursor.Position;
            }
            else //Mouse down was triggered by an x button and should be passed through to underlying application
            {
                _synthMouseUp = true;

                Win32.INPUT i = new Win32.INPUT();
                i.type = Win32.INPUTF.MOUSE;
                i.mi.dx = 0;
                i.mi.dy = 0;
                switch (e.Button) //Button 1 or Button 2
                {
                    case System.Windows.Forms.MouseButtons.XButton1:
                        i.mi.mouseData = (uint)Win32.MOUSEEVENTF.XBUTTON1;
                        break;
                    case System.Windows.Forms.MouseButtons.XButton2:
                        i.mi.mouseData = (uint)Win32.MOUSEEVENTF.XBUTTON2;
                        break;
                    default:
                        i.mi.mouseData = 0;
                        break;
                }

                i.mi.time = 0;
                i.mi.dwExtraInfo = UIntPtr.Zero;
                i.mi.dwFlags = Win32.MOUSEEVENTF.XUP;
                Win32.SendInput(1, ref i, System.Runtime.InteropServices.Marshal.SizeOf(new Win32.INPUT()));

                _synthMouseUp = false;

                // synthesize a mouse down in the mapped location, and since we want it to go through, unblock input
                Clickthrough = true;

                //this.Invalidate();
                _synthMouseDown = true;
                i.mi.dwFlags = Win32.MOUSEEVENTF.XDOWN;
                Win32.SendInput(1, ref i, System.Runtime.InteropServices.Marshal.SizeOf(new Win32.INPUT()));
                _synthMouseDown = false;
            }
        }

        /// <summary>
        /// Low Level mouse hook handles mouse up events regardless of whether they happen on the Pointing Magnifier.
        /// Will magnify the PM when in the unmagnified state or start the double click timeout 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRawInputMouseUp(object sender, MouseEventArgs e)
        {
            _logger.ME(e,"U", Magnified, _mouseDown, _synthMouseUp);
            if (Active && !_synthMouseUp)
            {
                if (e.Button != MouseButtons.XButton1
                    && e.Button != MouseButtons.XButton2)
                {
                    _mouseDown--;
                    if (!Magnified && !Clickthrough && _mouseDown < 1 && !_dragging && this.Focused)
                    {
                        ScrapePixels();
                        Magnify();
                    }
                    else if (!_dblclkTimer.Enabled && _mouseDown < 1)
                    {
                        _dblclkTimer.Enabled = true;
                    }
                }
                else
                {
                    if (!Clickthrough)
                        Clickthrough = true;

                    _dblclkTimer.Enabled = true;
                }
            }
        }
        /// <summary>
        /// Maps a C# mouse button to the proper Win32 mouse event flag.
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        private Win32.MOUSEEVENTF MouseButton(MouseButtons mb, bool up)
        {
            switch (mb)
            {
                case MouseButtons.Left:
                    return up ? Win32.MOUSEEVENTF.LEFTUP : Win32.MOUSEEVENTF.LEFTDOWN;
                case MouseButtons.Right:
                    return up ? Win32.MOUSEEVENTF.RIGHTUP : Win32.MOUSEEVENTF.RIGHTDOWN;
                case MouseButtons.Middle:
                    return up ? Win32.MOUSEEVENTF.MIDDLEUP : Win32.MOUSEEVENTF.MIDDLEDOWN;
                default:
                    return 0u;
            }
        }
        #endregion
    }
 
}
