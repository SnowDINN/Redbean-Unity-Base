using System;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string ContentsTab = "Contents";
		private const string ExampleGroup = "Tabs/Contents/Example";
		
		[TabGroup(TabGroup, ContentsTab), TitleGroup(ExampleGroup), DisableInEditorMode, Button("Exception")]
		private void ThrowException()
		{
			throw new Exception("An exception has occurred.");
		}
	}
}