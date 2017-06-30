namespace MonitorWatch
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.lblInterval = new System.Windows.Forms.Label();
			this.cmbInterval = new System.Windows.Forms.ComboBox();
			this.btnHide = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.lblWindowCount = new System.Windows.Forms.Label();
			this.lblTracking = new System.Windows.Forms.Label();
			this.lblWindows = new System.Windows.Forms.Label();
			this.lblDisplayCount = new System.Windows.Forms.Label();
			this.lblDisplays = new System.Windows.Forms.Label();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.SuspendLayout();
			// 
			// lblInterval
			// 
			this.lblInterval.AutoSize = true;
			this.lblInterval.Location = new System.Drawing.Point(12, 9);
			this.lblInterval.Name = "lblInterval";
			this.lblInterval.Size = new System.Drawing.Size(72, 13);
			this.lblInterval.TabIndex = 0;
			this.lblInterval.Text = "Track interval";
			// 
			// cmbInterval
			// 
			this.cmbInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbInterval.FormattingEnabled = true;
			this.cmbInterval.Items.AddRange(new object[] {
            "every 250 milliseconds",
            "every 500 milliseconds",
            "every 1 second",
            "every 2 seconds",
            "every 5 seconds",
            "every 10 seconds",
            "every 30 seconds",
            "every 1 minute"});
			this.cmbInterval.Location = new System.Drawing.Point(12, 25);
			this.cmbInterval.Name = "cmbInterval";
			this.cmbInterval.Size = new System.Drawing.Size(264, 21);
			this.cmbInterval.TabIndex = 1;
			this.cmbInterval.SelectedIndexChanged += new System.EventHandler(this.cmbInterval_SelectedIndexChanged);
			// 
			// btnHide
			// 
			this.btnHide.Location = new System.Drawing.Point(201, 91);
			this.btnHide.Name = "btnHide";
			this.btnHide.Size = new System.Drawing.Size(75, 23);
			this.btnHide.TabIndex = 2;
			this.btnHide.Text = "Hide";
			this.btnHide.UseVisualStyleBackColor = true;
			this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(120, 91);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// lblWindowCount
			// 
			this.lblWindowCount.Location = new System.Drawing.Point(67, 49);
			this.lblWindowCount.Name = "lblWindowCount";
			this.lblWindowCount.Size = new System.Drawing.Size(25, 13);
			this.lblWindowCount.TabIndex = 4;
			this.lblWindowCount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblTracking
			// 
			this.lblTracking.AutoSize = true;
			this.lblTracking.Location = new System.Drawing.Point(12, 49);
			this.lblTracking.Name = "lblTracking";
			this.lblTracking.Size = new System.Drawing.Size(49, 13);
			this.lblTracking.TabIndex = 5;
			this.lblTracking.Text = "Tracking";
			// 
			// lblWindows
			// 
			this.lblWindows.AutoSize = true;
			this.lblWindows.Location = new System.Drawing.Point(98, 49);
			this.lblWindows.Name = "lblWindows";
			this.lblWindows.Size = new System.Drawing.Size(63, 13);
			this.lblWindows.TabIndex = 6;
			this.lblWindows.Text = "windows on";
			// 
			// lblDisplayCount
			// 
			this.lblDisplayCount.Location = new System.Drawing.Point(160, 49);
			this.lblDisplayCount.Name = "lblDisplayCount";
			this.lblDisplayCount.Size = new System.Drawing.Size(19, 13);
			this.lblDisplayCount.TabIndex = 7;
			this.lblDisplayCount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblDisplays
			// 
			this.lblDisplays.AutoSize = true;
			this.lblDisplays.Location = new System.Drawing.Point(185, 49);
			this.lblDisplays.Name = "lblDisplays";
			this.lblDisplays.Size = new System.Drawing.Size(44, 13);
			this.lblDisplays.TabIndex = 8;
			this.lblDisplays.Text = "displays";
			// 
			// notifyIcon
			// 
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "MonitorWatch";
			this.notifyIcon.Visible = true;
			this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
			this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(288, 126);
			this.ControlBox = false;
			this.Controls.Add(this.lblDisplays);
			this.Controls.Add(this.lblDisplayCount);
			this.Controls.Add(this.lblWindows);
			this.Controls.Add(this.lblTracking);
			this.Controls.Add(this.lblWindowCount);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnHide);
			this.Controls.Add(this.cmbInterval);
			this.Controls.Add(this.lblInterval);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(51, 101);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Text = "MonitorWatch";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblInterval;
		private System.Windows.Forms.ComboBox cmbInterval;
		private System.Windows.Forms.Button btnHide;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblWindowCount;
		private System.Windows.Forms.Label lblTracking;
		private System.Windows.Forms.Label lblWindows;
		private System.Windows.Forms.Label lblDisplayCount;
		private System.Windows.Forms.Label lblDisplays;
		private System.Windows.Forms.NotifyIcon notifyIcon;
	}
}

