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
		public static void ApiPublish<T>(this T api, object response) where T : ApiProtocol =>
			GetSingleton<RxApiBinder>().Publish<T>(response);
		
		/// <summary>
		/// 모델 데이터 배포
		/// </summary>
		public static T ModelPublish<T>(this T model) where T : IModel => 
			GetSingleton<RxModelBinder>().Publish(GetSingleton<MvpContainer>().Override(model));
	}
}