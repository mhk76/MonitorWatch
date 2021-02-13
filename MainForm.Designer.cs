using System.Reflection;

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
			this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.CtxPopup = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MnuRestore = new System.Windows.Forms.ToolStripMenuItem();
			this.MnuSave = new System.Windows.Forms.ToolStripMenuItem();
			this.MnuCurrent = new System.Windows.Forms.ToolStripMenuItem();
			this.menuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.MnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.menuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.MnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.lblVersion = new System.Windows.Forms.Label();
			this.CtxPopup.SuspendLayout();
			this.SuspendLayout();
			// 
			// NotifyIcon
			// 
			this.NotifyIcon.ContextMenuStrip = this.CtxPopup;
			this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
			this.NotifyIcon.Text = "MonitorWatch";
			this.NotifyIcon.Visible = true;
			// 
			// CtxPopup
			// 
			this.CtxPopup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuCurrent,
            this.MnuRestore,
            this.MnuSave,
            this.menuSeparator1,
            this.MnuAbout,
            this.menuSeparator2,
            this.MnuExit});
			this.CtxPopup.Name = "ctxPopup";
			this.CtxPopup.Size = new System.Drawing.Size(210, 148);
			// 
			// MnuRestore
			// 
			this.MnuRestore.Name = "MnuRestore";
			this.MnuRestore.Size = new System.Drawing.Size(209, 22);
			this.MnuRestore.Text = "Restore window positions";
			this.MnuRestore.Click += new System.EventHandler(this.MnuRestore_Click);
			// 
			// MnuSave
			// 
			this.MnuSave.Name = "MnuSave";
			this.MnuSave.Size = new System.Drawing.Size(209, 22);
			this.MnuSave.Text = "Save window positions";
			this.MnuSave.Click += new System.EventHandler(this.MnuSave_Click);
			// 
			// MnuCurrent
			// 
			this.MnuCurrent.Enabled = false;
			this.MnuCurrent.Name = "MnuCurrent";
			this.MnuCurrent.Size = new System.Drawing.Size(209, 22);
			this.MnuCurrent.Text = "CurrentDisplayset";
			// 
			// menuSeparator1
			// 
			this.menuSeparator1.Name = "menuSeparator1";
			this.menuSeparator1.Size = new System.Drawing.Size(206, 6);
			// 
			// MnuAbout
			// 
			this.MnuAbout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
			this.MnuAbout.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.MnuAbout.Name = "MnuAbout";
			this.MnuAbout.Size = new System.Drawing.Size(209, 22);
			this.MnuAbout.Text = "MonitorWatch vX.X.X";
			this.MnuAbout.Click += new System.EventHandler(this.MnuAbout_Click);
			// 
			// menuSeparator2
			// 
			this.menuSeparator2.Name = "menuSeparator2";
			this.menuSeparator2.Size = new System.Drawing.Size(206, 6);
			// 
			// MnuExit
			// 
			this.MnuExit.Name = "MnuExit";
			this.MnuExit.Size = new System.Drawing.Size(209, 22);
			this.MnuExit.Text = "Exit";
			this.MnuExit.Click += new System.EventHandler(this.MnuExit_Click);
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.lblVersion.Location = new System.Drawing.Point(13, 13);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(0, 13);
			this.lblVersion.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.ClientSize = new System.Drawing.Size(1056, 250);
			this.Controls.Add(this.lblVersion);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Location = new System.Drawing.Point(51, 101);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MonitorWatch";
			this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.CtxPopup.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.NotifyIcon NotifyIcon;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.ContextMenuStrip CtxPopup;
		private System.Windows.Forms.ToolStripMenuItem MnuRestore;
		private System.Windows.Forms.ToolStripMenuItem MnuSave;
		private System.Windows.Forms.ToolStripSeparator menuSeparator1;
		private System.Windows.Forms.ToolStripMenuItem MnuAbout;
		private System.Windows.Forms.ToolStripSeparator menuSeparator2;
		private System.Windows.Forms.ToolStripMenuItem MnuExit;
		private System.Windows.Forms.ToolStripMenuItem MnuCurrent;
	}
}

