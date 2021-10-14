using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zwindowscore.Utils.UI
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

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Win32Helper.WM_MOUSEACTIVATE) 
            {
                 m.Result = (IntPtr)Win32Helper.MA_NOACTIVATE;
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

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= Win32Helper.WS_EX_TOOLWINDOW;
                cp.ExStyle &= ~Win32Helper.WS_EX_APPWINDOW; //ShowInTaskbar = false
                return cp;
            }
        }
    }
}
