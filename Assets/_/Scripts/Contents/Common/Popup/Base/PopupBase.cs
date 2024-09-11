using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
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
			buttons
				.Select(_ => _.AsButtonObservable())
				.Merge()
				.Subscribe(_ => Close())
				.AddTo(this);
		}

		public async void Close()
		{
			await PreCloseTask();
			
			this.Popup().Close(Guid);
		}

		public async Task WaitUntilClose() =>
			await UniTask.WaitUntil(() => destroyCancellationToken.IsCancellationRequested);

		protected virtual Task PreCloseTask() => Task.CompletedTask;
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