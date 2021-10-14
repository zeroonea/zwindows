using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using zwindows.Utils.UI;

namespace zwindows
{
    public partial class Settings : Form
    {
        bool exit = false;
        static Dictionary<string, List<LayoutHighlighter>> layoutHighlighters = new Dictionary<string, List<LayoutHighlighter>>();

        public Settings()
        {
            InitializeComponent();

            Global.Main = this;

            TopMost = true;
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;

            var tmp = Global.LoadLayoutsFromFile();
            if (!string.IsNullOrEmpty(tmp.Item2))
            {
                Global.Settings = tmp.Item1;
                txtSettings.Text = tmp.Item2;
            }
            else
            {
                txtSettings.Text = JsonConvert.SerializeObject(Global.Settings, Formatting.Indented);
                File.WriteAllText(Global.GetLayoutsFilePath(), txtSettings.Text);
            }

            var fgg = Global.LoadForegroundGroupsFromFile();
            if(fgg != null)
            {
                Global.ForegroundGroups = new BindingList<ForegroundGroup>(fgg);
            }
            else
            {
                for(var i = 0; i < 10; i++)
                {
                    Global.ForegroundGroups.Add(new ForegroundGroup
                    {
                        Name = $"Group {i+1}"
                    });
                }
            }

            lbForegroundGroupWindows.DisplayMember = "Name";
            lbForegroundGroups.DataSource = Global.ForegroundGroups;
            lbForegroundGroups.DisplayMember = "Title";

            lbVD.DataSource = Global.VirtualDesktops;
            lbVD.DisplayMember = "Name";

            lbMonitors.DataSource = Global.MonitorDevices;
            lbMonitors.DisplayMember = "Name";

            lbLayouts.DisplayMember = "Title";
            
            
            txtSelectedWindow.DataBindings.Add("Text", Global.CurrentForegroundWindowText, 
                "Text", true, DataSourceUpdateMode.OnPropertyChanged);

            ctxmNotifyIcon.Opening += CtxmNotifyIcon_Opening;

            this.Load += Settings_Load1;

            MyInit();

            WindowsDesktop.VirtualDesktop.CurrentChanged += VirtualDesktop_CurrentChanged;
            
            Win32Helper.StartEventHook();
        }

        private void Settings_Load1(object sender, EventArgs e)
        {
            Win32Helper.RefreshTabBars();
        }

        private void VirtualDesktop_CurrentChanged(object sender, WindowsDesktop.VirtualDesktopChangedEventArgs e)
        {
            Global.SetCurrentDesktop();
            Console.WriteLine($"Switch to Desktop {Global.CurrentDesktopName}");

            MethodInvoker action = delegate {
                Noti.ShowMessage($"Desktop {Global.CurrentDesktopName}", 2000);
            };
            this.BeginInvoke(action);
        }

        private void CtxmNotifyIcon_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void MyInit()
        {
            Global.SetCurrentDesktop();
            Global.LoadMonitors();
            Global.LoadVirtualDesktops();
            ClearLayoutHighlighters();
        }

        private void MyRefresh()
        {
            Global.SetCurrentDesktop();
            Global.LoadMonitors();
            Global.LoadVirtualDesktops();
            Win32Helper.RefreshTabBars();
            ClearLayoutHighlighters();
        }

        private void LoadLayouts(string desktopName, string monitorId)
        {
            monitorId = monitorId.ToLower();
            if (Global.Settings.MonitorsLayouts.ContainsKey(monitorId))
            {
                var md = Global.Settings.MonitorsLayouts[monitorId];
                lbLayouts.DataSource = md.Layouts.Where(p => p.DesktopName == desktopName || p.DesktopName == "default" || string.IsNullOrEmpty(p.DesktopName)).ToList();
            }
        }

        public static bool ClearLayoutHighlighters(string vdn = null)
        {
            List<LayoutHighlighter> results = null;

            if(string.IsNullOrEmpty(vdn))
            {
                results = layoutHighlighters.Values.SelectMany(p => p).ToList();
                if(results.Count > 0)
                {
                    results.ForEach(p => p.Dispose());
                    layoutHighlighters.Clear();
                    return true;
                }
            }
            else if(layoutHighlighters.ContainsKey(vdn))
            {
                results = layoutHighlighters[vdn];
                if(results.Count > 0)
                { 
                    results.ForEach(p => p.Dispose());
                    layoutHighlighters[vdn].Clear();
                    return true;
                }
            }
            return false;
        }

