using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zwindowscore.Options;

namespace zwindowscore
{
    public partial class LayoutEditor : Form
    {
        MonitorDevice _device;
        MonitorLayout _layout;

        public delegate void ValueChanged();

        public ValueChanged ValueChangedCallback;

        private bool noChanged = false;

        public LayoutEditor()
        {
            InitializeComponent();

            nX.ValueChanged += ValueChangedEvent;
            nY.ValueChanged += ValueChangedEvent;
            nW.ValueChanged += ValueChangedEvent;
            nH.ValueChanged += ValueChangedEvent;

            nOffsetL.ValueChanged += ValueChangedEvent;
            nOffsetR.ValueChanged += ValueChangedEvent;
            nOffsetT.ValueChanged += ValueChangedEvent;
            nOffsetB.ValueChanged += ValueChangedEvent;

            cbDesktops.SelectedIndexChanged += ValueChangedEvent;
        }

        private void ValueChangedEvent(object sender, EventArgs e)
        {
            if(noChanged) return;

            _layout.X = (int)nX.Value;
            _layout.Y = (int)nY.Value;
            _layout.Width = (int)nW.Value;
            _layout.Height = (int)nH.Value;

            _layout.LeftOffset = (int)nOffsetL.Value;
            _layout.RightOffset = (int)nOffsetR.Value;
            _layout.TopOffset = (int)nOffsetT.Value;
            _layout.BottomOffset = (int)nOffsetB.Value;

            var dn = cbDesktops.SelectedItem.ToString();
            if(dn == "[All Desktops]") dn = "default";
            _layout.DesktopName = dn;

            if(ValueChangedCallback != null)
            {
                ValueChangedCallback();
            }
        }

        public void SetData(MonitorDevice device, MonitorLayout layout)
        {
            noChanged = true;

            _device = device;
            _layout = layout;

            cbDesktops.Items.Clear();
            cbDesktops.Items.Add(Global.CurrentDesktopName);
            cbDesktops.Items.Add("[All Desktops]");
            //Global.VirtualDesktops.ToList().ForEach(p => cbDesktops.Items.Add(p.Name));
            if(layout.DesktopName == "default")
            {
                cbDesktops.SelectedItem = "[All Desktops]";
            }
            else
            {
                cbDesktops.SelectedItem = layout.DesktopName;
            }

            lblDisplayWidth.Text = device.Width.ToString();
            lblDisplayHeight.Text = device.Height.ToString();

            nX.Value = layout.X;
            nY.Value = layout.Y;
            nW.Value = layout.Width;
            nH.Value = layout.Height;

            nOffsetL.Value = layout.LeftOffset;
            nOffsetR.Value = layout.RightOffset;
            nOffsetT.Value = layout.TopOffset;
            nOffsetB.Value = layout.BottomOffset;

            noChanged = false;
        }
    }
}
