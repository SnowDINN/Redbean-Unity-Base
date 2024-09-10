using System.Linq;
using Redbean.MVP.Content;
using Redbean.Popup;
using Redbean.Table;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 팝업 호출
		/// </summary>
		public static PopupManager Popup(this IExtension extension) => ExtensionGetSingleton<PopupManager>();
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static UserModel User(this IExtension extension) => ExtensionGetModel<UserModel>();
		
		/// <summary>
		/// 클래스 변환
		/// </summary>
		public static T As<T>(this IExtension extension) where T : class => extension as T;

		/// <summary>
		/// 언어 현지화 번역
		/// </summary>
		public static string GetLocalization(this IExtension extension, string key, params object[] args)
		{
			if (!TableContainer.Localization.TryGetValue(key, out var value))
				return key;

			var field = value.GetType().GetFields()
				.FirstOrDefault(_ => _.Name == $"{GameConfigureSetting.LanguageType}")
				.GetValue(value);
			
			return string.Format($"{field}", args);

		}
	}
}