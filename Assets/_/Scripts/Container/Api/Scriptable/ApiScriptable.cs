using Redbean.Base;
using UnityEngine;

namespace Redbean
{
	[CreateAssetMenu(fileName = "ApiScriptable", menuName = "Redbean/Library/ApiScriptable")]
	public class ApiScriptable : ScriptableBase
	{
		[Header("Get api server uri")]
		public string ApiUri = "http://localhost";
		
		[Header("Get generation path")]
		public string ProtocolPath;
	}
}