﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zwindows.Enum;

namespace zwindows.Utils.UI
{
    public class LayoutOverlay : Form
    {
        private System.Timers.Timer _autoOffTimer;
        private int _initialStyle;

        public LayoutOverlay()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;

            BackColor = ColorTranslator.FromHtml(Global.Settings.WindowDragOverlayColor);
            //Opacity = 0.3;
            //TopMost = true;
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;

            Load += LayoutOverlay_Load;
        }

        private void LayoutOverlay_Load(object sender, EventArgs e)
        {
            _initialStyle = Win32Helper.GetWindowLong(this.Handle, -20);
            Win32Helper.SetWindowLong(this.Handle, -20, _initialStyle | 0x80000 | 0x20);
            Win32Helper.SetLayeredWindowAttributes(this.Handle, 0, 100, Win32Helper.LWA_ALPHA);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;

                return Params;
            }
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LayoutOverlay
            // 
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LayoutOverlay";
            this.Padding = new System.Windows.Forms.Padding(15);
            this.ResumeLayout(false);

        }

        public void SetOwner(IntPtr hwndOwner)
        {
            Win32Helper.SetWindowLong(this.Handle, -8, hwndOwner);
        }


        #region Static

        private static LayoutOverlay _current = null;

        public static void ShowOverlay(IntPtr hwndOwner, int x, int y, int width, int height)
        {
            if(_current != null)
            {
                if (_current.IsDisposed)
                {
                    _current = null;
                }
            }

            if(_current == null)
            {
                _current = new LayoutOverlay();
            }
            
            _current.Location = new Point(x, y);
            _current.Width = width;
            _current.Height = height;

            if(hwndOwner != IntPtr.Zero)
            {
                _current.SetOwner(hwndOwner);
            }

            _current.Show();
        }

        public static void ShowOverlay(int x, int y, int width, int height)
        {
            ShowOverlay(IntPtr.Zero, x, y, width, height);
        }

        public static void ClearOverlay()
        {
            if(_current != null)
            { 
                _current.Dispose();
                _current = null;
            }
        }

        #endregion
    }
}
