using System;
using System.Collections.Generic;
using Redbean.Api;
using Redbean.Container;
using Redbean.Singleton;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string ContentsTab = "Contents";
		private const string ExampleGroup = "Tabs/Contents/Example";

		private const int ExampleOrder = 10;
		
		[TabGroup(TabGroup, ContentsTab), TitleGroup(ExampleGroup), PropertyOrder(ExampleOrder), DisableInEditorMode, Button("Exception")]
		private void ThrowException()
		{
			throw new Exception("An exception has occurred.");
		}
		
		[TabGroup(TabGroup, ContentsTab), TitleGroup(ExampleGroup), PropertyOrder(ExampleOrder), DisableInEditorMode, Button("Auto Release")]
		private void AutoRelease()
		{
			SingletonContainer.GetSingleton<AddressableSingleton>().AutoRelease();
		}
		
		[TabGroup(TabGroup, ContentsTab), TitleGroup(ExampleGroup), PropertyOrder(ExampleOrder), Button("Api")]
		private async void Api()
		{
			var result = await ApiGetProtocol.GetAndroidBundlesRequest("0.0.1");
			var list = result.Convert<List<string>>();
		}
	}
}