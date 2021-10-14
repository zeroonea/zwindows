using System.Windows.Forms;
using zwindowscore.Enum;

namespace zwindowscore.Utils.UI
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
