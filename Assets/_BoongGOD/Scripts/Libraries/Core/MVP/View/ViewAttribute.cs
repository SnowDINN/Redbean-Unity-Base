using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Redbean.Static
{
	public class TargetPresenterAttribute : PropertyAttribute
	{
		public List<string> presenterArray;

		public TargetPresenterAttribute()
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

	[CustomPropertyDrawer(typeof(TargetPresenterAttribute))]
	public class TargetPresenterDrawer : PropertyDrawer
	{
		private TargetPresenterAttribute target => (TargetPresenterAttribute)attribute;
		private int index;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var index = target.presenterArray.IndexOf(property.stringValue);
			if (index < 0)
				index = 0;
			index = EditorGUI.Popup(position, label.text, index, target.presenterArray.ToArray());
			
			property.stringValue = target.presenterArray[index];
			
			EditorGUILayout.Space();
		}
	}
}