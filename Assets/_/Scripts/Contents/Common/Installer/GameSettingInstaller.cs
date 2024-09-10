using Redbean.Base;
using Redbean.Rx;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Redbean
{
	[CreateAssetMenu(fileName = "GameConfigure", menuName = "Redbean/Game/GameConfigure")]
	public class GameConfigureInstaller : ScriptableObject
	{
		private LanguageType m_LanguageType;

		[ShowInInspector]
		public LanguageType LanguageType
		{
			get => m_LanguageType;
			set
			{
				m_LanguageType = value;
				RxLocalizationBinder.Publish(value);
			}
		}
	}
	
	public class GameConfigureSetting : SettingsBase<GameConfigureInstaller>
	{
		public static LanguageType LanguageType
		{
			get => Installer.LanguageType;
			set => Installer.LanguageType = value;
		}
	}
}