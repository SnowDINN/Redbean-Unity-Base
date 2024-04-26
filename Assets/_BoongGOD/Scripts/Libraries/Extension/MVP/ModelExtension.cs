using Redbean.Rx;
using Redbean.Static;

namespace Redbean.Extension
{
	public static partial class Extension
	{
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this IModel model) where T : IModel => Model.GetOrAdd<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IModel model) where T : ISingleton => Singleton.GetOrAdd<T>();
		
		/// <summary>
		/// 모델 데이터 재정의
		/// </summary>
		public static T Override<T>(this T model) where T : IModel => 
			model is not IModel ? default : Model.Override(model);

		/// <summary>
		/// 모델 데이터 재정의 및 배포
		/// </summary>
		public static T OverrideAndPublish<T>(this T model) where T : IModel => 
			model is not IModel ? default : Singleton.GetOrAdd<RxModelBinder>().Publish(Model.Override(model));
	}
}