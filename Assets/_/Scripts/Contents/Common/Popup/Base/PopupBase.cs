using System.Threading.Tasks;
using R3;
using Redbean.Singleton;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using UnityEditor;
#endif

namespace Redbean.Popup
{
	public enum PopupType
	{
		Asset,
		Bundle
	}
	
	public class PopupBase : MonoBase
	{
		[HideInInspector]
		public PopupType Type;
		
		[HideInInspector]
		public int Guid;

		[HideInInspector]
		public Button[] buttons;

		public virtual void Awake()
		{
			foreach (var button in buttons) 
				button.AsButtonObservable().Subscribe(_ => Close()).AddTo(this);
		}

		public virtual void Close() => 
			this.GetSingleton<PopupSingleton>().Close(Guid);

		public async Task WaitUntilClose() =>
			await TaskExtension.WaitUntil(() => destroyCancellationToken.IsCancellationRequested);
	}
	
#if UNITY_EDITOR
	[CustomEditor(typeof(PopupBase), true)]
	public class PopupBaseEditor : Editor
	{
		private PopupBase popup => target as PopupBase;
		private SerializedProperty buttons;
		private bool useSerializeField;

		private void OnEnable()
		{
			buttons = serializedObject.FindProperty("buttons");
			
			foreach (var field in popup.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
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
				EditorGUILayout.PropertyField(buttons, new GUIContent("Close Button"));

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