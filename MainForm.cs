﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace MonitorWatch
{
	public partial class MainForm : Form
	{
		const int WM_DESTROY = 0x0002;
		const int WM_SIZE = 0x0005;
		const int WM_DISPLAYCHANGE = 0x007e;

		const int GWL_STYLE = (-16);

		const uint WS_EX_WINDOW = 0x10080000;

		const uint SWP_NOACTIVATE = 0x0010;
		const uint SWP_NOZORDER = 0x0004;

		const uint WPF_RESTORETOMAXIMIZED = 0x0002;
		const uint WPF_ASYNCWINDOWPLACEMENT = 0x0004;

		const int WH_CALLWNDPROC = 0x0004;

		public enum ShowWindowCommands : int
		{
			Hide = 0,
			ShowNormal = 1,
			ShowMinimized = 2,
			ShowMaximized = 3,
			ShowNoActivate = 4,
			Maximize = 5,
			Minimize = 6,
			ShowMiniNoActivate = 7,
			ShowNA = 8,
			Restore = 9
		}

		public struct WINDOWPLACEMENT
		{
			public int length;
			public uint flags;
			public ShowWindowCommands showCmd;
			public Point ptMinPosition;
			public Point ptMaxPosition;
			public Rectangle rcNormalPosition;
		}

		private struct Rect
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		[DllImport("user32")]
		private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lpRect, MonitorEnumProc callback, int dwData);
		private delegate bool MonitorEnumProc(IntPtr hDesktop, IntPtr hdc, ref Rect pRect, int dwData);

		[DllImport("user32.dll")]
		private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
		private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
		private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

		[DllImport("user32.dll")]
		static extern bool UpdateWindow(IntPtr hWnd);


		private Dictionary<IntPtr, WindowTracker> _windowList = new Dictionary<IntPtr, WindowTracker>();
		private Timer _timer = new Timer();
		private int[] __intervals = new int[] { 250, 500, 1000, 2000, 5000, 10000, 30000, 60000 };
		private string _display = null;

		public MainForm()
		{
			InitializeComponent();

			cmbInterval.SelectedIndex = 2;
			WindowState = FormWindowState.Minimized;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			UpdateDisplay();

			_timer.Tick += new EventHandler(OnTimer);
			_timer.Interval = __intervals[cmbInterval.SelectedIndex];
			_timer.Enabled = true;

			OnTimer(null, null);
		}

		private void cmbInterval_SelectedIndexChanged(object sender, EventArgs e)
		{
			_timer.Interval = __intervals[cmbInterval.SelectedIndex];
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Close MonitorWatch?", "MonitorWatch", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				Application.Exit();
			}
		}

		private void btnHide_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
			Hide();
		}

		private void notifyIcon_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Normal;
			Show();
		}

		private void OnTimer(object sender, EventArgs e)
		{
			List<IntPtr> windowList = FindWindows();

			foreach (IntPtr hWnd in windowList)
			{
				if (_windowList.ContainsKey(hWnd))
				{
					_windowList[hWnd].CheckPlacement(_display);
				}
				else
				{
					_windowList.Add(
						hWnd,
						new WindowTracker(
							_display,
							hWnd
						)
					);
				}
			}

			if (_windowList.Count > windowList.Count)
			{
				List<IntPtr> dropList = new List<IntPtr>();

				foreach (KeyValuePair<IntPtr, WindowTracker> window in _windowList)
				{
					if (!windowList.Contains(window.Key))
					{
						dropList.Add(window.Key);
					}
				}

				foreach (IntPtr hWnd in dropList)
				{
					_windowList.Remove(hWnd);
				}
			}

			lblWindowCount.Text = _windowList.Count.ToString();
		}

		public static List<IntPtr> FindWindows()
		{
			List<IntPtr> windowList = new List<IntPtr>();

			EnumWindows(
				delegate (IntPtr hWnd, IntPtr lParam)
				{
					if ((GetWindowLong(hWnd, GWL_STYLE) & WS_EX_WINDOW) == WS_EX_WINDOW)
					{
						windowList.Add(hWnd);
					}
					return true;
				},
				IntPtr.Zero
			);

			return windowList;
		}

		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		protected override void WndProc(ref Message message)
		{
			switch (message.Msg)
			{
				case WM_DISPLAYCHANGE:
				{
					UpdateDisplay();
					break;
				}
			}
			base.WndProc(ref message);
		}

		private void UpdateDisplay()
		{
			StringBuilder display = new StringBuilder();
			int counter = 0;

			EnumDisplayMonitors(
				IntPtr.Zero,
				IntPtr.Zero,
				delegate (IntPtr hDesktop, IntPtr hdc, ref Rect pRect, int dwData)
				{
					display.Append(pRect.left);
					display.Append('x');
					display.Append(pRect.top);
					display.Append('-');
					display.Append(pRect.right);
					display.Append('x');
					display.Append(pRect.bottom);
					display.Append(';');

					++counter;

					return true;
				},
				0
			);

			lblDisplayCount.Text = counter.ToString();

			_display = display.ToString();
		}

		private class WindowTracker
		{
			private IntPtr _hWnd = IntPtr.Zero;
			private string _display = null;
			private Dictionary<string, WINDOWPLACEMENT> _placement = new Dictionary<string, WINDOWPLACEMENT>();

			public WindowTracker(string display, IntPtr hWnd)
			{
				_hWnd = hWnd;

				CheckPlacement(display);
			}

			public void CheckPlacement()
			{
				CheckPlacement(_display);
			}

			public void CheckPlacement(string display)
			{
				WINDOWPLACEMENT windowPlacement;

				if (_display == display)
				{
					windowPlacement = new WINDOWPLACEMENT();

					GetWindowPlacement(_hWnd, ref windowPlacement);

					_placement[display] = windowPlacement;
					return;
				}

				if (_display == null || !_placement.ContainsKey(display))
				{
					windowPlacement = new WINDOWPLACEMENT();

					GetWindowPlacement(_hWnd, ref windowPlacement);

					if (_display != null && windowPlacement.showCmd == ShowWindowCommands.ShowMaximized)
					{
						windowPlacement.flags = WPF_ASYNCWINDOWPLACEMENT;
						windowPlacement.showCmd = ShowWindowCommands.Restore;
						SetWindowPlacement(_hWnd, ref windowPlacement);
						UpdateWindow(_hWnd);
						windowPlacement.showCmd = ShowWindowCommands.ShowMaximized;
						SetWindowPlacement(_hWnd, ref windowPlacement);
						GetWindowPlacement(_hWnd, ref windowPlacement);
					}

					_placement.Add(display, windowPlacement);
					_display = display;
					return;
				}

				windowPlacement = _placement[display];

				if (windowPlacement.showCmd == ShowWindowCommands.ShowMaximized)
				{
					windowPlacement.flags = WPF_ASYNCWINDOWPLACEMENT;
					windowPlacement.showCmd = ShowWindowCommands.Restore;
					SetWindowPlacement(_hWnd, ref windowPlacement);
					UpdateWindow(_hWnd);
					windowPlacement.showCmd = ShowWindowCommands.ShowMaximized;
					SetWindowPlacement(_hWnd, ref windowPlacement);
				}
				else
				{
					SetWindowPlacement(_hWnd, ref windowPlacement);
				}

				_display = display;
			}
		}
	}
}