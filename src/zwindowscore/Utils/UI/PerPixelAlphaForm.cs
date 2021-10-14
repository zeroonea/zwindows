using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using zwindowscore.Struct;
using Size = zwindowscore.Struct.Size;

namespace zwindowscore.Utils
{
    public partial class PerPixelAlphaForm : Form, IMessageFilter
    {
        private bool _isCtx = false;

        public PerPixelAlphaForm()
        {
            Application.AddMessageFilter(this);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;
            this.Load += PerPixelAlphaForm_Load;
        }

        protected override bool ShowWithoutActivation => true;

        void PerPixelAlphaForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                // Add the layered extended style (WS_EX_LAYERED) to this window.
                CreateParams createParams = base.CreateParams;
                if(!DesignMode)
                    createParams.ExStyle |= Win32Helper.WS_EX_LAYERED |  Win32Helper.WS_EX_TOOLWINDOW;
                return createParams;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if(!_isCtx)
            { 
                if (m.Msg == Win32Helper.WM_MOUSEACTIVATE)
                {
                    m.Result = (IntPtr)Win32Helper.MA_NOACTIVATE;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        public ContextMenuStrip MyContextMenuStrip
        {
            set
            {
                ContextMenuStrip = value;
                ContextMenuStrip.Opening += ContextMenuStrip_Opening;
                ContextMenuStrip.Closed += ContextMenuStrip_Closed;
            }
        }

        private void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            _isCtx = false;
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            _isCtx = true;
            e.Cancel = false;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (_isCtx)
            {
                return false;
            }
            if (m.Msg == Win32Helper.WM_LBUTTONDOWN && m.HWnd == this.Handle)
            {
                Win32Helper.ReleaseCapture();
                Win32Helper.SendMessage(this.Handle, Win32Helper.WM_NCLBUTTONDOWN, Win32Helper.HT_CAPTION, 0);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        public void SelectBitmap(Bitmap bitmap)
        {
            SelectBitmap(bitmap, 255);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap">
        /// 
        /// </param>
        /// <param name="opacity">
        /// Specifies an alpha transparency value to be used on the entire source 
        /// bitmap. The SourceConstantAlpha value is combined with any per-pixel 
        /// alpha values in the source bitmap. The value ranges from 0 to 255. If 
        /// you set SourceConstantAlpha to 0, it is assumed that your image is 
        /// transparent. When you only want to use per-pixel alpha values, set 
        /// the SourceConstantAlpha value to 255 (opaque).
        /// </param>
        public void SelectBitmap(Bitmap bitmap, int opacity)
        {
            // Does this bitmap contain an alpha channel?
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ApplicationException("The bitmap must be 32bpp with alpha-channel.");
            }

            // Get device contexts
            IntPtr screenDc = Win32Helper.GetDC(IntPtr.Zero);
            IntPtr memDc = Win32Helper.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;

            try
            {
                // Get handle to the new bitmap and select it into the current 
                // device context.
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                hOldBitmap = Win32Helper.SelectObject(memDc, hBitmap);

                // Set parameters for layered window update.
                Size newSize = new Size(bitmap.Width, bitmap.Height);
                Struct.Point sourceLocation = new Struct.Point(0, 0);
                Struct.Point newLocation = new Struct.Point(this.Left, this.Top);
                BlendFunction blend = new BlendFunction();
                blend.BlendOp = Win32Helper.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = (byte)opacity;
                blend.AlphaFormat = Win32Helper.AC_SRC_ALPHA;

                // Update the window.
                Win32Helper.UpdateLayeredWindow(
                    this.Handle,     // Handle to the layered window
                    screenDc,        // Handle to the screen DC
                    ref newLocation, // New screen position of the layered window
                    ref newSize,     // New size of the layered window
                    memDc,           // Handle to the layered window surface DC
                    ref sourceLocation, // Location of the layer in the DC
                    0,               // Color key of the layered window
                    ref blend,       // Transparency of the layered window
                    Win32Helper.ULW_ALPHA        // Use blend as the blend function
                    );
            }
            finally
            {
                // Release device context.
                Win32Helper.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    Win32Helper.SelectObject(memDc, hOldBitmap);
                    Win32Helper.DeleteObject(hBitmap);
                }
                Win32Helper.DeleteDC(memDc);
            }
        }
    }
}