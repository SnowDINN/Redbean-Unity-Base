using Redbean.MVP.Content;
using Redbean.Singleton;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 팝업 호출
		/// </summary>
		public static PopupContainer Popup(this IExtension extension) => ExtensionGetSingleton<PopupContainer>();
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static UserModel User(this IExtension extension) => ExtensionGetModel<UserModel>();
		
		/// <summary>
		/// 클래스 변환
		/// </summary>
		public static T As<T>(this IExtension extension) where T : class => extension as T;
	}
}