using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Redbean.Base;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Redbean.MVP
{
	public class View : MonoBase, IView
	{
		[HideInInspector]
		public string PresenterFullName;
		
		private Presenter presenter;
		
		public virtual void Awake()
		{
			var type = Type.GetType(PresenterFullName);
			presenter = AppDomain.CurrentDomain.GetAssemblies()
			                     .SelectMany(x => x.GetTypes())
			                     .Where(x => type != null
			                                 && type.IsAssignableFrom(x)
			                                 && typeof(Presenter).IsAssignableFrom(x)
			                                 && !x.IsInterface
			                                 && !x.IsAbstract)
			                     .Select(x => (Presenter)Activator.CreateInstance(Type.GetType(x.FullName)))
			                     .FirstOrDefault();
			
			presenter?.BindView(this);
			presenter?.Setup();
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			
			presenter?.Dispose();
		}
		
		public GameObject GetGameObject() => gameObject;
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(View), true)]
	public class ViewEditor : Editor
	{
		private View view => (View)target;
		
		private List<string> presenterArray;
		private bool useSerializeField;
		
		private void OnEnable()
		{
			presenterArray = AppDomain.CurrentDomain.GetAssemblies()
			                          .SelectMany(x => x.GetTypes())
			                          .Where(x => typeof(IPresenter).IsAssignableFrom(x)
			                                      && !x.IsInterface
			                                      && !x.IsAbstract)
			                          .Where(x => x.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Any(_ => _.FieldType == view.GetType()))
			                          .Select(x => x.FullName)
			                          .ToList();
			
			if (presenterArray.Contains(typeof(Presenter).FullName))
				presenterArray.Remove(typeof(Presenter).FullName);
			
			foreach (var field in view.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
			{
				var attributes = field.GetCustomAttributes(false);
				if (!attributes.Any())
					continue;

				useSerializeField = attributes.Any(_ => _ is SerializeField);
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			{
				EditorGUILayout.LabelField("Presenter", EditorStyles.boldLabel);
				if (presenterArray.Any())
				{
					EditorGUI.BeginChangeCheck();
					{
						var index = presenterArray.IndexOf(view.PresenterFullName);
						if (index < 0)
							index = 0;
						index = EditorGUILayout.Popup(string.Empty, index, presenterArray.ToArray());
					
						view.PresenterFullName = presenterArray[index];
					}
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						EditorUtility.SetDirty(view);
					}
				}
				else
					EditorGUILayout.LabelField("This View associated with the Presenter does not exist.");

				if (useSerializeField)
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("View", EditorStyles.boldLabel);
				}

				DrawPropertiesExcluding(serializedObject, "m_Script");
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
#endif
}