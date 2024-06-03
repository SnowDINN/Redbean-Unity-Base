using Redbean.MVP;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this IApplicationBootstrap bootstrap) where T : IModel => GetModel<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IApplicationBootstrap bootstrap) where T : ISingleton => GetSingleton<T>();
	}
}