using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Redbean.Api;
using Redbean.MVP;
using Redbean.Singleton;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Search;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Redbean.Editor
{
	public partial class ApplicationWindow
	{
		private const string FinderTab = "Finder";
		
		private const string PresenterGroup = "Tabs/Finder/Presenter Finder";
		
		private const string PlayerPrefsGroup = "Tabs/Finder/PlayerPrefs Finder";
		
		private const int PresenterOrder = 10;
		private const int PlayerPrefsOrder = 20;

		private List<PresenterSearchable> presenterArray
		{
			get
			{
				var assemblies = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(x => x.GetTypes())
					.Where(x => typeof(IPresenter).IsAssignableFrom(x)
					            && typeof(Presenter).FullName != x.FullName
					            && !x.IsInterface
					            && !x.IsAbstract)
					.Select(x => x.FullName)
					.ToList();

				return assemblies.Select(assembly => new PresenterSearchable(assembly)).ToList();
			}
		}
		
		private List<PlayerPrefsViewer> playerPrefsArray
		{
			get
			{
				if (!PlayerPrefs.HasKey(MvpSingleton.PLAYER_PREFS_KEY))
					return new List<PlayerPrefsViewer>();

				var dataDecrypt = PlayerPrefs.GetString(MvpSingleton.PLAYER_PREFS_KEY).Decryption();
				var dataGroups = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataDecrypt);
				if (dataGroups == null)
					return new List<PlayerPrefsViewer>();

				return (from dataGroup in dataGroups let key = Assembly.Load("Assembly-CSharp")
					        .GetTypes()
					        .FirstOrDefault(_ => _.FullName == dataGroup.Key) 
				        select new PlayerPrefsViewer(key.Name, JToken.Parse(dataGroup.Value).ToString(Formatting.Indented))).ToList();
			}
		}

		[TabGroup(TabGroup, FinderTab), TitleGroup(PresenterGroup)]
		[PropertyOrder(PresenterOrder), ListDrawerSettings(IsReadOnly = true, ShowPaging = true, NumberOfItemsPerPage = 10), ShowInInspector, Searchable]
		private static List<PresenterSearchable> presenterList = new();

		[TabGroup(TabGroup, FinderTab), TitleGroup(PlayerPrefsGroup)]
		[PropertyOrder(PlayerPrefsOrder), ListDrawerSettings(IsReadOnly = true, ShowPaging = true, NumberOfItemsPerPage = 10), ShowInInspector, Searchable]
		private static List<PlayerPrefsViewer> playerPrefsList = new();

		public static void PresenterAllClear()
		{
			foreach (var presenter in presenterList)
				presenter.Clear();
		}
		
		public static void PlayerPrefsAllClear()
		{
			foreach (var playerPrefs in playerPrefsList)
				playerPrefs.Clear();
		}
	}

	[HideReferenceObjectPicker]
	public readonly struct PresenterSearchable
	{
		public PresenterSearchable(string key)
		{
			Presenter = key.Split('.').Last();
			
			Key = $"<b>{key.Split('.').Last()}</b>";
			Value = new Dictionary<Object, List<PresenterItemSearchable>>();
		}
		
		private readonly string Presenter;

		[InlineButton(nameof(Search), SdfIconType.Search, ""), DisplayAsString(EnableRichText = true), ShowInInspector, HideLabel]
		private readonly string Key;
		
		[Title("Reference"), ShowIf(nameof(isAny), Value = true), ShowInInspector]
		private readonly Dictionary<Object, List<PresenterItemSearchable>> Value;
		private bool isAny => Value.Any();

		public void Search()
		{
			ApplicationWindow.PresenterAllClear();
			Clear();

			var presenter = Presenter;
			var assetPaths = AssetDatabase.GetAllAssetPaths().Where(_ =>
				                                                        _.EndsWith(".prefab") || 
				                                                        _.EndsWith(".unity"))
				.ToArray();

			for (var i = 0; i < assetPaths.Length; i++)
			{
				EditorUtility.DisplayProgressBar("Searching", $"Searching Presenter...[ {i} / {assetPaths.Length} ]", i / (float)assetPaths.Length);
				
				var text = File.ReadAllText(assetPaths[i]);
				if (!text.Contains(Presenter))
					continue;

				var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPaths[i]);
				Value.Add(asset, new List<PresenterItemSearchable>());

				if (assetPaths[i].EndsWith(".prefab"))
				{
					var prefab = PrefabUtility.LoadPrefabContents(assetPaths[i]);
					var components = prefab.GetComponentsInChildren<View>()
						.Where(_ => _.PresenterFullName.Split('.').Last() == presenter)
						.Select(_ => new PresenterItemSearchable(assetPaths[i], SearchUtils.GetHierarchyPath(_.gameObject, false)))
						.ToList();
					
					if (components.Any())
						Value[asset] = components;
				}
				else if (assetPaths[i].EndsWith(".unity"))
				{
					var scene = EditorSceneManager.GetSceneByPath(assetPaths[i]);
					var rootGameObjects = scene.GetRootGameObjects();
					foreach (var obj in rootGameObjects)
					{
						var components = obj.GetComponentsInChildren<View>()
							.Where(_ => _.PresenterFullName.Split('.').Last() == presenter)
							.Select(_ => new PresenterItemSearchable(assetPaths[i], SearchUtils.GetHierarchyPath(_.gameObject, false)))
							.ToList();
						
						if (components.Any())
							Value[asset] = components;
					}
				}
			}
			EditorUtility.ClearProgressBar();
		}

		public void Clear() => Value.Clear();
	}
	
	[HideReferenceObjectPicker]
	public readonly struct PresenterItemSearchable
	{
		public PresenterItemSearchable(string key, string value)
		{
			this.key = key;
			this.value = value;
		}

		private readonly string key;
		
		[InlineButton(nameof(Search), SdfIconType.Search, ""), DisplayAsString(EnableRichText = true), ShowInInspector, HideLabel]
		private readonly string value;

		public void Search()
		{
			var presenter = value;
			
			if (key.EndsWith(".prefab"))
			{
				var prefab = PrefabStageUtility.OpenPrefab(key).prefabContentsRoot;
				var component = prefab.GetComponentsInChildren<View>().FirstOrDefault(_ => SearchUtils.GetHierarchyPath(_.gameObject, false).Contains(presenter));
				
				EditorGUIUtility.PingObject(component);
				Selection.activeObject = component;
			}
			else if (key.EndsWith(".unity"))
			{
				EditorSceneManager.OpenScene(key);
				
				var go = GameObject.Find(value);
				EditorGUIUtility.PingObject(go);
				Selection.activeObject = go;
			}
		}
	}
	
	[HideReferenceObjectPicker]
	public struct PlayerPrefsViewer
	{
		public PlayerPrefsViewer(string key, string value)
		{
			Key = $"<b>{key}</b>";
			Value = value;

			isOpen = false;
		}
		
		[InlineButton(nameof(Detail), SdfIconType.EnvelopeOpen, ""), DisplayAsString(EnableRichText = true), ShowInInspector, HideLabel]
		private string Key;

		[Title("JSON"), DisplayAsString(Overflow = false), ShowIf(nameof(isOpen), Value = true), ShowInInspector, HideLabel]
		private string Value;

		private bool isOpen;

		public void Detail()
		{
			ApplicationWindow.PlayerPrefsAllClear();
			isOpen = !isOpen;
		}

		public void Clear() => isOpen = false;
	}
}