using System;
using Redbean.Api;
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
			SingletonContainer.GetSingleton<BundleSingleton>().AutoRelease();
		}
		
		[TabGroup(TabGroup, ContentsTab), TitleGroup(ExampleGroup), PropertyOrder(ExampleOrder), Button]
		private void AES(string value)
		{
			Log.Print(value.Encryption());
		}
	}
}