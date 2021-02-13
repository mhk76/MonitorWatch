using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace MonitorWatch
{
	public partial class MainForm : Form
	{
		const int WM_DISPLAYCHANGE = 0x007e;

		const uint MONITORINFO_PRIMARY = 0x0001;

		private struct MONITORINFOEX
		{
			public int length;
			public RECT rcMonitor;
			public RECT rcWorkArea;
			public uint flags;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lpDeviceName;
		}

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

		[DllImport("user32", SetLastError = true)]
		private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lpRect, MonitorEnumProc callback, int dwData);
		private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdc, ref RECT pRect, int dwData);

		private Dictionary<string, WindowTracker> _displaySets = new Dictionary<string, WindowTracker>();
		private string _displaySet = null;

		public MainForm()
		{
			InitializeComponent();

			var version = typeof(MainForm).Assembly.GetName().Version;

			MnuAbout.Text = "MonitorWatch v" + version.Major + "." + version.Minor + "." + version.Build;

			WindowState = FormWindowState.Minimized;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			if (File.Exists(".\\MonitorWatch.json"))
			{
				_displaySets = JsonSerializer.Deserialize<SaveFormat>(
						File.ReadAllText(".\\MonitorWatch.json", Encoding.UTF8)
					)
					.DisplaySets
						.Select(keyValue =>
							new KeyValuePair<string, WindowTracker>(
								keyValue.Key,
								new WindowTracker(keyValue.Value)
							)
						)
						.ToDictionary(
							keyValue => keyValue.Key,
							keyValue => keyValue.Value
						);
			}

			UpdateDisplaySet();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs closingEvent)
		{
			if (closingEvent.CloseReason == CloseReason.UserClosing)
			{
				WindowState = FormWindowState.Minimized;
				closingEvent.Cancel = true;
			}
		}

		private void MainForm_Deactivate(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}

		private void MnuSave_Click(object sender, EventArgs e)
		{
			_displaySets[_displaySet].Save();

			File.WriteAllText(
				".\\MonitorWatch.json",
				JsonSerializer.Serialize(
					new SaveFormat()
					{
						Version = "v1.0",
						DisplaySets =
							_displaySets.ToDictionary(
								item => item.Key,
								item => item.Value.Export()
							)
					},
					new JsonSerializerOptions()
					{
						IncludeFields = true
					}
				),
				Encoding.UTF8
			);
		}

		private void MnuRestore_Click(object sender, EventArgs e)
		{
			_displaySets[_displaySet].Restore();
		}

		private void MnuAbout_Click(object sender, EventArgs e)
		{
			Process.Start("https://gitlab.com/mhk76/MonitorWatch");
		}

		private void MnuExit_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show(
					"\tClose MonitorWatch?\t",
					"MonitorWatch",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				)
				== DialogResult.Yes
			)
			{
				Application.Exit();
			}
		}

		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		protected override void WndProc(ref Message message)
		{
			switch (message.Msg)
			{
				case WM_DISPLAYCHANGE:
				{
					UpdateDisplaySet();
					break;
				}
			}
			base.WndProc(ref message);
		} // WndProc()

		private void UpdateDisplaySet()
		{
			MONITORINFOEX monitorInfo = new MONITORINFOEX();
			List<string> displaySetId = new List<string>();
			int counter = 0;

			monitorInfo.length = 104;

			EnumDisplayMonitors(
				IntPtr.Zero,
				IntPtr.Zero,
				delegate (IntPtr hMonitor, IntPtr hdc, ref RECT pRect, int dwData)
				{
					GetMonitorInfo(hMonitor, ref monitorInfo);

					StringBuilder id = new StringBuilder();

					if ((monitorInfo.flags & MONITORINFO_PRIMARY) == MONITORINFO_PRIMARY)
					{
						id.Append("(P)");
					}
					id.Append(++counter);
					id.Append(':');
					id.Append(pRect.right);
					id.Append('×');
					id.Append(pRect.bottom);
					if (pRect.left != 0 || pRect.top != 0)
					{
						id.Append('@');
						id.Append(pRect.left);
						id.Append('×');
						id.Append(pRect.top);
					}

					displaySetId.Add(id.ToString());

					return true;
				},
				0
			);

			_displaySet = string.Join(", ", displaySetId);

			if (_displaySets.ContainsKey(_displaySet))
			{
				_displaySets[_displaySet].Save();
			}
			else
			{
				_displaySets.Add(_displaySet, new WindowTracker());
			}

			MnuCurrent.Text = _displaySet;
		} // UpdateDisplaySet()

		private class SaveFormat
		{
			public string Version { get; set; } = "v1.0";
			public Dictionary<string, WindowPosition[]> DisplaySets { get; set; } = new Dictionary<string, WindowPosition[]>();
		} // class SaveFormat

	} // class MainForm
}