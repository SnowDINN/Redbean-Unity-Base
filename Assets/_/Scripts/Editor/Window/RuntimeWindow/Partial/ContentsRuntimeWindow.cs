using System;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string ExampleTitle = "Example";
		
		[TabGroup(ContentsTab), Title(ExampleTitle), DisableInEditorMode, Button("Exception")]
		private void ThrowException()
		{
			throw new Exception("An exception has occurred.");
		}
	}
}