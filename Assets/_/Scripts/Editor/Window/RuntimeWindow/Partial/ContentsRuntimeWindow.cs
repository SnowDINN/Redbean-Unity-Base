using Redbean.Dependencies;
using Redbean.Popup;
using Redbean.Popup.Content;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string ExampleTitle = "Example";
		
		[TabGroup(ContentsTab), Title(ExampleTitle), DisableInEditorMode, Button]
		private void OpenPopupExample()
		{
			SingletonContainer.GetOrAdd<PopupBinder>().Open<PopupExample>();
		}
	}
}