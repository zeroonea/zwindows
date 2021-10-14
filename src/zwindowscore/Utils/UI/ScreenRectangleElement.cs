using System;
using System.Drawing;
using System.Windows.Forms;
using zwindowscore.Utils.UI;
using zwindowscore.Enum;

namespace zwindowscore.Utils
{
    public class ScreenRectangleElement : IDisposable
    {
        private bool _visible;
        private Color _color;
        private Rectangle _location;

        private Form _form;

        public ScreenRectangleElement(Form desktopAnchor)
        {
            //initialize the form
            this._form = new ScreenElement(desktopAnchor);

            _form.TopMost = true;
            _form.Left = 0;
            _form.Top = 0;
            _form.Width = 1;
            _form.Height = 1;
            _form.Opacity = 0.2;

            this._color = Color.Black;
        }

        public Form Form
        {
            get { return this._form; }
        }

        /// <summary>
        /// get/set visibility for the rectangle
        /// </summary>
        public bool Visible
        {
            get { return this._visible; }
            set
            {
                this._visible = value;

                if (value)
                    Win32Helper.ShowWindow(_form.Handle, CmdShow.SW_SHOWNA);
                else
                    _form.Hide();
            }
        }

        /// <summary>
        /// get/set color of the rectangle
        /// </summary>
        public Color Color
        {
            get { return this._color; }
            set
            {
                this._color = value;
                this._form.BackColor = value;
            }
        }

        /// <summary>
        /// get/set opacity for the rectangle
        /// </summary>
        public double Opacity
        {
            get { return this._form.Opacity; }
            set { this._form.Opacity = value; }
        }

        /// <summary>
        /// get/set location of the rectangle
        /// </summary>
        public Rectangle Location
        {
            get { return this._location; }
            set
            {
                this._location = value;
                this.Layout();
            }
        }

        /// <summary>
        /// this will set position of the rectangle when location has been changed
        /// </summary>
        private void Layout()
        {
//            SafeNativeMethods.SetWindowPos(this._leftForm.Handle, NativeMethods.HWND_TOPMOST, this._location.Left - this._width, this._location.Top, this._width, this._location.Height, 0x10);
            Win32Helper.SetWindowPos(this._form.Handle, Win32Helper.HWND_TOPMOST, 
                this._location.X, this._location.Y, 
                this._location.Width, this._location.Height, WindowPosFlags.Unknown);
        }

        #region IDisposable Members

        public void Dispose()
        {
            this._form.Dispose();
        }

        #endregion
    }
}
