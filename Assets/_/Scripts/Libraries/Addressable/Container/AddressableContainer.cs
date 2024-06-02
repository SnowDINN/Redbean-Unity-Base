using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Redbean.Container
{
	public class AddressableContainer
	{
		public static async Task<GameObject> GetGameObject(string key) =>
			await Addressables.LoadAssetAsync<GameObject>(key).Task;
	}
}