using Redbean.Base;
using Redbean.Rx;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Redbean
{
	[CreateAssetMenu(fileName = "GameConfigureScriptable", menuName = "Redbean/Game/GameConfigureScriptable")]
	public class GameConfigureScriptable : ScriptableObject
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
	
	public class GameConfigureReferencer : ScriptableBase<GameConfigureScriptable>
	{
		public static LanguageType LanguageType
		{
			get => Scriptable.LanguageType;
			set => Scriptable.LanguageType = value;
		}
	}
}