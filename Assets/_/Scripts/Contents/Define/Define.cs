namespace Redbean
{
	public enum AuthenticationType
	{
		Guest,
		Google,
		Apple,
	}
	
	public class DataKey
	{
		public const string USER_ID_KEY = "id";
		public const string USER_INFORMATION_KEY = "information";
		public const string USER_SOCIAL_KEY = "social";
		public const string USER_PLATFORM_KEY = "platform";
		public const string USER_NICKNAME_KEY = "nickname";
		

	}

	public class StoragePath
	{
		private static string OS
		{
			get
			{
#if UNITY_ANDROID
				return "Android";
#endif
				
#if UNITY_IOS
				return "iOS;
#endif
			}
		}
		
		public static string TableRequest(string name) => $"Table/{ApplicationSettings.Version}/{name}.tsv";
	}
}