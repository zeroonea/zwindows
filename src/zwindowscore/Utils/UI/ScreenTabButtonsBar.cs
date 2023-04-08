using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using zwindowscore.Enum;
using zwindowscore.Utils.UI;
using System.Linq;
using zwindowscore.Properties;
using zwindowscore.Options;
using WindowsDesktop;

namespace zwindowscore.Utils
{
    public class ScreenTabButtonsBar : IDisposable
    {
        private bool _visible;
        private Rectangle _location;

        private MoveableScreenElement _bar;
        private FlowLayoutPanel _pnlWindows;
        private FlowLayoutPanel _wrapper;
        private bool _isSnapped = true;

        private ContextMenuStrip _ctxTabBtnMenu;
        private ToolStripMenuItem _ctxTabBtnMenuDesktopItem;
        private ToolStripMenuItem _ctxTabBtnMenuSnapItem;
        private ToolStripMenuItem _ctxTabBtnMenuTopmostItem;
        private ToolStripMenuItem _ctxTabBtnMenuNotificationItem;
        private ToolStripMenuItem _ctxTabBtnMenuCloseItem;

        private ContextMenuStrip _ctxMenu;
        private ToolStripMenuItem _ctxMenuSnapItem;
        private ToolStripMenuItem _ctxMenuTopmostItem;

        private WindowTabButton _selectedWindowTabBtn;

        public List<WindowTabButton> WindowButtons = new List<WindowTabButton>();

        private int _maxWidth;

        public string Key { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int MaxWidth
        {
            set
            {
                _maxWidth = value;
                _bar.MaximumSize = new System.Drawing.Size(_maxWidth, Global.Settings.TabButtonHeight);
                _pnlWindows.MaximumSize = new System.Drawing.Size(_maxWidth-50, Global.Settings.TabButtonHeight);
                _wrapper.MaximumSize = new System.Drawing.Size(_maxWidth-25, Global.Settings.TabButtonHeight);
            }
            get
            {
                return _maxWidth;
            }
        }

        public ScreenTabButtonsBar(Form desktopAnchor, MonitorLayoutTabsBar data)
        {
            //initialize the form
            _bar = new MoveableScreenElement(desktopAnchor);
            _bar.TopMost = true;
            _bar.Left = 0;
            _bar.Top = 0;
            _bar.AutoSize = true;
            _bar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bar.HorizontalScroll.Visible = false;
            _bar.HorizontalScroll.Enabled = false;
            _bar.Padding = new Padding(0, 0, 25, 0);
            _bar.BackColor = Global.GetColor(Global.Settings.TabBarColor);
            _bar.Cursor = Cursors.SizeAll;

            _wrapper = new FlowLayoutPanel();
            _wrapper.WrapContents = false;
            //_wrapper.Padding = new Padding(0);

            _pnlWindows = new FlowLayoutPanel();

            _pnlWindows.Margin = new Padding(0);
            _pnlWindows.WrapContents = false;
            _pnlWindows.AutoScroll = true;
            _pnlWindows.AutoSize = false;
            //_pnlWindows.AllowDrop = true;
            //_pnlWindows.DragDrop += pnlWindows_DragDrop;

            var moreBtn = new TabButton($"TabBar");
            moreBtn.Icon = Resources.iconMore;

            _ctxTabBtnMenu = new ContextMenuStrip();
            _ctxTabBtnMenu.ItemClicked += _ctxTabBtnMenu_ItemClicked;
            _ctxTabBtnMenu.Opening += _ctxTabBtnMenu_Opening;

            _ctxTabBtnMenuDesktopItem = new ToolStripMenuItem("Desktop");
            _ctxTabBtnMenuDesktopItem.DropDownItemClicked += _ctxTabBtnMenuDesktopItem_DropDownItemClicked;

            _ctxTabBtnMenuSnapItem = new ToolStripMenuItem("Snap");
            _ctxTabBtnMenuTopmostItem = new ToolStripMenuItem("Topmost");
            _ctxTabBtnMenuNotificationItem = new ToolStripMenuItem("Notification");
            _ctxTabBtnMenuCloseItem = new ToolStripMenuItem("Close");
            //_ctxTabBtnMenuCloseItem.Image = 

            _ctxTabBtnMenu.Items.Add(_ctxTabBtnMenuSnapItem);
            _ctxTabBtnMenu.Items.Add(_ctxTabBtnMenuTopmostItem);
            _ctxTabBtnMenu.Items.Add(_ctxTabBtnMenuNotificationItem);
            _ctxTabBtnMenu.Items.Add(_ctxTabBtnMenuDesktopItem);
            _ctxTabBtnMenu.Items.Add(_ctxTabBtnMenuCloseItem);

            // --------------------------
            _ctxMenu = new ContextMenuStrip();
            _ctxMenu.Opening += _ctxMenu_Opening;
            _ctxMenu.ItemClicked += _ctxMenu_ItemClicked;

            _ctxMenuSnapItem = new ToolStripMenuItem("Snap Bar");
            _ctxMenuTopmostItem = new ToolStripMenuItem("Topmost");
            _ctxMenuTopmostItem.Checked = true;

            _ctxMenu.Items.Add(_ctxMenuSnapItem);
            _ctxMenu.Items.Add(_ctxMenuTopmostItem);

            
            moreBtn.ContextMenuStrip = _ctxMenu;

            
            _wrapper.Controls.Add(_pnlWindows);
            _wrapper.Controls.Add(moreBtn);
            _bar.Controls.Add(_wrapper);
        }


        private void _ctxMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _ctxMenuTopmostItem.Checked = _bar.TopMost;
            e.Cancel = false;
        }

