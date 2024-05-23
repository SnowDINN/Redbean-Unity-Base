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
		
		protected override void OnEnable()
		{
			GoogleInstaller = Resources.Load<Installer>("Google/Installer");
		}

		[TabGroup(GoogleTab), Title(ClientKey), InlineButton(nameof(GetAosClientKey), "GET"), ShowInInspector]
		private string AndroidClientKey
		{
			get => GoogleInstaller.androidClientId;
			set
			{
				GoogleInstaller.androidClientId = value;
				GoogleInstaller.Save();
			}
		}

		[TabGroup(GoogleTab), InlineButton(nameof(GetIosClientKey), "GET"), ShowInInspector]
		private string IosClientKey
		{
			get => GoogleInstaller.iosClientId;
			set
			{
				GoogleInstaller.iosClientId = value;
				GoogleInstaller.Save();
			}
		}

		[TabGroup(GoogleTab), InlineButton(nameof(GetWebClientKey), "GET"), ShowInInspector]
		private string WebClientKey
		{
			get => GoogleInstaller.webClientId;
			set
			{
				GoogleInstaller.webClientId = value;
				GoogleInstaller.Save();
			}
		}

		[TabGroup(GoogleTab), Title(Editor), ShowInInspector]
		private string WebSecretKey
		{
			get => GoogleInstaller.webSecretId;
			set
			{
				GoogleInstaller.webSecretId = value;
				GoogleInstaller.Save();
			}
		}
		
		[TabGroup(GoogleTab), ShowInInspector]
		private int Port
		{
			get => GoogleInstaller.webRedirectPort;
			set
			{
				GoogleInstaller.webRedirectPort = value;
				GoogleInstaller.Save();
			}
		}

		private void GetAosClientKey() => AndroidClientKey = GoogleInstaller.GetClientId(ClientType.AndroidClientId);

		private void GetIosClientKey() => IosClientKey = GoogleInstaller.GetClientId(ClientType.IosClientId);

		private void GetWebClientKey() => WebClientKey = GoogleInstaller.GetClientId(ClientType.WebClientId);
	}
}