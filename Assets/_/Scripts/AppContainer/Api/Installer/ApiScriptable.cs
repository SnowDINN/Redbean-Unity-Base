using Redbean.Base;
using UnityEngine;

namespace Redbean.Api
{
	[CreateAssetMenu(fileName = "ApiScriptable", menuName = "Redbean/Library/ApiScriptable")]
	public class ApiScriptable : ScriptableObject
	{
		[Header("Get generation path")]
		public string ProtocolPath;
	}
	
	public class ApiReferencer : ScriptableBase<ApiScriptable>
	{
		public const string ApiUri = "http://localhost";
		
#if UNITY_EDITOR
		public static string ProtocolPath
		{
			get => Scriptable.ProtocolPath;
			set
			{
				Scriptable.ProtocolPath = value;
				Save();
			}
		}
#endif
	}
}