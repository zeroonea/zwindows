namespace zwindowscore
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.txtSettings = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDrawLayouts = new System.Windows.Forms.Button();
            this.btnApplySettings = new System.Windows.Forms.Button();
            this.btnShowSettingsFromFile = new System.Windows.Forms.Button();
            this.btnSaveSettingsToFile = new System.Windows.Forms.Button();
            this.btnShowSettingsFromMemory = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.niSettings = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxmNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mniSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniRefreshTaskBars = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSnapAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mniToggleTabBarAlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.mniToggleTabBarVisible = new System.Windows.Forms.ToolStripMenuItem();
            this.mniResetAllWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniSaveDesktop = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadDesktop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniToggleLayouts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniTaskBarHide = new System.Windows.Forms.ToolStripMenuItem();
            this.mniCenterTaskbarIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTransparentTaskbar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tabLayouts = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.lbForegroundGroupWindows = new System.Windows.Forms.ListBox();
            this.pnlFGs = new System.Windows.Forms.Panel();
            this.lbForegroundGroups = new System.Windows.Forms.ListBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnSaveFgGroups = new System.Windows.Forms.Button();
            this.btnRefreshWindowsInFG = new System.Windows.Forms.Button();
            this.btnRemoveWindowInFG = new System.Windows.Forms.Button();
            this.btnToggleDetectWindow = new System.Windows.Forms.Button();
            this.btnAddWindow = new System.Windows.Forms.Button();
            this.txtSelectedWindow = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.ctxmNotifyIcon.SuspendLayout();
            this.tabLayouts.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel8.SuspendLayout();
            this.pnlFGs.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSettings
            // 
            this.txtSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSettings.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSettings.Location = new System.Drawing.Point(23, 23);
            this.txtSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSettings.Multiline = true;
            this.txtSettings.Name = "txtSettings";
            this.txtSettings.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSettings.Size = new System.Drawing.Size(1268, 649);
            this.txtSettings.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.txtSettings);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(23);
            this.panel1.Size = new System.Drawing.Size(1314, 695);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnDrawLayouts);
            this.panel2.Controls.Add(this.btnApplySettings);
            this.panel2.Controls.Add(this.btnShowSettingsFromFile);
            this.panel2.Controls.Add(this.btnSaveSettingsToFile);
            this.panel2.Controls.Add(this.btnShowSettingsFromMemory);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 695);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(29);
            this.panel2.Size = new System.Drawing.Size(1314, 44);
            this.panel2.TabIndex = 2;
            // 
            // btnDrawLayouts
            // 
            this.btnDrawLayouts.Location = new System.Drawing.Point(22, 7);
            this.btnDrawLayouts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDrawLayouts.Name = "btnDrawLayouts";
            this.btnDrawLayouts.Size = new System.Drawing.Size(138, 27);
            this.btnDrawLayouts.TabIndex = 4;
            this.btnDrawLayouts.Text = "Show/Hide Layouts";
            this.btnDrawLayouts.UseVisualStyleBackColor = true;
            this.btnDrawLayouts.Click += new System.EventHandler(this.btnDrawLayouts_Click);
            // 
            // btnApplySettings
            // 
            this.btnApplySettings.Location = new System.Drawing.Point(699, 7);
            this.btnApplySettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnApplySettings.Name = "btnApplySettings";
            this.btnApplySettings.Size = new System.Drawing.Size(139, 27);
            this.btnApplySettings.TabIndex = 3;
            this.btnApplySettings.Text = "Apply Settings";
            this.btnApplySettings.UseVisualStyleBackColor = true;
            this.btnApplySettings.Click += new System.EventHandler(this.btnApplySettings_Click);
            // 
            // btnShowSettingsFromFile
            // 
            this.btnShowSettingsFromFile.Location = new System.Drawing.Point(372, 7);
            this.btnShowSettingsFromFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnShowSettingsFromFile.Name = "btnShowSettingsFromFile";
            this.btnShowSettingsFromFile.Size = new System.Drawing.Size(156, 27);
            this.btnShowSettingsFromFile.TabIndex = 2;
            this.btnShowSettingsFromFile.Text = "Show Settings From File";
            this.btnShowSettingsFromFile.UseVisualStyleBackColor = true;
            this.btnShowSettingsFromFile.Click += new System.EventHandler(this.btnShowSettingsFromFile_Click);
            // 
            // btnSaveSettingsToFile
            // 
            this.btnSaveSettingsToFile.Location = new System.Drawing.Point(536, 7);
            this.btnSaveSettingsToFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSaveSettingsToFile.Name = "btnSaveSettingsToFile";
            this.btnSaveSettingsToFile.Size = new System.Drawing.Size(156, 27);
            this.btnSaveSettingsToFile.TabIndex = 1;
            this.btnSaveSettingsToFile.Text = "Save To File";
            this.btnSaveSettingsToFile.UseVisualStyleBackColor = true;
            this.btnSaveSettingsToFile.Click += new System.EventHandler(this.btnSaveSettingsToFile_Click);
            // 
            // btnShowSettingsFromMemory
            // 
            this.btnShowSettingsFromMemory.Location = new System.Drawing.Point(167, 7);
            this.btnShowSettingsFromMemory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnShowSettingsFromMemory.Name = "btnShowSettingsFromMemory";
            this.btnShowSettingsFromMemory.Size = new System.Drawing.Size(198, 27);
            this.btnShowSettingsFromMemory.TabIndex = 0;
            this.btnShowSettingsFromMemory.Text = "Show Settings From Memory";
            this.btnShowSettingsFromMemory.UseVisualStyleBackColor = true;
            this.btnShowSettingsFromMemory.Click += new System.EventHandler(this.btnShowSettingsFromMemory_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(4, 3);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1314, 739);
            this.panel3.TabIndex = 3;
            // 
            // niSettings
            // 
            this.niSettings.ContextMenuStrip = this.ctxmNotifyIcon;
            this.niSettings.Icon = ((System.Drawing.Icon)(resources.GetObject("niSettings.Icon")));
            this.niSettings.Text = "ZWindows Settings";
            this.niSettings.Visible = true;
            this.niSettings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.niSettings_MouseDoubleClick);
            // 
            // ctxmNotifyIcon
            // 
            this.ctxmNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniSettings,
            this.toolStripSeparator5,
            this.mniRefreshTaskBars,
            this.mniSnapAll,
            this.mniToggleTabBarAlwaysOnTop,
            this.mniToggleTabBarVisible,
            this.mniResetAllWindows,
            this.toolStripSeparator4,
            this.mniSaveDesktop,
            this.mniLoadDesktop,
            this.toolStripSeparator2,
            this.mniToggleLayouts,
            this.toolStripSeparator1,
            this.mniTaskBarHide,
            this.mniCenterTaskbarIcons,
            this.mniTransparentTaskbar,
            this.toolStripSeparator3,
            this.mniExit});
            this.ctxmNotifyIcon.Name = "ctxmNotifyIcon";
            this.ctxmNotifyIcon.Size = new System.Drawing.Size(195, 320);
            // 
            // mniSettings
            // 
            this.mniSettings.Name = "mniSettings";
            this.mniSettings.Size = new System.Drawing.Size(194, 22);
            this.mniSettings.Text = "Settings";
            this.mniSettings.Click += new System.EventHandler(this.mniSettings_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(191, 6);
            // 
            // mniRefreshTaskBars
            // 
            this.mniRefreshTaskBars.Name = "mniRefreshTaskBars";
            this.mniRefreshTaskBars.Size = new System.Drawing.Size(194, 22);
            this.mniRefreshTaskBars.Text = "Refresh TabBars";
            this.mniRefreshTaskBars.Click += new System.EventHandler(this.mniRefreshTaskBars_Click);
            // 
            // mniSnapAll
            // 
            this.mniSnapAll.Name = "mniSnapAll";
            this.mniSnapAll.Size = new System.Drawing.Size(194, 22);
            this.mniSnapAll.Text = "Snap All";
            this.mniSnapAll.Click += new System.EventHandler(this.mniSnapAll_Click);
            // 
            // mniToggleTabBarAlwaysOnTop
            // 
            this.mniToggleTabBarAlwaysOnTop.Checked = true;
            this.mniToggleTabBarAlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mniToggleTabBarAlwaysOnTop.Name = "mniToggleTabBarAlwaysOnTop";
            this.mniToggleTabBarAlwaysOnTop.Size = new System.Drawing.Size(194, 22);
            this.mniToggleTabBarAlwaysOnTop.Text = "TabBars Topmost";
            this.mniToggleTabBarAlwaysOnTop.Click += new System.EventHandler(this.mniToggleTabBarAlwaysOnTop_Click);
            // 
            // mniToggleTabBarVisible
            // 
            this.mniToggleTabBarVisible.Checked = true;
            this.mniToggleTabBarVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mniToggleTabBarVisible.Name = "mniToggleTabBarVisible";
            this.mniToggleTabBarVisible.Size = new System.Drawing.Size(194, 22);
            this.mniToggleTabBarVisible.Text = "TabBars Display";
            this.mniToggleTabBarVisible.Click += new System.EventHandler(this.mniToggleTabBarVisible_Click);
            // 
            // mniResetAllWindows
            // 
            this.mniResetAllWindows.Name = "mniResetAllWindows";
            this.mniResetAllWindows.Size = new System.Drawing.Size(194, 22);
            this.mniResetAllWindows.Text = "Reset All Windows";
            this.mniResetAllWindows.Click += new System.EventHandler(this.mniResetAllWindows_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(191, 6);
            // 
            // mniSaveDesktop
            // 
            this.mniSaveDesktop.Name = "mniSaveDesktop";
            this.mniSaveDesktop.Size = new System.Drawing.Size(194, 22);
            this.mniSaveDesktop.Text = "Save Desktop";
            this.mniSaveDesktop.Click += new System.EventHandler(this.mniSaveDesktop_Click);
            // 
            // mniLoadDesktop
            // 
            this.mniLoadDesktop.Name = "mniLoadDesktop";
            this.mniLoadDesktop.Size = new System.Drawing.Size(194, 22);
            this.mniLoadDesktop.Text = "Load Desktop";
            this.mniLoadDesktop.Click += new System.EventHandler(this.mniLoadDesktop_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(191, 6);
            // 
            // mniToggleLayouts
            // 
            this.mniToggleLayouts.Name = "mniToggleLayouts";
            this.mniToggleLayouts.Size = new System.Drawing.Size(194, 22);
            this.mniToggleLayouts.Text = "Toggle Display Layouts";
            this.mniToggleLayouts.Click += new System.EventHandler(this.mniToggleLayouts_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(191, 6);
            // 
            // mniTaskBarHide
            // 
            this.mniTaskBarHide.Name = "mniTaskBarHide";
            this.mniTaskBarHide.Size = new System.Drawing.Size(194, 22);
            this.mniTaskBarHide.Text = "Hide Taskbar";
            this.mniTaskBarHide.Click += new System.EventHandler(this.mniTaskBarHide_Click);
            // 
            // mniCenterTaskbarIcons
            // 
            this.mniCenterTaskbarIcons.Name = "mniCenterTaskbarIcons";
            this.mniCenterTaskbarIcons.Size = new System.Drawing.Size(194, 22);
            this.mniCenterTaskbarIcons.Text = "Center Taskbar Icons";
            this.mniCenterTaskbarIcons.Click += new System.EventHandler(this.mniCenterTaskbarIcons_Click);
            // 
            // mniTransparentTaskbar
            // 
            this.mniTransparentTaskbar.Name = "mniTransparentTaskbar";
            this.mniTransparentTaskbar.Size = new System.Drawing.Size(194, 22);
            this.mniTransparentTaskbar.Text = "Transparent Taskbar";
            this.mniTransparentTaskbar.Click += new System.EventHandler(this.mniTransparentTaskbar_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(191, 6);
            // 
            // mniExit
            // 
            this.mniExit.Name = "mniExit";
            this.mniExit.Size = new System.Drawing.Size(194, 22);
            this.mniExit.Text = "Exit";
            this.mniExit.Click += new System.EventHandler(this.mniExit_Click);
            // 
            // tabLayouts
            // 
            this.tabLayouts.Controls.Add(this.tabPage1);
            this.tabLayouts.Controls.Add(this.tabPage2);
            this.tabLayouts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLayouts.Location = new System.Drawing.Point(0, 0);
            this.tabLayouts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabLayouts.Name = "tabLayouts";
            this.tabLayouts.SelectedIndex = 0;
            this.tabLayouts.Size = new System.Drawing.Size(1330, 773);
            this.tabLayouts.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage1.Size = new System.Drawing.Size(1322, 745);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel4);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage2.Size = new System.Drawing.Size(1322, 745);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Foreground Groups";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(4, 3);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(29);
            this.panel4.Size = new System.Drawing.Size(1314, 739);
            this.panel4.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.lbForegroundGroupWindows);
            this.panel8.Controls.Add(this.pnlFGs);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(29, 29);
            this.panel8.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1256, 624);
            this.panel8.TabIndex = 3;
            // 
            // lbForegroundGroupWindows
            // 
            this.lbForegroundGroupWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbForegroundGroupWindows.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbForegroundGroupWindows.FormattingEnabled = true;
            this.lbForegroundGroupWindows.ItemHeight = 18;
            this.lbForegroundGroupWindows.Location = new System.Drawing.Point(251, 0);
            this.lbForegroundGroupWindows.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lbForegroundGroupWindows.Name = "lbForegroundGroupWindows";
            this.lbForegroundGroupWindows.ScrollAlwaysVisible = true;
            this.lbForegroundGroupWindows.Size = new System.Drawing.Size(1005, 624);
            this.lbForegroundGroupWindows.TabIndex = 0;
            // 
            // pnlFGs
            // 
            this.pnlFGs.Controls.Add(this.lbForegroundGroups);
            this.pnlFGs.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFGs.Location = new System.Drawing.Point(0, 0);
            this.pnlFGs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlFGs.Name = "pnlFGs";
            this.pnlFGs.Padding = new System.Windows.Forms.Padding(0, 0, 29, 0);
            this.pnlFGs.Size = new System.Drawing.Size(251, 624);
            this.pnlFGs.TabIndex = 0;
            // 
            // lbForegroundGroups
            // 
            this.lbForegroundGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbForegroundGroups.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbForegroundGroups.FormattingEnabled = true;
            this.lbForegroundGroups.ItemHeight = 18;
            this.lbForegroundGroups.Location = new System.Drawing.Point(0, 0);
            this.lbForegroundGroups.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lbForegroundGroups.Name = "lbForegroundGroups";
            this.lbForegroundGroups.ScrollAlwaysVisible = true;
            this.lbForegroundGroups.Size = new System.Drawing.Size(222, 624);
            this.lbForegroundGroups.TabIndex = 0;
            this.lbForegroundGroups.SelectedIndexChanged += new System.EventHandler(this.lbForegroundGroups_SelectedIndexChanged);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnSaveFgGroups);
            this.panel7.Controls.Add(this.btnRefreshWindowsInFG);
            this.panel7.Controls.Add(this.btnRemoveWindowInFG);
            this.panel7.Controls.Add(this.btnToggleDetectWindow);
            this.panel7.Controls.Add(this.btnAddWindow);
            this.panel7.Controls.Add(this.txtSelectedWindow);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(29, 653);
            this.panel7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1256, 57);
            this.panel7.TabIndex = 2;
            // 
            // btnSaveFgGroups
            // 
            this.btnSaveFgGroups.Location = new System.Drawing.Point(4, 27);
            this.btnSaveFgGroups.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSaveFgGroups.Name = "btnSaveFgGroups";
            this.btnSaveFgGroups.Size = new System.Drawing.Size(149, 27);
            this.btnSaveFgGroups.TabIndex = 7;
            this.btnSaveFgGroups.Text = "Save Groups";
            this.btnSaveFgGroups.UseVisualStyleBackColor = true;
            this.btnSaveFgGroups.Click += new System.EventHandler(this.btnSaveFgGroups_Click);
            // 
            // btnRefreshWindowsInFG
            // 
            this.btnRefreshWindowsInFG.Location = new System.Drawing.Point(1124, 27);
            this.btnRefreshWindowsInFG.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRefreshWindowsInFG.Name = "btnRefreshWindowsInFG";
            this.btnRefreshWindowsInFG.Size = new System.Drawing.Size(120, 27);
            this.btnRefreshWindowsInFG.TabIndex = 6;
            this.btnRefreshWindowsInFG.Text = "Refresh Windows";
            this.btnRefreshWindowsInFG.UseVisualStyleBackColor = true;
            this.btnRefreshWindowsInFG.Click += new System.EventHandler(this.btnRefreshWindowsInFG_Click);
            // 
            // btnRemoveWindowInFG
            // 
            this.btnRemoveWindowInFG.Location = new System.Drawing.Point(996, 27);
            this.btnRemoveWindowInFG.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRemoveWindowInFG.Name = "btnRemoveWindowInFG";
            this.btnRemoveWindowInFG.Size = new System.Drawing.Size(120, 27);
            this.btnRemoveWindowInFG.TabIndex = 5;
            this.btnRemoveWindowInFG.Text = "Remove Window";
            this.btnRemoveWindowInFG.UseVisualStyleBackColor = true;
            this.btnRemoveWindowInFG.Click += new System.EventHandler(this.btnRemoveWindowInFG_Click);
            // 
            // btnToggleDetectWindow
            // 
            this.btnToggleDetectWindow.Location = new System.Drawing.Point(251, 27);
            this.btnToggleDetectWindow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnToggleDetectWindow.Name = "btnToggleDetectWindow";
            this.btnToggleDetectWindow.Size = new System.Drawing.Size(144, 27);
            this.btnToggleDetectWindow.TabIndex = 4;
            this.btnToggleDetectWindow.Text = "Detect Window";
            this.btnToggleDetectWindow.UseVisualStyleBackColor = true;
            this.btnToggleDetectWindow.Click += new System.EventHandler(this.btnToggleDetectWindow_Click);
            // 
            // btnAddWindow
            // 
            this.btnAddWindow.Enabled = false;
            this.btnAddWindow.Location = new System.Drawing.Point(872, 28);
            this.btnAddWindow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAddWindow.Name = "btnAddWindow";
            this.btnAddWindow.Size = new System.Drawing.Size(118, 27);
            this.btnAddWindow.TabIndex = 3;
            this.btnAddWindow.Text = "Add Window";
            this.btnAddWindow.UseVisualStyleBackColor = true;
            this.btnAddWindow.Click += new System.EventHandler(this.btnAddWindow_Click);
            // 
            // txtSelectedWindow
            // 
            this.txtSelectedWindow.Location = new System.Drawing.Point(401, 30);
            this.txtSelectedWindow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSelectedWindow.Name = "txtSelectedWindow";
            this.txtSelectedWindow.ReadOnly = true;
            this.txtSelectedWindow.Size = new System.Drawing.Size(462, 23);
            this.txtSelectedWindow.TabIndex = 2;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1330, 773);
            this.Controls.Add(this.tabLayouts);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Settings";
            this.Text = "ZWindows";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            this.Load += new System.EventHandler(this.Settings_Load);
            this.Resize += new System.EventHandler(this.Settings_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ctxmNotifyIcon.ResumeLayout(false);
            this.tabLayouts.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.pnlFGs.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSettings;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnShowSettingsFromMemory;
        private System.Windows.Forms.Button btnSaveSettingsToFile;
        private System.Windows.Forms.Button btnShowSettingsFromFile;
        private System.Windows.Forms.Button btnApplySettings;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnDrawLayouts;
        private System.Windows.Forms.NotifyIcon niSettings;
        private System.Windows.Forms.ContextMenuStrip ctxmNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem mniExit;
        private System.Windows.Forms.ToolStripMenuItem mniToggleLayouts;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabControl tabLayouts;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel pnlFGs;
        private System.Windows.Forms.ListBox lbForegroundGroups;
        private System.Windows.Forms.ListBox lbForegroundGroupWindows;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnAddWindow;
        private System.Windows.Forms.TextBox txtSelectedWindow;
        private System.Windows.Forms.Button btnToggleDetectWindow;
        private System.Windows.Forms.Button btnRemoveWindowInFG;
        private System.Windows.Forms.Button btnRefreshWindowsInFG;
        private System.Windows.Forms.Button btnSaveFgGroups;
        private System.Windows.Forms.ToolStripMenuItem mniRefreshTaskBars;
        private System.Windows.Forms.ToolStripMenuItem mniToggleTabBarAlwaysOnTop;
        private System.Windows.Forms.ToolStripMenuItem mniSaveDesktop;
        private System.Windows.Forms.ToolStripMenuItem mniLoadDesktop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniSnapAll;
        private System.Windows.Forms.ToolStripMenuItem mniToggleTabBarVisible;
        private System.Windows.Forms.ToolStripMenuItem mniTaskBarHide;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniCenterTaskbarIcons;
        private System.Windows.Forms.ToolStripMenuItem mniTransparentTaskbar;
        private System.Windows.Forms.ToolStripMenuItem mniResetAllWindows;
        private System.Windows.Forms.ToolStripMenuItem mniSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}

