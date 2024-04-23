﻿namespace Redbean.Define
{
	public enum AppStartedType
	{
		Success,
		FirebaseError,
		UserInformationError,
		ResourceError,
	}
	
	public static class Balance
	{
		public const int DoubleButtonPrevention = 200;
		public const int DoubleInputPrevention = 10;
	}

	public static class Key
	{
		public const string GetDataGroup = "LOCALDATA_GROUP";
	}
}