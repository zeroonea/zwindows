using System;
using System.Drawing;
using System.Windows.Forms;
using zwindows;
using zwindows.Enum;
using zwindows.Utils;

namespace zwindows.Utils
{
    public class WindowTabButton
    {
        private Label _label;
        private ToolTip _tooltip;
        private PictureBox _icon;
        public IntPtr Hwnd { get; set; }
        public CustomPanel Panel { get; set; }
        public ScreenTabButtonsBar Parent { get; set; }
        public MonitorLayout Layout { get; set; }
        private Color _iconColor;
        private bool _isActive;
        public bool TitleEvent { get; set; } = false;
        public bool IsWindowTopMost
        {
            get
            {
                return Win32Helper.IsWindowTopMost(Hwnd);
            }
        }
        private int DragState = 0;

        //public delegate void MouseRightClick(WindowTabButton source);
        //public MouseRightClick MyRightClick;

        public WindowTabButton()
        {
            TitleEvent = Global.Settings != null ? Global.Settings.DefaultWindowsTabNotificationOn : false;

            _label = new Label();
            _label.Text = "";
            _label.ForeColor = Color.White;
            _label.Font = new Font("Arial", 9, FontStyle.Regular);
            _label.AutoSize = false;
            _label.AutoEllipsis = true;
            _label.Location = new Point(24, 5);
            _label.Size = new System.Drawing.Size(76, 20);

            _tooltip = new ToolTip();
            _tooltip.SetToolTip(_label, "Click to active");
            _tooltip.ShowAlways = true;

            _icon = new PictureBox();
            _icon.Width = 16;
            _icon.Height = 16;
            _icon.SizeMode = PictureBoxSizeMode.StretchImage;
            _icon.Location = new Point(5, 5);

            Panel = new CustomPanel();
            Panel.Margin = new Padding(0);
            Panel.Controls.Add(_label);
            Panel.Controls.Add(_icon);
            Panel.Size = new System.Drawing.Size(100, 25);
            Panel.TabButton = this;
            Panel.Cursor = Cursors.Hand;

            _label.MouseClick += Panel_Click;
            _icon.MouseClick += Panel_Click;
            Panel.MouseClick += Panel_Click;;

            Panel.AllowDrop = true;
            Panel.DragOver += Panel_DragOver;

            _icon.DoubleClick += Panel_DoubleClick;
            _label.DoubleClick += Panel_DoubleClick;
            Panel.DoubleClick += Panel_DoubleClick;



            Panel.MouseDown += Panel_MouseDown;
            Panel.MouseMove += Panel_MouseMove;
            Panel.MouseUp += Panel_MouseUp;
            _label.MouseDown += Panel_MouseDown;
            _label.MouseMove += Panel_MouseMove;
            _label.MouseUp += Panel_MouseUp;
            _icon.MouseDown += Panel_MouseDown;
            _icon.MouseMove += Panel_MouseMove;
            _icon.MouseUp += Panel_MouseUp;
        }

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            if(DragState != 0)
            { 
                DragState = 0;
                BackColor = _iconColor;
                TextColor = Color.White;
                Console.WriteLine($"DragState: {DragState}");
            }
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button != MouseButtons.Left) return;

            if(DragState == 1)
            {
                if(Math.Abs(Cursor.Position.X - Global.GlobalMouseDragStartX) >= 6)
                { 
                    // Mouse start to move
                    DragState = 2;
                    Console.WriteLine($"DragState: {DragState}");
                    BackColor = Global.CustomColors["DragWindowTab"];
                    TextColor = Color.Black;
                }
            }

            if(DragState == 2)
            {
                // Mouse is moving
                var d = Math.Abs(Parent.Bar.Location.X - Cursor.Position.X);
                var index = (int)Math.Floor(d/Panel.Width + 0.0);
                var oldIndex = (int)Math.Floor(Panel.Location.X/Panel.Width + 0.0);
                Console.WriteLine($"DragState: {DragState}, {d}, {Panel.Location.X}, {oldIndex}, {index}");

                if(oldIndex != index)
                {
                    Parent.WindowButtonsPanel.Controls.SetChildIndex(Panel, index);
                }
            }
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button != MouseButtons.Left) return;

            if(DragState == 0)
            {
                DragState = 1;
                Global.GlobalMouseDragStartX = Cursor.Position.X;
                Console.WriteLine($"DragState: {DragState}");
            }
        }

        private void Panel_DoubleClick(object sender, EventArgs e)
        {
            Snap();
        }

        private void Panel_DragOver(object sender, DragEventArgs e)
        {
            Active();
        }

        private void Panel_Click(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                Active();
            }
        }

        public void Active()
        {
            var title = Win32Helper.GetWindowTitle(Hwnd);
            if (string.IsNullOrEmpty(title))
            {
                Parent.RemoveTabWindowButton(this);
                return;
            }
            Text = title;
            if(Win32Helper.IsIconic(Hwnd))
            {
                Win32Helper.ShowWindow(Hwnd, CmdShow.SW_RESTORE);
            }
            else
            {
                Win32Helper.ShowWindow(Hwnd, CmdShow.SW_RESTORE);
                Win32Helper.BringWindowToFront(Hwnd);
            }
            Parent.SelectedWindowTabButton = this;
        }

        public void Snap()
        {
            Win32Helper.SetDesktopAndMonitor(Parent.Handle);
            Win32Helper.SnapWindowToLayout(Hwnd, Global.CurrentMonitor, Layout);
            Win32Helper.BringWindowToFront(Hwnd);
        }

        public string Text
        {
            get { return _label.Text; }
            set 
            { 
                _tooltip.ToolTipTitle = value;
                _label.Text = value; 
            }
        }

        private Color BackColor
        {
            set
            {
                Panel.BackColor = value;
            }
            get
            {
                return Panel.BackColor;
            }
        }

        public Color TextColor
        {
            set
            {
                _label.ForeColor = value;
            }
            get
            {
                return _label.ForeColor;
            }
        }

        public Icon Icon
        {
            set
            {
                if(value == null) return;
                _iconColor = MyMath.CalculateAverageColor(value.ToBitmap(), 2);
                BackColor = _iconColor;
                _icon.Image = value.ToBitmap();
            }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                if (_isActive)
                {
                    BackColor = System.Drawing.ColorTranslator
                        .FromHtml(Global.Settings.ActiveWindowTabButtonColor);
                    /*TextColor = System.Drawing.ColorTranslator
                        .FromHtml(Global.Settings.ActiveWindowTabButtonTextColor);*/
                }
                else
                {
                    BackColor = _iconColor;
                    //TextColor = Color.White;
                }
            }
        }

        public void ResetBackColor()
        {
            BackColor = _iconColor;
        }

        public class CustomPanel : Panel
        {
            public WindowTabButton TabButton { get; set; }
        }
    }
}
