using System;
using System.Drawing;
using System.Windows.Forms;

namespace zwindowscore.Utils
{
    /// <summary>
    /// This class is responsible to draw bounding rectangle around some location on the screen.
    /// </summary>
    public class ScreenBoundingRectangle: IDisposable
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
        private ScreenText _text;

        public ScreenBoundingRectangle(Form desktopAnchor)
        {
            //initialize instance
            _width = 3;

            _leftRectangle = new ScreenRectangleElement(desktopAnchor);
            _topRectangle = new ScreenRectangleElement(desktopAnchor);
            _rightRectangle = new ScreenRectangleElement(desktopAnchor);
            _bottomRectangle = new ScreenRectangleElement(desktopAnchor);

            _rectangles = new ScreenRectangleElement[] { _leftRectangle, _topRectangle, _rightRectangle, _bottomRectangle };

            _text = new ScreenText(desktopAnchor);
            _text.Click = new System.EventHandler(_textClicked);
        }

        private void _textClicked(object sender, EventArgs e)
        {
            foreach(var rectangle in _rectangles)
            {
                rectangle.Form.TopMost = true;
            }
            _text.Form.TopMost = true;
        }

        public EventHandler TextMoved
        {
            set
            {
                _text.ClickUp = new System.EventHandler(value);
            }
        }

        public EventHandler TextRightClick
        {
            set
            {
                _text.RightClick = new System.EventHandler(value);
            }
        }

        public EventHandler TextClick
        {
            set
            {
                _text.Click = new System.EventHandler(value);
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = _leftRectangle.Visible = _rightRectangle.Visible = 
                _topRectangle.Visible = _bottomRectangle.Visible = _text.Visible = value;}
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = _leftRectangle.Color = _rightRectangle.Color =
              _topRectangle.Color = _bottomRectangle.Color = _text.Color = value;
            }
        }

        public double Opacity
        {
            get { return _leftRectangle.Opacity; }
            set
            {
                _leftRectangle.Opacity = _rightRectangle.Opacity =
              _topRectangle.Opacity = _bottomRectangle.Opacity = _text.Opacity = value;
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

        public Rectangle TextLocation
        {
            get { return _textLocation; }
            set
            {
                _textLocation = value;
                TextLayout();
            }
        }

        public Point TextCurrentLocation
        {
            get
            {
                return _text.Form.Location;
            }
        }

        public string Text
        {
            get { return _text.Text; }
            set
            {
                _text.Text = value;
            }
        }

        public ScreenText Label
        {
            get { return _text; }
        }

        private void Layout()
        {
            _leftRectangle.Location = new Rectangle(_location.Left - _width, _location.Top, _width, _location.Height);
            _topRectangle.Location = new Rectangle(_location.Left - _width, _location.Top - _width, _location.Width + (2 * _width), _width);
            _rightRectangle.Location = new Rectangle(_location.Left + _location.Width, _location.Top, _width, _location.Height);
            _bottomRectangle.Location = new Rectangle(_location.Left - _width, _location.Top + _location.Height, _location.Width + (2 * _width), _width);
            //_text.Location = new Rectangle(_textLocation.Left, _textLocation.Top, _textLocation.Width, _textLocation.Height);
        }

        private void TextLayout()
        {
            _text.Location = new Rectangle(_textLocation.Left, _textLocation.Top, _textLocation.Width, _textLocation.Height);
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (ScreenRectangleElement rectangle in _rectangles)
                rectangle.Dispose();

            _text.Dispose();
        }

        #endregion
    }
}
