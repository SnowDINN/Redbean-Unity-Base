using System;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string ExampleTitle = "Example";
		
		[TabGroup(ContentsTab), Title(ExampleTitle), DisableInEditorMode, Button]
		private void OpenPopupExample()
		{
			throw new Exception("TEST");
		}
	}
}