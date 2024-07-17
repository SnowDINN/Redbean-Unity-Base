using System.Threading.Tasks;
using Redbean.MVP.Content;
using Redbean.Singleton;

namespace Redbean
{
	public static partial class Extension
	{
#if UNITY_EDITOR
		/// <summary>
		/// API 호출
		/// </summary>
		public static async Task<object> EditorRequestApi<T>(this IExtension extension, params object[] args) where T : IApiContainer => 
			await ApiContainer.RequestApi<T>(args);
#endif
		
		/// <summary>
		/// API 호출
		/// </summary>
		public static async Task<object> RequestApi<T>(this IExtension extension, params object[] args) where T : IApiContainer
		{
			T generic = default;
			
			var request = await ApiContainer.RequestApi<T>(args);
			ApiPublish(generic, request);
			
			return request;
		}

		/// <summary>
		/// 팝업 호출
		/// </summary>
		public static PopupSingleton Popup(this IExtension extension) => GetSingleton<PopupSingleton>();
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static UserModel User(this IExtension extension) => GetModel<UserModel>();
		
		/// <summary>
		/// 클래스 변환
		/// </summary>
		public static T As<T>(this IExtension extension) where T : class => extension as T;
	}
}