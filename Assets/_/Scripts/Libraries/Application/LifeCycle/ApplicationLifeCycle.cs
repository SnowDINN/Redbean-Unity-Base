using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Redbean
{
	public class ApplicationLifeCycle : MonoBehaviour
	{
		private List<IApplicationSetup> instances = new();
		
		private void OnDestroy()
		{
			foreach (var instance in instances)
				instance.Dispose();
			
			Log.System("App has been terminated.");
			
#if UNITY_EDITOR
			if (EditorApplication.isPlaying)
				EditorApplication.isPlaying = false;
#endif
		}

		public void AddInstances(List<IApplicationSetup> instances)
		{
			this.instances = instances;
			this.instances.Reverse();
		}
	}
}