#if false

using System;
using Anonymous.Editor;
using UnityEditor;
using UnityEngine;

namespace Google
{
	[CustomEditor(typeof(Installer))]
	public class InstallerEditor : Editor
	{
		private static Installer m_installer;
		private static Installer installer
		{
			get
			{
				if (m_installer == null)
					m_installer = Resources.Load<Installer>("GoogleSignIn/Installer");

				return m_installer;
			}
		}
		private static bool isAuto => installer.connectType == ClientConnectType.Auto;
		
		private string AndroidClientId;
		private string IosClientId;
		
		private const float margin = 50;

		private ClientConnectType connectType
		{
			get => installer.connectType;
			set
			{
				if (installer.connectType == value)
					return;
				
				installer.connectType = value;
				Initialize();
			}
		}
		
		private bool foldoutPreferences
		{
			get => Convert.ToBoolean(PlayerPrefs.GetInt("UNITY_EDITOR_FOLDOUT_PREFERENCES"));
			set => PlayerPrefs.SetInt("UNITY_EDITOR_FOLDOUT_PREFERENCES", Convert.ToInt32(value));
		}
		
		private bool foldoutEditorPreferences
		{
			get => Convert.ToBoolean(PlayerPrefs.GetInt("UNITY_EDITOR_FOLDOUT_EDITOR_PREFERENCES"));
			set => PlayerPrefs.SetInt("UNITY_EDITOR_FOLDOUT_EDITOR_PREFERENCES", Convert.ToInt32(value));
		}

		[InitializeOnLoadMethod]
		public static void Initialize()
		{
			if (!isAuto)
				return;

			installer.androidClientId = GoogleExtension.GetAndroidClientId();
			installer.iosClientId = GoogleExtension.GetIosClientId();
			installer.webClientId = GoogleExtension.GetWebClientId();
		}

		public void OnEnable()
		{
			Initialize();
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.BeginHorizontal();
			EditorHeader.Title("Sign In Google Installer", 30);
			if (GUILayout.Button("SAVE", GUILayout.Width(50), GUILayout.Height(40)))
				installer.Save();
			
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorHeader.Line(2);
			
			EditorGUILayout.Space();
			
			foldoutPreferences = CategoryHeader.ShowHeader("Preferences", foldoutPreferences);
			if (foldoutPreferences)
			{
				GUILayout.BeginVertical(GUI.skin.GetStyle("GroupBox"));
				{
					EditorHeader.Title("How to Connect Client ID", 15);
					connectType = (ClientConnectType)EditorGUILayout.EnumPopup("Connect Type", connectType);
					
					EditorGUILayout.Space();
			
					EditorHeader.Title("Client ID", 15);
					EditorGUI.BeginDisabledGroup(isAuto);
					{
						installer.webClientId = EditorGUILayout.TextField("Web Client ID", installer.webClientId);
						installer.androidClientId = EditorGUILayout.TextField("Android Client ID", installer.androidClientId);
						installer.iosClientId = EditorGUILayout.TextField("iOS Client ID", installer.iosClientId);
					}
					EditorGUI.EndDisabledGroup();
				}
				GUILayout.EndVertical();	
			}
					
			foldoutEditorPreferences = CategoryHeader.ShowHeader("Editor-only Preferences", foldoutEditorPreferences);
			if (foldoutEditorPreferences)
			{
				GUILayout.BeginVertical(GUI.skin.GetStyle("GroupBox"));
				{
					EditorGUILayout.LabelField("Web Client Security Secret", EditorStyles.boldLabel);
					installer.webClientSecretId = EditorGUILayout.TextField(installer.webClientSecretId);
			
					EditorGUILayout.Space();
			
					EditorGUILayout.LabelField("Web Redirect URL Port", EditorStyles.boldLabel);
					GUILayout.BeginHorizontal(GUI.skin.GetStyle("Box"));
					{
						var width = 55;
					
						EditorGUILayout.LabelField("http://localhost:", EditorStyles.boldLabel, GUILayout.Width(GetStretchWidthLeft(width)));
						installer.webRedirectPort =
							EditorGUILayout.IntField(installer.webRedirectPort, GUILayout.Width(GetStretchWidthRight(width)));
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();	
			}
		}
		
		private float GetStretchWidthLeft(float width)
		{
			width += margin;
			if (EditorGUIUtility.currentViewWidth > width)
				return EditorGUIUtility.currentViewWidth - width;

			return EditorGUIUtility.currentViewWidth / width;
		}

		private float GetStretchWidthRight(float width)
		{
			if (EditorGUIUtility.currentViewWidth > width)
				return width;

			return EditorGUIUtility.currentViewWidth / width;
		}
	}	
}

#endif