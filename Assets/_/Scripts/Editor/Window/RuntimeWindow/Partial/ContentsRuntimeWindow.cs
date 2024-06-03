﻿using System;
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
	}
}