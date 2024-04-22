using UnityEngine;

namespace Redbean.Extension
{
	public static class ComponentExtension
	{
		public static void Active(this Component component, bool value) =>
			component.gameObject.SetActive(value);
	}   
}