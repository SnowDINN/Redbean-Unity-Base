using UnityEngine;

namespace Redbean.Extension
{
	public static class ComponentExtension
	{
		public static void ActiveSelf(this Component component, bool value) =>
			component.gameObject.SetActive(value);
	}   
}