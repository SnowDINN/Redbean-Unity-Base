using Redbean.Dependencies;
using Redbean.Popup;
using Redbean.Popup.Content;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string Example = nameof(Example);
		
		[TabGroup(Contents), Title(Example), DisableInEditorMode, Button]
		private void OpenPopupExample()
		{
			DependenciesSingleton.GetOrAdd<PopupBinder>().Open<PopupExample>();
		}
	}
}