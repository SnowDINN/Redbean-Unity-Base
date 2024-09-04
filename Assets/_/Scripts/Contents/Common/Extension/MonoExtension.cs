using UnityEngine;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 게임 오브젝트 비/활성화
		/// </summary>
		public static void ActiveGameObject(this MonoBehaviour mono, bool value) => mono.gameObject.SetActive(value);

		/// <summary>
		/// 컴포넌트 비/활성화
		/// </summary>
		public static void ActiveComponent(this MonoBehaviour mono, bool value) => mono.enabled = value;
	}
}