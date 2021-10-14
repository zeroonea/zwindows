using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zwindows.Win32;

namespace zwindows.Utils.UI
{
    public class MoveableScreenElement : Form, IMessageFilter
    {
        private HashSet<Control> controlsToMove = new HashSet<Control>();

        public delegate void MyClickDelegate(object sender, EventArgs e);

        public MyClickDelegate MyClick;
        public MyClickDelegate MyClickUp;
        public MyClickDelegate MyRightClick;

        public MoveableScreenElement(Form owner)
        {
            Application.AddMessageFilter(this);

            this.Visible = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.Owner = owner;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            controlsToMove.Add(this);

            /*this.Shown += ScreenElement_Shown;*/
        }

        public void AddControls(Control control)
        {
            controlsToMove.Add(control);
            Controls.Add(control);
        }

        protected override bool ShowWithoutActivation => true;

        
        private const int WM_MOUSEACTIVATE = 0x0021, MA_NOACTIVATE = 0x0003;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE) 
            {
                 m.Result = (IntPtr)MA_NOACTIVATE;
                 return;
            }
            base.WndProc(ref m);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == Win32Helper.WM_LBUTTONDOWN &&
                controlsToMove.Contains(Control.FromHandle(m.HWnd)))
            {
                if(MyClick != null)
                { 
                    MyClick(null, null);
                }
                Win32Helper.ReleaseCapture();
                Win32Helper.SendMessage(this.Handle, Win32Helper.WM_NCLBUTTONDOWN, Win32Helper.HT_CAPTION, 0);
                return true;
            }
            else if (m.Msg == Win32Helper.WM_LBUTTONUP)
            {
                if(MyClickUp != null)
                { 
                    MyClickUp(null, null);
                }
                //Win32Helper.ReleaseCapture();
            }
            else if (m.Msg == Win32Helper.WM_RBUTTONDOWN)
            {
                if(MyRightClick != null)
                { 
                    MyRightClick(null, null);
                }
                //Win32Helper.ReleaseCapture();
            }
            return false;
        }

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