        private void _ctxTabBtnMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var panel = (WindowTabButton.CustomPanel)_ctxTabBtnMenu.SourceControl;
            var tabBtn = panel.TabButton;
            _ctxTabBtnMenuTopmostItem.Checked = tabBtn.IsWindowTopMost;
            _ctxTabBtnMenuNotificationItem.Checked = tabBtn.TitleEvent;
            var dn = Win32Helper.GetDesktopName(tabBtn.Hwnd);
            if(dn == Global.CurrentDesktopName)
            {
                _ctxTabBtnMenuDesktopItem.Checked = true;
            }
            else
            {
                _ctxTabBtnMenuDesktopItem.Checked = false;
            }
            _ctxTabBtnMenuDesktopItem.Text = $"Desktops [{dn}]";
            _ctxTabBtnMenuDesktopItem.DropDownItems.Clear();
            var i = 1;
            foreach(var d in Global.VirtualDesktops)
            {
                _ctxTabBtnMenuDesktopItem.DropDownItems.Add(new ToolStripMenuItem
                {
                    Text = $"{i}. {d.Name}",
                    Tag = d.Id,
                    ToolTipText = d.Name != dn ? $"Move Window to Desktop [{d.Name}]" : $"Window already on Desktop [{d.Name}]",
                    Enabled = d.Name != dn
                });
                i++;
            }

            e.Cancel = false;
        }

        private void _ctxTabBtnMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var panel = (WindowTabButton.CustomPanel)_ctxTabBtnMenu.SourceControl;
            var tabBtn = panel.TabButton;

