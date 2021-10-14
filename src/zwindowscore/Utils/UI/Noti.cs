using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zwindowscore.Enum;

namespace zwindowscore.Utils.UI
{
    public class Noti : Form
    {
        private Label lblMessage;
        private System.Timers.Timer _autoOffTimer;
        private int _initialStyle;

        public Noti()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;

            BackColor = Color.Black;
            lblMessage.ForeColor = Color.White;
            lblMessage.BackColor = Color.Transparent;
            TopMost = true;
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;

            //Opacity = 1;
            //FadeIn();
            Load += Noti_Load;
        }

        private void Noti_Load(object sender, EventArgs e)
        {
            _initialStyle = Win32Helper.GetWindowLong(this.Handle, -20);
            Win32Helper.SetWindowLong(this.Handle, -20, _initialStyle | 0x80000 | 0x20);
            Win32Helper.SetLayeredWindowAttributes(this.Handle, 0, 100, Win32Helper.LWA_ALPHA);
        }

        public string Message
        {
            set
            {
                lblMessage.Text = value;    
            }
            get
            {
                return lblMessage.Text;
            }
        }

        private async void FadeIn(int interval = 80) 
        {
            while (Opacity < 1.0)
            {
                await Task.Delay(interval);
                Opacity += 0.1;
            }
            Opacity = 1;
        }

        private async void FadeOut(int interval = 80)
        {
            while (Opacity > 0.0)
            {
                await Task.Delay(interval);
                Opacity -= 0.335;
            }
            Opacity = 0;
            Dispose();
        }


        public void ShowFloatingForXMilliSeconds(int milliSeconds) {
            Show();

            if (_autoOffTimer == null) {
                _autoOffTimer = new System.Timers.Timer();
                _autoOffTimer.Elapsed += OnAutoOffTimerElapsed;
                _autoOffTimer.SynchronizingObject = this;
            }
            else
            {
                _autoOffTimer.Stop();
            }
            _autoOffTimer.Interval = milliSeconds;
            _autoOffTimer.Enabled = true;
            _autoOffTimer.Start();
        }


        void OnAutoOffTimerElapsed(Object sender, System.Timers.ElapsedEventArgs ea) {
            if ((_autoOffTimer != null) && _autoOffTimer.Enabled) {
                _autoOffTimer.Enabled = false;
                //FadeOut();
                Dispose();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;

                return Params;
            }
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(15, 15);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(126, 46);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "label1";
            // 
            // Noti
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Noti";
            this.Padding = new System.Windows.Forms.Padding(15);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #region Static

        private static Dictionary<string, Noti> _notis = new Dictionary<string, Noti>();

        public static void ShowMessage(string message, int miliSeconds, int x = 0, int y = 0)
        {
            var mi = 1;

            foreach(var md in Global.MonitorDevices)
            {
                var monitor = Global.Settings.MonitorsLayouts[md.Id];

                Noti current = null;
                bool isNew = false;
                if (_notis.ContainsKey(monitor.Id))
                {
                    current = _notis[monitor.Id];
                }

                if(current != null)
                {
                    if (current.IsDisposed)
                    {
                        isNew = true;
                    }
                }
                else
                {
                    isNew = true;
                }
                if (isNew)
                {
                    current = new Noti();
                    current.Location = new Point(monitor.X + x, monitor.Y + y);
                    _notis[monitor.Id] = current;
                }
            
                //current.Message = $"{message} #{mi}";
                current.Message = message;
                current.ShowFloatingForXMilliSeconds(miliSeconds);

                mi++;
            }
        }


        #endregion
    }
}
