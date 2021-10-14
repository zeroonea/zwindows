namespace zwindows
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
            this.mniRefreshTaskBars = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSnapAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mniToggleTabBarAlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniSaveDesktop = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadDesktop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniToggleLayouts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tabLayouts = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.lbLayouts = new System.Windows.Forms.ListBox();
            this.lbMonitors = new System.Windows.Forms.ListBox();
            this.lbVD = new System.Windows.Forms.ListBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnClearViewLayouts = new System.Windows.Forms.Button();
            this.btnShowHideSelectedLayout = new System.Windows.Forms.Button();
            this.btnShowHideLayoutList = new System.Windows.Forms.Button();
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
            this.tabPage3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel5.SuspendLayout();
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
            this.txtSettings.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSettings.Location = new System.Drawing.Point(20, 20);
            this.txtSettings.Multiline = true;
            this.txtSettings.Name = "txtSettings";
            this.txtSettings.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSettings.Size = new System.Drawing.Size(1086, 560);
            this.txtSettings.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.txtSettings);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(20);
            this.panel1.Size = new System.Drawing.Size(1126, 600);
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
            this.panel2.Location = new System.Drawing.Point(0, 600);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(25);
            this.panel2.Size = new System.Drawing.Size(1126, 38);
            this.panel2.TabIndex = 2;
            // 
            // btnDrawLayouts
            // 
            this.btnDrawLayouts.Location = new System.Drawing.Point(19, 6);
            this.btnDrawLayouts.Name = "btnDrawLayouts";
            this.btnDrawLayouts.Size = new System.Drawing.Size(118, 23);
            this.btnDrawLayouts.TabIndex = 4;
            this.btnDrawLayouts.Text = "Show/Hide Layouts";
            this.btnDrawLayouts.UseVisualStyleBackColor = true;
            this.btnDrawLayouts.Click += new System.EventHandler(this.btnDrawLayouts_Click);
            // 
            // btnApplySettings
            // 
            this.btnApplySettings.Location = new System.Drawing.Point(599, 6);
            this.btnApplySettings.Name = "btnApplySettings";
            this.btnApplySettings.Size = new System.Drawing.Size(119, 23);
            this.btnApplySettings.TabIndex = 3;
            this.btnApplySettings.Text = "Apply Settings";
            this.btnApplySettings.UseVisualStyleBackColor = true;
            this.btnApplySettings.Click += new System.EventHandler(this.btnApplySettings_Click);
            // 
            // btnShowSettingsFromFile
            // 
            this.btnShowSettingsFromFile.Location = new System.Drawing.Point(319, 6);
            this.btnShowSettingsFromFile.Name = "btnShowSettingsFromFile";
            this.btnShowSettingsFromFile.Size = new System.Drawing.Size(134, 23);
            this.btnShowSettingsFromFile.TabIndex = 2;
            this.btnShowSettingsFromFile.Text = "Show Settings From File";
            this.btnShowSettingsFromFile.UseVisualStyleBackColor = true;
            this.btnShowSettingsFromFile.Click += new System.EventHandler(this.btnShowSettingsFromFile_Click);
            // 
            // btnSaveSettingsToFile
            // 
            this.btnSaveSettingsToFile.Location = new System.Drawing.Point(459, 6);
            this.btnSaveSettingsToFile.Name = "btnSaveSettingsToFile";
            this.btnSaveSettingsToFile.Size = new System.Drawing.Size(134, 23);
            this.btnSaveSettingsToFile.TabIndex = 1;
            this.btnSaveSettingsToFile.Text = "Save To File";
            this.btnSaveSettingsToFile.UseVisualStyleBackColor = true;
            this.btnSaveSettingsToFile.Click += new System.EventHandler(this.btnSaveSettingsToFile_Click);
            // 
            // btnShowSettingsFromMemory
            // 
            this.btnShowSettingsFromMemory.Location = new System.Drawing.Point(143, 6);
            this.btnShowSettingsFromMemory.Name = "btnShowSettingsFromMemory";
            this.btnShowSettingsFromMemory.Size = new System.Drawing.Size(170, 23);
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
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1126, 638);
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
            this.mniRefreshTaskBars,
            this.mniSnapAll,
            this.mniToggleTabBarAlwaysOnTop,
            this.toolStripSeparator4,
            this.mniSaveDesktop,
            this.mniLoadDesktop,
            this.toolStripSeparator2,
            this.mniToggleLayouts,
            this.toolStripSeparator1,
            this.mniExit});
            this.ctxmNotifyIcon.Name = "ctxmNotifyIcon";
            this.ctxmNotifyIcon.Size = new System.Drawing.Size(195, 198);
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
            this.tabLayouts.Controls.Add(this.tabPage3);
            this.tabLayouts.Controls.Add(this.tabPage2);
            this.tabLayouts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLayouts.Location = new System.Drawing.Point(0, 0);
            this.tabLayouts.Name = "tabLayouts";
            this.tabLayouts.SelectedIndex = 0;
            this.tabLayouts.Size = new System.Drawing.Size(1140, 670);
            this.tabLayouts.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1132, 644);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "App Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel6);
            this.tabPage3.Controls.Add(this.panel5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1132, 644);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Layout Editor";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel10);
            this.panel6.Controls.Add(this.lbMonitors);
            this.panel6.Controls.Add(this.lbVD);
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(25, 25, 25, 0);
            this.panel6.Size = new System.Drawing.Size(1132, 598);
            this.panel6.TabIndex = 1;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.lbLayouts);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(409, 48);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(698, 550);
            this.panel10.TabIndex = 4;
            // 
            // lbLayouts
            // 
            this.lbLayouts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLayouts.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLayouts.FormattingEnabled = true;
            this.lbLayouts.HorizontalScrollbar = true;
            this.lbLayouts.ItemHeight = 18;
            this.lbLayouts.Location = new System.Drawing.Point(0, 0);
            this.lbLayouts.Name = "lbLayouts";
            this.lbLayouts.ScrollAlwaysVisible = true;
            this.lbLayouts.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbLayouts.Size = new System.Drawing.Size(698, 550);
            this.lbLayouts.TabIndex = 2;
            // 
            // lbMonitors
            // 
            this.lbMonitors.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbMonitors.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMonitors.FormattingEnabled = true;
            this.lbMonitors.HorizontalScrollbar = true;
            this.lbMonitors.ItemHeight = 18;
            this.lbMonitors.Location = new System.Drawing.Point(221, 48);
            this.lbMonitors.Name = "lbMonitors";
            this.lbMonitors.ScrollAlwaysVisible = true;
            this.lbMonitors.Size = new System.Drawing.Size(188, 550);
            this.lbMonitors.TabIndex = 1;
            this.lbMonitors.SelectedIndexChanged += new System.EventHandler(this.lbMonitors_SelectedIndexChanged);
            // 
            // lbVD
            // 
            this.lbVD.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbVD.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVD.FormattingEnabled = true;
            this.lbVD.HorizontalScrollbar = true;
            this.lbVD.ItemHeight = 18;
            this.lbVD.Location = new System.Drawing.Point(25, 48);
            this.lbVD.Name = "lbVD";
            this.lbVD.ScrollAlwaysVisible = true;
            this.lbVD.Size = new System.Drawing.Size(196, 550);
            this.lbVD.TabIndex = 0;
            this.lbVD.SelectedIndexChanged += new System.EventHandler(this.lbVD_SelectedIndexChanged);
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.label3);
            this.panel9.Controls.Add(this.label2);
            this.panel9.Controls.Add(this.label1);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(25, 25);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1082, 23);
            this.panel9.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(381, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Layouts";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Virtual Desktops";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Monitors";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnClearViewLayouts);
            this.panel5.Controls.Add(this.btnShowHideSelectedLayout);
            this.panel5.Controls.Add(this.btnShowHideLayoutList);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 598);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1132, 46);
            this.panel5.TabIndex = 0;
            // 
            // btnClearViewLayouts
            // 
            this.btnClearViewLayouts.Location = new System.Drawing.Point(271, 15);
            this.btnClearViewLayouts.Name = "btnClearViewLayouts";
            this.btnClearViewLayouts.Size = new System.Drawing.Size(96, 23);
            this.btnClearViewLayouts.TabIndex = 2;
            this.btnClearViewLayouts.Text = "Clear View";
            this.btnClearViewLayouts.UseVisualStyleBackColor = true;
            this.btnClearViewLayouts.Click += new System.EventHandler(this.btnClearViewLayouts_Click);
            // 
            // btnShowHideSelectedLayout
            // 
            this.btnShowHideSelectedLayout.Location = new System.Drawing.Point(128, 15);
            this.btnShowHideSelectedLayout.Name = "btnShowHideSelectedLayout";
            this.btnShowHideSelectedLayout.Size = new System.Drawing.Size(137, 23);
            this.btnShowHideSelectedLayout.TabIndex = 1;
            this.btnShowHideSelectedLayout.Text = "View Selected Layout(s)";
            this.btnShowHideSelectedLayout.UseVisualStyleBackColor = true;
            this.btnShowHideSelectedLayout.Click += new System.EventHandler(this.btnShowHideSelectedLayout_Click);
            // 
            // btnShowHideLayoutList
            // 
            this.btnShowHideLayoutList.Location = new System.Drawing.Point(25, 15);
            this.btnShowHideLayoutList.Name = "btnShowHideLayoutList";
            this.btnShowHideLayoutList.Size = new System.Drawing.Size(96, 23);
            this.btnShowHideLayoutList.TabIndex = 0;
            this.btnShowHideLayoutList.Text = "View Layout(s)";
            this.btnShowHideLayoutList.UseVisualStyleBackColor = true;
            this.btnShowHideLayoutList.Click += new System.EventHandler(this.btnShowHideLayoutList_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1132, 644);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Foreground Groups";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(25);
            this.panel4.Size = new System.Drawing.Size(1126, 638);
            this.panel4.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.lbForegroundGroupWindows);
            this.panel8.Controls.Add(this.pnlFGs);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(25, 25);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1076, 539);
            this.panel8.TabIndex = 3;
            // 
            // lbForegroundGroupWindows
            // 
            this.lbForegroundGroupWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbForegroundGroupWindows.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbForegroundGroupWindows.FormattingEnabled = true;
            this.lbForegroundGroupWindows.ItemHeight = 18;
            this.lbForegroundGroupWindows.Location = new System.Drawing.Point(215, 0);
            this.lbForegroundGroupWindows.Name = "lbForegroundGroupWindows";
            this.lbForegroundGroupWindows.ScrollAlwaysVisible = true;
            this.lbForegroundGroupWindows.Size = new System.Drawing.Size(861, 539);
            this.lbForegroundGroupWindows.TabIndex = 0;
            // 
            // pnlFGs
            // 
            this.pnlFGs.Controls.Add(this.lbForegroundGroups);
            this.pnlFGs.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFGs.Location = new System.Drawing.Point(0, 0);
            this.pnlFGs.Name = "pnlFGs";
            this.pnlFGs.Padding = new System.Windows.Forms.Padding(0, 0, 25, 0);
            this.pnlFGs.Size = new System.Drawing.Size(215, 539);
            this.pnlFGs.TabIndex = 0;
            // 
            // lbForegroundGroups
            // 
            this.lbForegroundGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbForegroundGroups.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbForegroundGroups.FormattingEnabled = true;
            this.lbForegroundGroups.ItemHeight = 18;
            this.lbForegroundGroups.Location = new System.Drawing.Point(0, 0);
            this.lbForegroundGroups.Name = "lbForegroundGroups";
            this.lbForegroundGroups.ScrollAlwaysVisible = true;
            this.lbForegroundGroups.Size = new System.Drawing.Size(190, 539);
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
            this.panel7.Location = new System.Drawing.Point(25, 564);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1076, 49);
            this.panel7.TabIndex = 2;
            // 
            // btnSaveFgGroups
            // 
            this.btnSaveFgGroups.Location = new System.Drawing.Point(3, 23);
            this.btnSaveFgGroups.Name = "btnSaveFgGroups";
            this.btnSaveFgGroups.Size = new System.Drawing.Size(128, 23);
            this.btnSaveFgGroups.TabIndex = 7;
            this.btnSaveFgGroups.Text = "Save Groups";
            this.btnSaveFgGroups.UseVisualStyleBackColor = true;
            this.btnSaveFgGroups.Click += new System.EventHandler(this.btnSaveFgGroups_Click);
            // 
            // btnRefreshWindowsInFG
            // 
            this.btnRefreshWindowsInFG.Location = new System.Drawing.Point(963, 23);
            this.btnRefreshWindowsInFG.Name = "btnRefreshWindowsInFG";
            this.btnRefreshWindowsInFG.Size = new System.Drawing.Size(103, 23);
            this.btnRefreshWindowsInFG.TabIndex = 6;
            this.btnRefreshWindowsInFG.Text = "Refresh Windows";
            this.btnRefreshWindowsInFG.UseVisualStyleBackColor = true;
            this.btnRefreshWindowsInFG.Click += new System.EventHandler(this.btnRefreshWindowsInFG_Click);
            // 
            // btnRemoveWindowInFG
            // 
            this.btnRemoveWindowInFG.Location = new System.Drawing.Point(854, 23);
            this.btnRemoveWindowInFG.Name = "btnRemoveWindowInFG";
            this.btnRemoveWindowInFG.Size = new System.Drawing.Size(103, 23);
            this.btnRemoveWindowInFG.TabIndex = 5;
            this.btnRemoveWindowInFG.Text = "Remove Window";
            this.btnRemoveWindowInFG.UseVisualStyleBackColor = true;
            this.btnRemoveWindowInFG.Click += new System.EventHandler(this.btnRemoveWindowInFG_Click);
            // 
            // btnToggleDetectWindow
            // 
            this.btnToggleDetectWindow.Location = new System.Drawing.Point(215, 23);
            this.btnToggleDetectWindow.Name = "btnToggleDetectWindow";
            this.btnToggleDetectWindow.Size = new System.Drawing.Size(123, 23);
            this.btnToggleDetectWindow.TabIndex = 4;
            this.btnToggleDetectWindow.Text = "Detect Window";
            this.btnToggleDetectWindow.UseVisualStyleBackColor = true;
            this.btnToggleDetectWindow.Click += new System.EventHandler(this.btnToggleDetectWindow_Click);
            // 
            // btnAddWindow
            // 
            this.btnAddWindow.Enabled = false;
            this.btnAddWindow.Location = new System.Drawing.Point(747, 24);
            this.btnAddWindow.Name = "btnAddWindow";
            this.btnAddWindow.Size = new System.Drawing.Size(101, 23);
            this.btnAddWindow.TabIndex = 3;
            this.btnAddWindow.Text = "Add Window";
            this.btnAddWindow.UseVisualStyleBackColor = true;
            this.btnAddWindow.Click += new System.EventHandler(this.btnAddWindow_Click);
            // 
            // txtSelectedWindow
            // 
            this.txtSelectedWindow.Location = new System.Drawing.Point(344, 26);
            this.txtSelectedWindow.Name = "txtSelectedWindow";
            this.txtSelectedWindow.ReadOnly = true;
            this.txtSelectedWindow.Size = new System.Drawing.Size(397, 20);
            this.txtSelectedWindow.TabIndex = 2;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1140, 670);
            this.Controls.Add(this.tabLayouts);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.tabPage3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel5.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ListBox lbMonitors;
        private System.Windows.Forms.ListBox lbVD;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.ListBox lbLayouts;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnShowHideSelectedLayout;
        private System.Windows.Forms.Button btnShowHideLayoutList;
        private System.Windows.Forms.Button btnClearViewLayouts;
        private System.Windows.Forms.ToolStripMenuItem mniRefreshTaskBars;
        private System.Windows.Forms.ToolStripMenuItem mniToggleTabBarAlwaysOnTop;
        private System.Windows.Forms.ToolStripMenuItem mniSaveDesktop;
        private System.Windows.Forms.ToolStripMenuItem mniLoadDesktop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniSnapAll;
    }
}

