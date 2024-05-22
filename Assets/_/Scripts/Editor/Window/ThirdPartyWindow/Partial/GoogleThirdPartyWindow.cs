using Google;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ThirdPartyWindow
	{
		private const string ClientKey = "Client key settings";
		private const string Editor = "Use only in the editor";

		private Installer GoogleInstaller;

		[TabGroup(Google), Title(ClientKey), InlineButton(nameof(GetAosClientKey), "GET"), ShowInInspector]
		public string AndroidClientKey
		{
			get => GoogleInstaller.androidClientId;
			set
			{
				GoogleInstaller.androidClientId = value;
				GoogleInstaller.Save();
			}
		}

		[TabGroup(Google), InlineButton(nameof(GetIosClientKey), "GET"), ShowInInspector]
		public string IosClientKey
		{
			get => GoogleInstaller.iosClientId;
			set
			{
				GoogleInstaller.iosClientId = value;
				GoogleInstaller.Save();
			}
		}

		[TabGroup(Google), InlineButton(nameof(GetWebClientKey), "GET"), ShowInInspector]
		public string WebClientKey
		{
			get => GoogleInstaller.webClientId;
			set
			{
				GoogleInstaller.webClientId = value;
				GoogleInstaller.Save();
			}
		}

		[TabGroup(Google), Title(Editor), ShowInInspector]
		public string WebSecretKey
		{
			get => GoogleInstaller.webSecretId;
			set
			{
				GoogleInstaller.webSecretId = value;
				GoogleInstaller.Save();
			}
		}
		
		[TabGroup(Google), ShowInInspector]
		public int Port
		{
			get => GoogleInstaller.webRedirectPort;
			set
			{
				GoogleInstaller.webRedirectPort = value;
				GoogleInstaller.Save();
			}
		}

		public void GetAosClientKey() => AndroidClientKey = GoogleInstaller.GetClientId(ClientType.AndroidClientId);

		public void GetIosClientKey() => IosClientKey = GoogleInstaller.GetClientId(ClientType.IosClientId);

		public void GetWebClientKey() => WebClientKey = GoogleInstaller.GetClientId(ClientType.WebClientId);
		
		protected override void OnEnable()
		{
			GoogleInstaller = Resources.Load<Installer>("Google/Installer");
		}
	}
}