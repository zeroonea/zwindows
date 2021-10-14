using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zwindows.Enum;
using zwindows.Win32;

namespace zwindows.Utils.UI
{
    public class ScreenElement : Form
    {
        public ScreenElement(Form owner)
        {
            this.Visible = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.Owner = owner;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            /*this.Shown += ScreenElement_Shown;*/
        }

        bool _topmost = false;

        public new bool TopMost
        {
            get
            {
                return _topmost;
            }
            set
            {
                _topmost = value;
                if (_topmost)
                {
                    Win32Helper.SetWindowPos(this.Handle, Win32Helper.HWND_TOPMOST, 0, 0, 0, 0, 
                        WindowPosFlags.IgnoreResizeAndMove);
                }
                else
                {
                    Win32Helper.SetWindowPos(this.Handle, Win32Helper.HWND_NOTOPMOST, 0, 0, 0, 0, 
                        WindowPosFlags.IgnoreResizeAndMove);
                }
            }
        }

        protected override bool ShowWithoutActivation => true;

        /*private const int WM_MOUSEACTIVATE = 0x0021, MA_NOACTIVATE = 0x0003;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE) 
            {
                 m.Result = (IntPtr)MA_NOACTIVATE;
                 return;
            }
            base.WndProc(ref m);
        }*/

        const int WS_EX_TOOLWINDOW = 0x80;
        const int WS_EX_APPWINDOW = 0x40000;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TOOLWINDOW;
                cp.ExStyle &= ~WS_EX_APPWINDOW; //ShowInTaskbar = false
                return cp;
            }
        }

        /*private const int WM_SYSCOMMAND = 0x0112; 
        private const int SC_MINIMIZE = 0xf020; 

        protected override void WndProc(ref Message m) 
        { 
            if (m.WParam.ToInt32() == SC_MINIMIZE) 
            { 
                m.Result = IntPtr.Zero; 
                return; 
            } 
            base.WndProc(ref m); 
        } 
*/
        /*private void ScreenElement_Shown(object sender, EventArgs e)
        {
            int num1 = UnsafeNativeMethods.GetWindowLong(Handle, -20);
            UnsafeNativeMethods.SetWindowLong(Handle, -20, num1 | 0x00000080);
        }*/

        /*protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }*/
    }
}
