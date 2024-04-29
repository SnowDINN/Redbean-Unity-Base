using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Redbean.MVP
{
	public class PresenterAttribute : PropertyAttribute
	{
		public List<string> presenterArray;

		public PresenterAttribute()
		{
			presenterArray = AppDomain.CurrentDomain.GetAssemblies()
			                          .SelectMany(x => x.GetTypes())
			                          .Where(x => typeof(IPresenter).IsAssignableFrom(x)
			                                      && !x.IsInterface
			                                      && !x.IsAbstract)
			                          .Select(x => x.FullName)
			                          .ToList();

			var className = typeof(Presenter).FullName;
			if (presenterArray.Contains(className))
				presenterArray.Remove(className);
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(PresenterAttribute))]
	public class PresenterDrawer : PropertyDrawer
	{
		private PresenterAttribute target => (PresenterAttribute)attribute;
		private int index;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var index = target.presenterArray.IndexOf(property.stringValue);
			if (index < 0)
				index = 0;
			index = EditorGUI.Popup(position, string.Empty, index, target.presenterArray.ToArray());
			
			property.stringValue = target.presenterArray[index];
		}
	}
#endif
}