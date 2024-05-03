namespace Redbean
{
	public enum AuthenticationType
	{
		Guest,
		Google,
		Apple,
	}
	
	public class LocalKey
	{
		public const string USER_ID_KEY = "id";
		public const string USER_INFO_KEY = "user_information";
		public const string USER_NICKNAME_KEY = "nickname";
	}
}