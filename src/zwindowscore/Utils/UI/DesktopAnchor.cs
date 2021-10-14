using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zwindowscore.Properties;

namespace zwindowscore.Utils.UI
{
    public class DesktopAnchor : Form
    {
        public DesktopAnchor()
        {
            TopMost = true;
            MinimizeBox = false;
            MaximizeBox = false;

            Show();

            Width = 1;
            Height = 1;
            Left = -30000;
            Top = -30000;
            ShowInTaskbar = false;
            Icon = Resources.icoZW;
            Text = "ZWindows - VirtualDesktop Anchor";
            FormBorderStyle = FormBorderStyle.None;
        }
    }
}