            if (e.ClickedItem == _ctxTabBtnMenuSnapItem)
            {
                tabBtn.Snap();
            }
            else if (e.ClickedItem == _ctxTabBtnMenuNotificationItem)
            {
                tabBtn.TitleEvent = !tabBtn.TitleEvent;
                _ctxTabBtnMenuNotificationItem.Checked = tabBtn.TitleEvent;
            }
            else if (e.ClickedItem == _ctxTabBtnMenuTopmostItem)
            {
                tabBtn.Topmost(!tabBtn.IsWindowTopMost);
                _ctxTabBtnMenuTopmostItem.Checked = !tabBtn.IsWindowTopMost;
            }
            else if (e.ClickedItem == _ctxTabBtnMenuCloseItem)
            {
                Win32Helper.CloseWindow(tabBtn.Hwnd);
            }
        }

        private void _ctxTabBtnMenuDesktopItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var panel = (WindowTabButton.CustomPanel)_ctxTabBtnMenu.SourceControl;
            var tabBtn = panel.TabButton;
            var did = (Guid)e.ClickedItem.Tag;
            var vd = Global.VirtualDesktops.Where(p => p.Id == did).FirstOrDefault();
            if(vd != null)
            { 
                Win32Helper.PauseWinEventHook = true;
                VirtualDesktop.MoveToDesktop(tabBtn.Hwnd, vd.VirtualDesktop);
                Win32Helper.PauseWinEventHook = false;
                tabBtn.Desktop = vd.Index;
            }
        }

        private void _ctxMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == _ctxMenuSnapItem)
            {
                _isSnapped = true;
                UpdateLocation();
            }
            else if (e.ClickedItem == _ctxMenuTopmostItem)
            {
                TopMost = !_ctxMenuTopmostItem.Checked;
                _ctxMenuTopmostItem.Checked = TopMost;
            }
        }

        public WindowTabButton AddTabWindowButton(IntPtr hwnd, string title)
        {
            var tbtn = new WindowTabButton
            {
                Hwnd = hwnd,
                Text = title,
                Icon = IconHelper.GetAppIcon(hwnd)
            };
            return AddTabWindowButton(tbtn);
        }

        public WindowTabButton RemoveTabWindowButton(IntPtr hwnd)
        {
            var tbtn = WindowButtons.FirstOrDefault(p => p.Hwnd == hwnd);
            if(tbtn != null)
            {
                RemoveTabWindowButton(tbtn);
            }
            return tbtn;
        }

        public WindowTabButton AddTabWindowButton(WindowTabButton tbtn)
        {
            WindowButtons.Add(tbtn);
            tbtn.Parent = this;
            tbtn.Panel.ContextMenuStrip = _ctxTabBtnMenu;
            _pnlWindows.SuspendLayout();
            _pnlWindows.Controls.Add(tbtn.Panel);
            tbtn.Panel.ResumeLayout(false);
            _pnlWindows.ResumeLayout(false);
            if(_isSnapped)
            { 
                UpdateLocation();
            }

            var vd = Win32Helper.GetDesktop(tbtn.Hwnd);
            if(vd != null)
            {
                tbtn.Desktop = Win32Helper.GetDesktopIndex(vd);
            }
            return tbtn;
        }

        public void RemoveTabWindowButton(WindowTabButton tbtn)
        {
            WindowButtons.Remove(tbtn);
            _pnlWindows.SuspendLayout();
            tbtn.Panel.SuspendLayout();
            _pnlWindows.Controls.Remove(tbtn.Panel);
            _pnlWindows.ResumeLayout(false);
            tbtn.Parent = null;
            Console.WriteLine($"Remove tab button: {tbtn.Text}");
            if(_isSnapped)
            { 
                UpdateLocation();
            }
        }

        public void UpdateLocation()
        {
            if (WindowButtons.Count == 0)
            {
                Global.TabButtonsBars.Remove(Key);
                Dispose();
                return;
            }
            //Console.WriteLine("Bar Width: {0}", _pnl.Width);
            _pnlWindows.Width = WindowButtons.Count * Global.Settings.TabButtonWidth;
            //_pnlWindows.Height = 25;
            _wrapper.Width = _pnlWindows.Width + 25;
            //_wrapper.Height = 25;
            var btnsw = _wrapper.Width + 25;
            _location = new Rectangle();
            if(btnsw < MaxWidth)
            {
                _location.X = X + (MaxWidth - btnsw) / 2;
            }
            else
            {
                _location.X = X;
            }
            _location.Y = Y;
            Location = _location;

            //Console.WriteLine($"---bar: {Key}");
            //Console.WriteLine($"_pnlWindows.Width: {_pnlWindows.Width}");
            //Console.WriteLine($"_wrapper.Width: {_wrapper.Width}");
        }

        public Form Bar
        {
            get
            {
                return _bar;
            }
        }

        public FlowLayoutPanel WindowButtonsPanel
        {
            get
            {
                return _pnlWindows;
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;

                if (value)
                    Win32Helper.ShowWindow(_bar.Handle, CmdShow.SW_SHOWNA);
                else
                    _bar.Hide();
            }
        }

        public double Opacity
        {
            get { return _bar.Opacity; }
            set { _bar.Opacity = value; }
        }

        public Rectangle Location
        {
            get { return _location; }
            set
            {
                _location = value;
                Layout();
            }
        }

        public bool TopMost
        {
            set
            {
                _bar.TopMost = value;
            }
            get
            {
                return _bar.TopMost;
            }
        }

        public IntPtr Handle
        {
            get
            {
                return _bar.Handle;
            }
        }

        public WindowTabButton SelectedWindowTabButton
        {
            get
            {
                return _selectedWindowTabBtn;
            }
            set
            {
                foreach(var wbtn in WindowButtons)
                {
                    if(wbtn == value) continue;
                    wbtn.IsActive = false;
                }
                _selectedWindowTabBtn = value;
                _selectedWindowTabBtn.IsActive = true;
            }
        }
        
        private void Layout()
        {
            Win32Helper.SetWindowPos(_bar.Handle, Win32Helper.HWND_TOPMOST, 
                _location.X, _location.Y, 
                0, 0, WindowPosFlags.Unknown | WindowPosFlags.IgnoreResize);

        }

        #region IDisposable Members

        public void Dispose()
        {
            _bar.Dispose();
        }

        #endregion
    }
}
