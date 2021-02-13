using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MonitorWatch
{
	public class WindowTracker
	{
		#region Windows API
		const int WM_GETTEXT = 0x000D;
		const int WM_GETTEXTLENGTH = 0x000e;

		const int GWL_STYLE = -16;

		const uint WS_VISIBLE = 0x10000000;
		const uint WS_SYSMENU = 0x00080000;
		const uint WS_CAPTION = 0x00C00000;
		const uint WS_EX_APPWINDOW = 0x00040000;
		const uint WS_EX_WINDOW = WS_VISIBLE | WS_CAPTION | WS_EX_APPWINDOW;
		const uint WS_UNIVERSAL_APP = 0x80000000;
		const uint WPF_ASYNCWINDOWPLACEMENT = 0x0004;

		const uint TB_BUTTONCOUNT = 0x0418;
		const uint TB_GETBUTTONINFOW = 0x043f;

		private enum ShowWindowCommands : int
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

		private struct WINDOWPLACEMENT
		{
			public int length;
			public uint flags;
			public ShowWindowCommands showCmd;
			public Point ptMinPosition;
			public Point ptMaxPosition;
			public Rectangle rcNormalPosition;
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wparam, int lparam);

		[DllImport("user32.dll")]
		private static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumWindowsProc lpfn, IntPtr lParam);
		private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool UpdateWindow(IntPtr hWnd);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out IntPtr processId);

		[DllImport("psapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, int nSize);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, IntPtr dwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CloseHandle(IntPtr hObject);
		#endregion

		private List<WindowInfo> _windows;

		public WindowTracker()
		{
			_windows = FindWindows();
		} // WindowTracker()

		public WindowTracker(WindowPosition[] windowPositions)
		{
			Indexer indexer = new Indexer();

			_windows = windowPositions
				.Select(window =>
					new WindowInfo()
					{
						Executable = window.Executable,
						Index = indexer.Get(window.Executable ?? ""),
						Placement = new WINDOWPLACEMENT()
						{
							showCmd = window.Maximized ? ShowWindowCommands.ShowMaximized : ShowWindowCommands.ShowNormal,
							rcNormalPosition = new Rectangle()
							{
								X = window.Left,
								Y = window.Top,
								Width = window.Top,
								Height = window.Height
							}
						}
					}
				)
				.ToList();
		} // WindowTracker(WindowPosition[])

		public WindowPosition[] Export()
		{
			return _windows
				.Select(window =>
					new WindowPosition()
					{
						Executable = window.Executable,
						Maximized = window.Placement.showCmd == ShowWindowCommands.ShowMaximized,
						Left = window.Placement.rcNormalPosition.X,
						Top = window.Placement.rcNormalPosition.Y,
						Width = window.Placement.rcNormalPosition.Width,
						Height = window.Placement.rcNormalPosition.Height
					}
				)
				.ToArray();
		} // Get()

		public void Save()
		{
			_windows = FindWindows();
		} // Update()

		public void Restore()
		{
			Indexer indexer = new Indexer();
			_windows = FindWindows()
				.Select(window => {
					window.Index = indexer.Get(window.Executable ?? "");

					WindowInfo oldWindow = _windows.FirstOrDefault(target =>
							target.Executable == window.Executable
							&& target.Index == window.Index
						);

					if (oldWindow != null)
					{
						window.Placement = oldWindow.Placement;
					}

					return window;
				})
				.ToList();

			_windows.ForEach(window =>
				SetPlacement(window.HWnd, window.Placement)
			);
		} // Set()

		private List<WindowInfo> FindWindows()
		{
			List<WindowInfo> windows = new List<WindowInfo>();

			EnumDesktopWindows(
				IntPtr.Zero,
				delegate (IntPtr hWnd, IntPtr lParam)
				{
					uint windowLong = GetWindowLong(hWnd, GWL_STYLE);

					if ((windowLong & WS_EX_WINDOW) == WS_EX_WINDOW && (windowLong & WS_UNIVERSAL_APP) != WS_UNIVERSAL_APP)
					{
						windows.Insert(
							0,
							new WindowInfo()
							{
								HWnd = hWnd,
								//Title = FetchWindowTitle(hWnd),
								Placement = FetchWindowPlacement(hWnd),
								Executable = FetchWindowExecutable(hWnd)
							}
						);
					}
					return true;
				},
				IntPtr.Zero
			);

			IntPtr hTray = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
		/*
			IntPtr hReBar = FindWindowEx(hTray, IntPtr.Zero, "ReBarWindow32", null);
			IntPtr hTask = FindWindowEx(hReBar, IntPtr.Zero, "MSTaskSwWClass", null);
			IntPtr hToolbar = FindWindowEx(hTask, IntPtr.Zero, "ToolbarWindow32", null);
		*/
			windows.Insert(
				0,
				new WindowInfo()
				{
					HWnd = hTray,
					//Title = "Tray",
					Placement = FetchWindowPlacement(hTray),
					Executable = ""
				}
			);

			return windows;
		} // FindWindows()

		private WINDOWPLACEMENT FetchWindowPlacement(IntPtr hWnd)
		{
			WINDOWPLACEMENT windowPlacement = new WINDOWPLACEMENT();

			GetWindowPlacement(hWnd, ref windowPlacement);

			return windowPlacement;
		} // FetchWindowPlacement()

		private string FetchWindowTitle(IntPtr hWnd)
		{
			StringBuilder title;

			int length = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0);

			if (length > 0)
			{
				title = new StringBuilder(length + 1);

				SendMessage(hWnd, WM_GETTEXT, title.Capacity, title);
			}
			else
			{
				title = new StringBuilder(hWnd.ToString());
			}

			return title.ToString();
		} // FetchWindowTitle()

		private string FetchWindowExecutable(IntPtr hWnd)
		{
			GetWindowThreadProcessId(hWnd, out IntPtr processId);

			StringBuilder filename = new StringBuilder(1024);
			IntPtr hProcess = OpenProcess(1040, 0, processId);

			GetModuleFileNameEx(hProcess, IntPtr.Zero, filename, filename.Capacity);

			CloseHandle(hProcess);

			return filename.ToString();
		} // FetchWindowExecutable()

		private void SetPlacement(IntPtr hWnd, WINDOWPLACEMENT windowPlacement)
		{
			if (windowPlacement.showCmd == ShowWindowCommands.ShowMaximized)
			{
				windowPlacement.flags = WPF_ASYNCWINDOWPLACEMENT;
				windowPlacement.showCmd = ShowWindowCommands.Restore;
				SetWindowPlacement(hWnd, ref windowPlacement);
				windowPlacement.showCmd = ShowWindowCommands.ShowMaximized;
				SetWindowPlacement(hWnd, ref windowPlacement);
			}
			else
			{
				SetWindowPlacement(hWnd, ref windowPlacement);
			}
		} // SetPlacement()

		private class WindowInfo
		{
			public IntPtr HWnd { get; set; }
			public string Executable { get; set; }
			//public string Title { get; set; }
			public int Index { get; set; }
			public WINDOWPLACEMENT Placement { get; set; }
		} // class WindowInfo

		private class Indexer
		{
			private readonly Dictionary<string, int> _indexes = new Dictionary<string, int>();

			public int Get(string item)
			{
				if (!_indexes.ContainsKey(item))
				{
					_indexes.Add(item, 0);
					return 0;
				}

				return ++_indexes[item];
			}
		} // class Indexer

	} // class WindowTracker
}
