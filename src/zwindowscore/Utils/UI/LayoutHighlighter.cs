using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using zwindowscore.Options;
using zwindowscore.Utils;

namespace zwindowscore
{
    public class LayoutHighlighter
    {
        ScreenBoundingRectangle _rectangle;
        int _borderSize = 5;
        MonitorDevice _monitor;
        MonitorLayout _layout;
        int textWidth = 100;
        int textHeight = 80;
        int _monitorX = 0;
        int _monitorY = 0;
        float _opacity = 0.2f;
        ContextMenuStrip _ctxTabBtnMenu;

        public LayoutHighlighter(Form desktopAnchor, MonitorDevice device)
        {
            _monitor = device;
            _rectangle = new ScreenBoundingRectangle(desktopAnchor);
        }

        public void DrawRectangle(string text, int monitorX, int monitorY, MonitorLayout layout, string color)
        {
            _layout = layout;
            _layout._rectangle = _rectangle;
            _monitorX = monitorX;
            _monitorY = monitorY;

            var left = layout.Left != 0 ? layout.Left - 1 : layout.Left;
            var top = layout.Top != 0 ? layout.Top - 1 : layout.Top;

            var rect = Rectangle.FromLTRB(
                monitorX + left + _borderSize, 
                monitorY + top + _borderSize, 
                
                monitorX + layout.Right - _borderSize, 
                monitorY + layout.Bottom - _borderSize
            );

            
            var textRect = new Rectangle(
                monitorX + layout.DockX.Value,
                monitorY + layout.DockY.Value - textHeight/2,
                textWidth, 
                textHeight
            );

            _rectangle.TextMoved = TextMoved;
            _rectangle.TextClick = TextClick;
            _rectangle.TextRightClick = TextRightClick;
            
            _ctxTabBtnMenu = new ContextMenuStrip();
            _ctxTabBtnMenu.ItemClicked += _ctxTabBtnMenu_ItemClicked;
            _ctxTabBtnMenu.Opening += _ctxTabBtnMenu_Opening;

            _rectangle.Label.Form.ContextMenuStrip = _ctxTabBtnMenu;

            _rectangle.LineWidth = _borderSize;
            _rectangle.Color = Global.GetColor(color);
            _rectangle.Opacity = 1;
            _rectangle.Text = text;
            _rectangle.TextLocation = textRect;
            _rectangle.Location = rect;
            _rectangle.Visible = true;
            _rectangle.Opacity = _opacity;
        }

        public void UpdateRectangle()
        {
            var layout = _layout;
            var monitorX = _monitor.X;
            var monitorY = _monitor.Y;

            var left = layout.Left != 0 ? layout.Left - 1 : layout.Left;
            var top = layout.Top != 0 ? layout.Top - 1 : layout.Top;

            var rect = Rectangle.FromLTRB(
                monitorX + left + _borderSize, 
                monitorY + top + _borderSize, 
                
                monitorX + layout.Right - _borderSize, 
                monitorY + layout.Bottom - _borderSize
            );

            _rectangle.Location = rect;
        }

        private void _ctxTabBtnMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _ctxTabBtnMenu.Items.Clear();
            _ctxTabBtnMenu.Items.Add($"Update Layout #{_rectangle.Text} (X: {_layout.X}%, Y: {_layout.Y}%, Width: {_layout.Width}%, Height: {_layout.Height}%)");
            //if(Global.CountLayouts(_monitor) > 1)
            //{
                _ctxTabBtnMenu.Items.Add(new ToolStripSeparator());
                _ctxTabBtnMenu.Items.Add($"Remove Layout #{_rectangle.Text}");
            //}
            _ctxTabBtnMenu.Items.Add(new ToolStripSeparator());
            _ctxTabBtnMenu.Items.Add("Add New Layout");

            TextClick(null, null);
            e.Cancel = false;
        }

        private void _ctxTabBtnMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var text = e.ClickedItem.Text;
            if (text == $"Remove Layout #{_rectangle.Text}")
            {
                //if(Global.CountLayouts(_monitor) <= 1)
                //{
                //    return;
                //}
                /*var confirmResult =  MessageBox.Show(_rectangle.Label.Form, $"Are you sure to delete layout #{_rectangle.Text}?", "Confirm Delete!",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    _device.Layouts.Remove(_layout);
                    Dispose();
                }*/
                _monitor.Layouts.Remove(_layout);
                Dispose();
            }
            else if (text == $"Add New Layout")
            {
                var l = new MonitorLayout
                {
                    DesktopName = Global.CurrentDesktopName,
                    X = 0,
                    Y = 0,
                    Width = 50,
                    Height = 50,
                    MonitorRatio = _monitor.IsPortrait ? 1 : 2
                };
                Global.UpdateLayout(_monitor, l);
                _monitor.Layouts.Add(l);

                Settings.ToggleShowHideLayouts(true, null, _monitor.Id, 
                    new int[] { _monitor.Layouts.IndexOf(l) }, 
                    false);
            }
            else
            {
                if(LayoutEditor.instance == null)
                { 
                    var le = new LayoutEditor();
                    LinkLayoutEditor(le);
                    LayoutEditor.instance = le;
                }
            }
        }

        private void ValueChangedCallback()
        {
            Global.UpdateLayout(_monitor, _layout);
            UpdateRectangle();
        }

        private void TextMoved(object sender, EventArgs e)
        {
            _layout.DockX = _rectangle.TextCurrentLocation.X - _monitorX;
            _layout.DockY = _rectangle.TextCurrentLocation.Y + textHeight / 2 - _monitorY;
        }

        private void TextRightClick(object sender, EventArgs e)
        {
            //TextClick(sender, e);
        }

        private void TextClick(object sender, EventArgs e)
        {
            _monitor.Layouts.Where(p => p._rectangle != null && p._rectangle != _rectangle)
                .ToList()
                .ForEach(p => p._rectangle.Opacity = _opacity);
            _rectangle.Opacity = 1;

            if(LayoutEditor.instance != null)
            {
                LinkLayoutEditor(LayoutEditor.instance);
            }
        }

        private void LinkLayoutEditor(LayoutEditor le)
        {
            le.ValueChangedCallback = ValueChangedCallback;
            le.Text = $"Layout #{_rectangle.Text}";
            le.SetData(_monitor, _layout);
            le.StartPosition = FormStartPosition.CenterScreen;
            le.Show();
        }

        public void Dispose()
        {
            _ctxTabBtnMenu.Dispose();
            _rectangle.Dispose();
        }
    }
}
