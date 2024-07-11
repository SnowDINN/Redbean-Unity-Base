using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Redbean.MVP;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Redbean.Editor
{
	internal partial class ApplicationWindow
	{
		private const string FinderTab = "Finder";
		
		private const string PresenterGroup = "Tabs/Finder/Presenter Finder";
		private const string PlayerPrefsGroup = "Tabs/Finder/PlayerPrefs Finder";
		
		private const int PresenterOrder = 10;
		private const int PlayerPrefsOrder = 20;
		
		private readonly List<PresenterSearchable> presenterSearch = new();
		private static List<string> presenterArray => 
			AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => typeof(IPresenter).IsAssignableFrom(x)
				            && !x.IsInterface
				            && !x.IsAbstract)
				.Select(x => x.FullName)
				.ToList();

		[TabGroup(TabGroup, FinderTab), TitleGroup(PresenterGroup), PropertyOrder(PresenterOrder), ListDrawerSettings(IsReadOnly = true, ShowPaging = true, NumberOfItemsPerPage = 10), ShowInInspector, HideReferenceObjectPicker, Searchable]
		private List<PresenterSearchable> presenterGroup
		{
			get
			{
				if (presenterSearch.Any())
					return presenterSearch;
				
				foreach (var presenter in presenterArray)
					presenterSearch.Add(new PresenterSearchable(presenter));
				
				return presenterSearch;
			}
			set
			{
			}
		}

		[TabGroup(TabGroup, FinderTab), TitleGroup(PlayerPrefsGroup), PropertyOrder(PlayerPrefsOrder), DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout), ShowInInspector, ReadOnly]
		private Dictionary<string, object> playerPrefsGroup = new();
	}

	public class PresenterSearchable
	{
		public PresenterSearchable(string presenter)
		{
			Presenter = presenter;
		}
		
		[InlineButton(nameof(Search), SdfIconType.Search, ""), LabelText("")]
		public string Presenter;

		public async void Search()
		{
			var assetPaths = AssetDatabase.GetAllAssetPaths().Where(_ =>
				                                                        _.EndsWith(".prefab") || 
				                                                        _.EndsWith(".unity"))
				.ToArray();
			
			foreach (var assetPath in assetPaths)
			{
				var text = await File.ReadAllTextAsync(assetPath);
				if (text.Contains(Presenter))
				{
					Log.Print(assetPath);
				}
			}
		}
	}
}