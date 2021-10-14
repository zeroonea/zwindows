using System;
using System.Drawing;
using System.Windows.Forms;
using zwindows;
using zwindows.Enum;
using zwindows.Utils;

namespace zwindows.Utils
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
            _icon.Width = 16;
            _icon.Height = 16;
            _icon.SizeMode = PictureBoxSizeMode.StretchImage;
            _icon.Location = new Point(5, 5);

            _tooltip = new ToolTip();
            _tooltip.SetToolTip(_icon, title);
            _tooltip.ShowAlways = true;

            this.Margin = new Padding(0);

            this.Controls.Add(_icon);
            this.Size = new System.Drawing.Size(25, 25);
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