        public static void ToggleShowHideLayouts(bool forceShow = false, string desktopName = null, string monitorId = null, 
            int[] layoutIndexes = null, bool clearAll = true)
        {
            desktopName = Global.SetCurrentDesktop(desktopName);
            bool flag = clearAll ? !ClearLayoutHighlighters(desktopName) : false;
            if (!layoutHighlighters.ContainsKey(desktopName))
            {
                layoutHighlighters[desktopName] = new List<LayoutHighlighter>();
            }
            
            if(flag || forceShow)
            {
                var i = 0;
                foreach(var mn in Global.Settings.MonitorsLayouts.Keys)
                {
                    if(monitorId != null && mn != monitorId) continue;

                    var monitor = Global.Settings.MonitorsLayouts[mn];
                    if(!monitor.IsAvailable) continue;
                    var layouts = monitor.Layouts.Where(p => (p.MonitorRatio == 0 
                        || (p.MonitorRatio == 1 && monitor.IsPortrait) 
                        || (p.MonitorRatio == 2 && !monitor.IsPortrait)) 
                        &&
                        (string.IsNullOrEmpty(p.DesktopName) 
                            || p.DesktopName == Global.CurrentDesktopName 
                            || p.DesktopName == "default")
                    ).ToList();
                    if(layouts.Count == 0)
                    {
                        layoutIndexes = null;
                        var ml = new MonitorLayout
                        {
                            X = 0,
                            Y = 0,
                            Width = 100,
                            Height = 100,
                            MonitorRatio = monitor.IsPortrait ? 1 : 2
                        };
                        Global.UpdateLayout(monitor, ml);
                        monitor.Layouts.Add(ml);

                        layouts.Add(ml);
                    }
                    foreach(var layout in layouts)
                    {
                        var li = monitor.Layouts.IndexOf(layout);

                        if(layoutIndexes != null && !layoutIndexes.Contains(li)) continue;
                        var hi = new LayoutHighlighter(Global.GetDesktopAnchor(Global.CurrentDesktopName), monitor);
                        hi.DrawRectangle((li + 1).ToString(), monitor.X, monitor.Y, layout, Global.Colors[li]);
                        if(i >= Global.Colors.Count)
                        {
                            i = 0;
                        }
                        layoutHighlighters[desktopName].Add(hi);
                    }
                }
            }
        }

        private void Event(object sender, EventArgs e) { Console.WriteLine("Left mouse click!"); }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            //MouseHook.Stop();
        }

        private void btnShowSettingsFromMemory_Click(object sender, EventArgs e)
        {
            txtSettings.Text = JsonConvert.SerializeObject(Global.Settings, Formatting.Indented);
        }

