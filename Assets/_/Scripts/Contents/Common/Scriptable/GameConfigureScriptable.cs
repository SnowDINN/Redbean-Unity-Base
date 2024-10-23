using Redbean.Base;
using Redbean.Rx;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Redbean
{
	[CreateAssetMenu(fileName = "GameConfigureScriptable", menuName = "Redbean/Game/GameConfigureScriptable")]
	public class GameConfigureScriptable : ScriptableBase
	{
		private LanguageType languageType;

		[ShowInInspector]
		public LanguageType LanguageType
		{
			get => languageType;
			set
			{
				languageType = value;
				RxLocalizationBinder.Publish(value);
			}
		}
	}
}