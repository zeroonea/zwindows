using System;
using System.Drawing;
using System.Windows.Forms;
using zwindowscore;
using zwindowscore.Enum;
using zwindowscore.Utils;

namespace zwindowscore.Utils
{
    public class TabButton : Panel
    {
        private ToolTip _tooltip;
        private PictureBox _icon;
        public EventHandler MyClick
        {
            set
            {
                _icon.Click += value;
                this.Click += value;
            }
        }

        public TabButton(string title)
        {
            _icon = new PictureBox();
            _icon.Width = Global.Settings.TabButtonIconSize;
            _icon.Height = Global.Settings.TabButtonIconSize;
            _icon.SizeMode = PictureBoxSizeMode.StretchImage;
            var tmp = (int)Math.Round((Global.Settings.TabButtonHeight - Global.Settings.TabButtonIconSize) / 2f);
            _icon.Location = new Point(tmp, tmp);

            _tooltip = new ToolTip();
            _tooltip.SetToolTip(_icon, title);
            _tooltip.ShowAlways = true;

            this.Margin = new Padding(0);

            this.Controls.Add(_icon);
            this.Size = new System.Drawing.Size(25, Global.Settings.TabButtonHeight);
            this.BackColor = Color.White;
            this.Cursor = Cursors.Default;
        }

        public Image Icon
        {
            set
            {
                if(value == null) return;
                _icon.Image = value;
            }
        }
    }
}
