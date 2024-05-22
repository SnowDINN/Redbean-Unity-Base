using Google;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ThirdPartyWindow : OdinEditorWindow
	{
		private const string Import = "How to import";

		private Installer GoogleInstaller;

		[TabGroup(Google), Title(Import), InlineButton(nameof(GetAosClientKey), "Get"), ShowInInspector]
		public string AndroidClientKey
		{
			get => GoogleInstaller.androidClientId;
			set => GoogleInstaller.androidClientId = value;
		}

		[TabGroup(Google), InlineButton(nameof(GetIosClientKey), "Get"), ShowInInspector]
		public string IosClientKey
		{
			get => GoogleInstaller.iosClientId;
			set => GoogleInstaller.iosClientId = value;
		}

		[TabGroup(Google), InlineButton(nameof(GetWebClientKey), "Get"), ShowInInspector]
		public string WebClientKey
		{
			get => GoogleInstaller.webClientId;
			set => GoogleInstaller.webClientId = value;
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