//---------------------------------------------------------------------------
//
// <copyright file="ScreenRectangle" company="Microsoft">
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.
// </copyright>
// 
//
// Description: Definition of class responsible to draw filled rectangle on the screen
//
// History:  
//  09/06/2006 : Ondrej Lehecka, created
//
//---------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using zwindows.Win32;
using zwindows.Utils;
using zwindows.Utils.UI;

namespace zwindows.Utils
{
    public class ScreenText : IDisposable
    {
        private bool _visible;
        private Color _color;
        private Rectangle _location;

        private MoveableScreenElement _form;
        private Label _label;

        public ScreenText(Form desktopAnchor)
        {
            _label = new Label();
            _label.Text = "";
            _label.ForeColor = Color.White;
            _label.Font = new Font("Arial", 28, FontStyle.Bold);
            _label.AutoSize = false;
            _label.Dock = DockStyle.Fill;
            _label.TextAlign = ContentAlignment.MiddleCenter;

            _form = new MoveableScreenElement(desktopAnchor);
            _form.TopMost = true;
            _form.Left = 0;
            _form.Top = 0;
            _form.Width = 1;
            _form.Height = 1;
            _form.Opacity = 1;
            
            _form.AddControls(_label);

            _color = Color.Black;
        }

        public EventHandler Click
        {
            set
            {
                _form.MyClick += new MoveableScreenElement.MyClickDelegate(value);
            }
        }

        public EventHandler ClickUp
        {
            set
            {
                _form.MyClickUp += new MoveableScreenElement.MyClickDelegate(value);
            }
        }

        public EventHandler RightClick
        {
            set
            {
                _form.MyRightClick += new MoveableScreenElement.MyClickDelegate(value);
            }
        }

        public Form Form
        {
            get { return _form; }
        }

        public Label Label
        {
            get { return _label; }
        }

        public string Text
        {
            get { return _label.Text; }
            set { _label.Text = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                _label.Visible = value;

                if (value)
                    SafeNativeMethods.ShowWindow(_form.Handle, 8);
                else
                    _form.Hide();
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                _form.BackColor = value;
            }
        }

        public double Opacity
        {
            get { return _form.Opacity; }
            set { _form.Opacity = value; }
        }

        public Rectangle Location
        {
            get { return _location; }
            set
            {
                _location = value;
                Layout();
            }
        }

        private void Layout()
        {
            SafeNativeMethods.SetWindowPos(_form.Handle, NativeMethods.HWND_TOPMOST, _location.X, _location.Y, _location.Width, _location.Height, 0x10);
        }

        #region IDisposable Members

        public void Dispose()
        {
            _form.Dispose();
        }

        #endregion
    }
}
