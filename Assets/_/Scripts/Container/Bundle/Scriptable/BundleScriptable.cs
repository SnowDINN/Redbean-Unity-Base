using Redbean.Base;
using UnityEngine;

namespace Redbean
{
	[CreateAssetMenu(fileName = "BundleScriptable", menuName = "Redbean/Library/BundleScriptable")]
	public class BundleScriptable : ScriptableBase
	{
		[Header("Get addressable information during runtime")]
		public string[] Labels;
	}
}