        private void btnSaveSettingsToFile_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Global.GetLayoutsFilePath(), txtSettings.Text);
        }

        private void btnShowSettingsFromFile_Click(object sender, EventArgs e)
        {
            var filePath = Global.GetLayoutsFilePath();
            if(File.Exists(filePath))
            {
                var settingsJson = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(settingsJson))
                {
                    txtSettings.Text = settingsJson;
                }
            }
            else
            {
                MessageBox.Show($"File {filePath} not found!", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnApplySettings_Click(object sender, EventArgs e)
        {
            Global.Settings = JsonConvert.DeserializeObject<ZWindowsSettings>(txtSettings.Text);
            MyRefresh();
        }

        private void btnDrawLayouts_Click(object sender, EventArgs e)
        {
            ToggleShowHideLayouts();
        }

        private void Settings_Resize(object sender, EventArgs e)
        {
            /*            
            if (this.WindowState == FormWindowState.Minimized)  
            {  
                Hide();  
                //niSettings.Visible = true;                  
            }  */
        }

        private void niSettings_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*this.Left = pX;
            this.Top = pY;*/
            Show();  
            Visible = true;
            this.WindowState = FormWindowState.Normal;  

            //niSettings.Visible = false;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!exit)
            {
                e.Cancel = true;
                /*pX = this.Left;
                pY = this.Top;
                this.Left = -30000;
                this.Top = -30000;*/
                Visible = false;
                //Hide();
                return;
            }
            ctxmNotifyIcon.Visible = false;
            ctxmNotifyIcon.Dispose();
            Global.TabButtonsBars.Values.ToList().ForEach(p => p.Dispose());
            ClearLayoutHighlighters();
        }

        # region notify icon context menu actions

        private void mniExit_Click(object sender, EventArgs e)
        {
            exit = true;
            this.Close();
            Application.Exit();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void mniToggleLayouts_Click(object sender, EventArgs e)
        {
            ToggleShowHideLayouts();
        }

        private void mniRefreshTaskBars_Click(object sender, EventArgs e)
        {
            Win32Helper.RefreshTabBars();
        }
        #endregion

        private void lbForegroundGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            var fg = (ForegroundGroup)lbForegroundGroups.SelectedItem;
            lbForegroundGroupWindows.DataSource = fg.Windows;

            if(fg.Windows.Count > 0)
            { 
                Win32Helper.BringWindowAndItsGroupToFront(fg.Windows[0].Hwnd);
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {

        }

        private void btnAddWindow_Click(object sender, EventArgs e)
        {
            if(Global.CurrentForegroundWindow.Hwnd != IntPtr.Zero)
            {
                var fg = (ForegroundGroup)lbForegroundGroups.SelectedItem;

                if(fg.Windows.Any(p => p.Hwnd == Global.CurrentForegroundWindow.Hwnd))
                {
                    MessageBox.Show($"{fg.Name} already has this window", "Warning", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                fg.Windows.Add(new ForegroundWindow{
                    Hwnd = Global.CurrentForegroundWindow.Hwnd,
                    Name = Global.CurrentForegroundWindow.Name
                });

                lbForegroundGroups.DisplayMember = "";
                lbForegroundGroups.DisplayMember = "Title";
            }
        }

        private void btnToggleDetectWindow_Click(object sender, EventArgs e)
        {
            if (!Global.IsDetectWindow)
            {
                btnToggleDetectWindow.Text = "Cancel Detect";
                Global.CurrentForegroundWindowText.Text = "Cick on any Window to detect, and click `Add Window` to add it into group";
                btnAddWindow.Enabled = true;
            }
            else
            {
                btnToggleDetectWindow.Text = "Detect Window";
                Global.CurrentForegroundWindowText.Text = "";
                btnAddWindow.Enabled = false;
            }

            
            Global.IsDetectWindow = !Global.IsDetectWindow;
        }

        private void btnRemoveWindowInFG_Click(object sender, EventArgs e)
        {
            var fg = (ForegroundGroup)lbForegroundGroups.SelectedItem;
            var w = (ForegroundWindow)lbForegroundGroupWindows.SelectedItem;

            if(fg != null && w != null)
            {
                fg.Windows.Remove(w);

                lbForegroundGroups.DisplayMember = "";
                lbForegroundGroups.DisplayMember = "Title";
            }
        }

        private void btnRefreshWindowsInFG_Click(object sender, EventArgs e)
        {
            var fg = (ForegroundGroup)lbForegroundGroups.SelectedItem;
            if(fg != null)
            {

                for(var i = fg.Windows.Count - 1; i >=0; i--)
                {
                    var w = fg.Windows[i];
                    var title = Win32Helper.GetWindowTitle(w.Hwnd);
                    if (!string.IsNullOrEmpty(title))
                    {
                        w.Name = title;
                    }
                    else
                    {
                        fg.Windows.Remove(w);
                    }
                }

                lbForegroundGroupWindows.DisplayMember = "";
                lbForegroundGroupWindows.DisplayMember = "Name";

                lbForegroundGroups.DisplayMember = "";
                lbForegroundGroups.DisplayMember = "Title";
            }
        }

        private void btnSaveFgGroups_Click(object sender, EventArgs e)
        {
            var json = JsonConvert.SerializeObject(Global.ForegroundGroups, Formatting.Indented);
            File.WriteAllText(Global.GetForegroundGroupsFilePath(), json);
        }

        private void lbVD_SelectedIndexChanged(object sender, EventArgs e)
        {
            var vd = (VirtualDesktopInfo)lbVD.SelectedItem;
            var md = (MonitorDeviceDefinition)lbMonitors.SelectedItem;
            if(vd != null && md != null)
            {
                LoadLayouts(vd.Name, md.Id.ToLower());
            }
        }

        private void lbMonitors_SelectedIndexChanged(object sender, EventArgs e)
        {
            var vd = (VirtualDesktopInfo)lbVD.SelectedItem;
            var md = (MonitorDeviceDefinition)lbMonitors.SelectedItem;
            if(vd != null && md != null)
            {
                LoadLayouts(vd.Name, md.Id.ToLower());
            }
        }

        private void btnShowHideSelectedLayout_Click(object sender, EventArgs e)
        {
            var vd = (VirtualDesktopInfo)lbVD.SelectedItem;
            var md = (MonitorDeviceDefinition)lbMonitors.SelectedItem;
            if(vd != null && md != null)
            {
                var indexes = lbLayouts.SelectedIndices.OfType<int>().ToArray();
                ToggleShowHideLayouts(true, vd.Name, md.Id.ToLower(), indexes);
            }
        }

        private void btnShowHideLayoutList_Click(object sender, EventArgs e)
        {
            var vd = (VirtualDesktopInfo)lbVD.SelectedItem;
            var md = (MonitorDeviceDefinition)lbMonitors.SelectedItem;
            if(vd != null && md != null)
            {
                ToggleShowHideLayouts(true, vd.Name, md.Id.ToLower());
            }
        }

        private void btnClearViewLayouts_Click(object sender, EventArgs e)
        {
            ClearLayoutHighlighters(Global.CurrentDesktopName);
        }

        private void mniToggleTabBarAlwaysOnTop_Click(object sender, EventArgs e)
        {
            foreach(var bar in Global.TabButtonsBars.Values)
            {
                bar.TopMost = !mniToggleTabBarAlwaysOnTop.Checked;
            }
            mniToggleTabBarAlwaysOnTop.Checked = !mniToggleTabBarAlwaysOnTop.Checked;
        }

        private void mniSaveDesktop_Click(object sender, EventArgs e)
        {
            Win32Helper.SaveDesktop();
        }

        private void mniLoadDesktop_Click(object sender, EventArgs e)
        {
            Win32Helper.LoadDesktop();
        }

        private void mniSnapAll_Click(object sender, EventArgs e)
        {
            foreach(var bar in Global.TabButtonsBars.Values)
            {
                foreach(var btn in bar.WindowButtons)
                {
                    btn.Snap();
                }
            }
        }
    }
}
