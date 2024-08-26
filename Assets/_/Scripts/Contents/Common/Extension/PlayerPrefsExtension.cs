using Redbean.MVP;
using Redbean.Singleton;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static float SetPlayerPrefs(this float value) =>
			GetSingleton<MvpContainer>().Save(value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static int SetPlayerPrefs(this int value) =>
			GetSingleton<MvpContainer>().Save(value);
		
		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static string SetPlayerPrefs(this string value) =>
			GetSingleton<MvpContainer>().Save(value);

		/// <summary>
		/// 로컬 데이터 저장
		/// </summary>
		public static T SetPlayerPrefs<T>(this T value) where T : IModel => 
			GetSingleton<MvpContainer>().Save(value);
		
		/// <summary>
		/// 로컬 데이터 호출
		/// </summary>
		public static T GetPlayerPrefs<T>(this IExtension extension) =>
			GetSingleton<MvpContainer>().Load<T>();
	}
}