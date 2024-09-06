namespace Redbean
{
	public static class Balance
	{
		public const int DoubleButtonPrevention = 200;
		public const int DoubleInputPrevention = 10;
	}

	public static class BootstrapKey
	{
		public const string OnStart = nameof(OnStart);
		public const string OnLogin = nameof(OnLogin);
	}

	public static class PlayerPrefsKey
	{
		public const string LAST_LOGIN_HISTORY = nameof(LAST_LOGIN_HISTORY);
		public const string GUEST_USER_ID = nameof(GUEST_USER_ID);
	}
}