using System;
using System.Drawing;
using System.Windows.Forms;

namespace zwindowscore.Utils
{
    /// <summary>
    /// This class is responsible to draw bounding rectangle around some location on the screen.
    /// </summary>
    public class LayoutBoundingRectangle: IDisposable
    {
        //we will remember values for properties Width, Visible, Color, Location, ToolTipText
        private int _width;
        private bool _visible;
        private Color _color;
        private Rectangle _location;
        private Rectangle _textLocation;

        //for bounding rect we will use 4 rectangles
        private ScreenRectangleElement _leftRectangle, _bottomRectangle, _rightRectangle, _topRectangle;
        private ScreenRectangleElement[] _rectangles;

        public LayoutBoundingRectangle()
        {
            //initialize instance
            _width = 3;

            _leftRectangle = new ScreenRectangleElement(null);
            _topRectangle = new ScreenRectangleElement(null);
            _rightRectangle = new ScreenRectangleElement(null);
            _bottomRectangle = new ScreenRectangleElement(null);

            _rectangles = new ScreenRectangleElement[] { _leftRectangle, _topRectangle, _rightRectangle, _bottomRectangle };
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = _leftRectangle.Visible = _rightRectangle.Visible = 
                _topRectangle.Visible = _bottomRectangle.Visible = value;}
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = _leftRectangle.Color = _rightRectangle.Color =
              _topRectangle.Color = _bottomRectangle.Color = value;
            }
        }

        public double Opacity
        {
            get { return _leftRectangle.Opacity; }
            set
            {
                _leftRectangle.Opacity = _rightRectangle.Opacity =
              _topRectangle.Opacity = _bottomRectangle.Opacity = value;
            }
        }

        public int LineWidth
        {
            get { return _width; }
            set { _width = value; }
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
            _leftRectangle.Location = new Rectangle(_location.Left - _width, _location.Top, _width, _location.Height);
            _topRectangle.Location = new Rectangle(_location.Left - _width, _location.Top - _width, _location.Width + (2 * _width), _width);
            _rightRectangle.Location = new Rectangle(_location.Left + _location.Width, _location.Top, _width, _location.Height);
            _bottomRectangle.Location = new Rectangle(_location.Left - _width, _location.Top + _location.Height, _location.Width + (2 * _width), _width);
            foreach(var rectangle in _rectangles)
            {
                rectangle.Form.TopMost = true;
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            foreach (ScreenRectangleElement rectangle in _rectangles)
                rectangle.Dispose();
        }

        #endregion
    }
}
