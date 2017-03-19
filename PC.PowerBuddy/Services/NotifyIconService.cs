using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PC.PowerBuddy.Services
{
	public sealed class NotifyIconService : IDisposable
	{
		public event EventHandler Clicked;
		public event EventHandler CloseRequested;
		public event EventHandler IconEditorLaunchRequested;

		private readonly Icon defaultIcon;
		private readonly NotifyIcon notifyIcon;
		private IDictionary<Guid, Icon> candidateIcons;
		private Guid activePowerPlanId;

		public NotifyIconService()
		{
			this.activePowerPlanId = Guid.Empty;

			this.defaultIcon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().ManifestModule.Name);

			this.LoadIcons();

			this.notifyIcon = new NotifyIcon();
			this.BuildNotifyIcon();

			this.CloseRequested += (s, e) => this.notifyIcon.Visible = false;
		}

		private void LoadIcons()
		{
			this.candidateIcons =
				new DirectoryInfo(".")
					.GetFiles("*.ico")
					.Select(item => new
					{
						Key = ExtractPowerPlanIdFromFilename(item),
						Value = this.LoadIcon(item)
					})
					.Where(item => item.Value != null && item.Key != Guid.Empty)
					.ToDictionary(keySelector => keySelector.Key, elementSelector => elementSelector.Value);
		}

		private static Guid ExtractPowerPlanIdFromFilename(FileInfo item)
		{
			Guid powerPlanId;

			if (!Guid.TryParse(Path.GetFileNameWithoutExtension(item.Name), out powerPlanId))
			{
				powerPlanId = Guid.Empty;
			}

			return powerPlanId;
		}

		private Icon LoadIcon(FileInfo item)
		{
			Icon result = null;
			try
			{
				result = new Icon(item.FullName, this.defaultIcon.Size);
			}
			catch
			{
				// do nothing
			}
			return result;
		}

		private void BuildNotifyIcon()
		{
			this.notifyIcon.Icon = this.defaultIcon;
			this.notifyIcon.Visible = true;
			this.notifyIcon.MouseDown += (sender, args) =>
			{
				if (args.Button == MouseButtons.Left)
				{
					this.Clicked?.Invoke(this, EventArgs.Empty);
				}
			};
			this.notifyIcon.ContextMenu =
				new ContextMenu(
					new MenuItem[] {
						new MenuItem("&Icon Editor...", (sender, args) => this.IconEditorLaunchRequested?.Invoke(this, EventArgs.Empty)),
						new MenuItem("-"),
						new MenuItem("E&xit", (sender, args) => this.CloseRequested?.Invoke(this, EventArgs.Empty))
						});
		}

		internal void SetDisplayedIcon(Guid powerPlanId, string text)
		{
			Icon icon = null;
			this.candidateIcons.TryGetValue(powerPlanId, out icon);

			this.activePowerPlanId = powerPlanId;
			this.notifyIcon.Icon = icon ?? this.defaultIcon;
			this.notifyIcon.Text = text.Substring(0, Math.Min(64, text.Length));
		}

		internal void StoreNewPowerPlanIcon(Guid powerPlanId, byte[] iconData)
		{
			File.WriteAllBytes($".\\{powerPlanId}.ico", iconData);
			this.LoadIcons();
			this.SetDisplayedIcon(this.activePowerPlanId, this.notifyIcon.Text); //HACK
		}

		public void Dispose()
		{
			this.notifyIcon.Dispose();
		}
	}
}
