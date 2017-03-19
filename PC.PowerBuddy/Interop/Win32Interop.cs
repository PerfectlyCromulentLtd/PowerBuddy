using PInvoke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace PC.PowerBuddy.Interop
{
	internal class Win32Interop
	{
		public static event EventHandler PowerSchemeChanged;

		public static void HideWindowFromAltTab(IntPtr windowHandle)
		{
			int exStyle = User32.GetWindowLong(windowHandle, User32.WindowLongIndexFlags.GWL_EXSTYLE);
			exStyle |= (int)User32.WindowStylesEx.WS_EX_TOOLWINDOW;

			User32.SetWindowLong(windowHandle, User32.WindowLongIndexFlags.GWL_EXSTYLE, (User32.SetWindowLongFlags)exStyle);
		}

		public static void RegisterForPowerSettingNotification(IntPtr windowHandle)
		{
			HwndSource source = HwndSource.FromHwnd(windowHandle);
			source.AddHook(new HwndSourceHook(WndProc));

			var guid = GUID_POWERSCHEME_PERSONALITY;
			RegisterPowerSettingNotification(windowHandle, ref guid, 0);
		}

		public static void UnregisterForPowerSettingNotification(IntPtr windowHandle)
		{
			HwndSource source = HwndSource.FromHwnd(windowHandle);
			source.AddHook(new HwndSourceHook(WndProc));
		}

		private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			// We're only interested in WM_POWERBROADCAST (0x0218) messages with wParam of PBT_POWERSETTINGCHANGE (0x8013)
			if (msg == 0x0218 && (int)wParam == 0x8013)
			{
				POWERBROADCAST_SETTING settingMessage =
					(POWERBROADCAST_SETTING)Marshal.PtrToStructure(
						lParam,
						typeof(POWERBROADCAST_SETTING));

				// We're only interested if the PowerSetting changed is GUID_POWERSCHEME_PERSONALITY
				if (settingMessage.PowerSetting == GUID_POWERSCHEME_PERSONALITY && settingMessage.DataLength == Marshal.SizeOf(typeof(Guid)))
				{
					try
					{
						Win32Interop.PowerSchemeChanged?.Invoke(null, EventArgs.Empty);
					}
					catch
					{
						// TODO: log?  Or is that too expensive here?
					}
				}
			}

			handled = false;
			return IntPtr.Zero;
		}

		[DllImport("User32", SetLastError = true, EntryPoint = "RegisterPowerSettingNotification", CallingConvention = CallingConvention.StdCall)]
		private static extern IntPtr RegisterPowerSettingNotification(IntPtr hRecipient, ref Guid PowerSettingGuid, Int32 Flags);

		[DllImport("User32", EntryPoint = "UnregisterPowerSettingNotification", CallingConvention = CallingConvention.StdCall)]
		private static extern bool UnregisterPowerSettingNotification(IntPtr handle);

		private static readonly Guid GUID_POWERSCHEME_PERSONALITY = new Guid("245d8541-3943-4422-b025-13A784F679B7");

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		internal struct POWERBROADCAST_SETTING
		{
			public Guid PowerSetting;
			public uint DataLength;
			public byte Data;
		}
	}
}
