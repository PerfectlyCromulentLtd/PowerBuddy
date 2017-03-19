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

		public NotifyIconService()
		{
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
					.Where(item =>
					{
						Guid powerPlanId;
						return Guid.TryParse(Path.GetFileNameWithoutExtension(item.Name), out powerPlanId);
					})
					.Select(item => new
					{
						Key = Guid.Parse(Path.GetFileNameWithoutExtension(item.Name)),
						Value = this.GetIconOrNull(item)
					})
					.Where(item => item.Value != null)
					.ToDictionary(keySelector => keySelector.Key, elementSelector => elementSelector.Value);
		}

		private Icon GetIconOrNull(FileInfo item)
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
			this.notifyIcon.Tag = Guid.Empty; //HACK
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

		internal void UpdateDisplayedIcon(Guid powerPlanId, string text)
		{
			Icon icon = null;
			this.candidateIcons.TryGetValue(powerPlanId, out icon);

			this.notifyIcon.Tag = powerPlanId; //HACK
			this.notifyIcon.Icon = icon ?? this.defaultIcon;
			this.notifyIcon.Text = text.Substring(0, Math.Min(64, text.Length));
		}

		internal void StoreNewPowerPlanIcon(Guid powerPlanId, byte[] iconData)
		{
			File.WriteAllBytes($".\\{powerPlanId}.ico", iconData);
			this.LoadIcons();
			this.UpdateDisplayedIcon((Guid)this.notifyIcon.Tag, this.notifyIcon.Text); //HACK
		}

		public void Dispose()
		{
			this.notifyIcon.Dispose();
		}
	}
}
