namespace Redbean
{
	public interface IExtension
	{
	}
	
	public enum BootstrapType
	{
		Runtime = 0,
		SignInUser = 100,
	}

	public static class Key
	{
		public const string GetDataGroup = "LOCALDATA_GROUP";
	}
}