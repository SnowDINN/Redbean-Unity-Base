using Redbean.MVP;
using Redbean.Rx;
using Redbean.Singleton;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// API 데이터 배포
		/// </summary>
		public static void ApiPublish<T>(this T api, object response) where T : IApiContainer =>
			GetSingleton<RxApiBinder>().Publish<T>(response);
		
		/// <summary>
		/// 모델 데이터 배포
		/// </summary>
		public static T ModelPublish<T>(this T model, bool isPlayerPrefs = false) where T : IModel => 
			GetSingleton<RxModelBinder>().Publish(GetSingleton<MvpSingleton>().Override(model, isPlayerPrefs));
	}
}