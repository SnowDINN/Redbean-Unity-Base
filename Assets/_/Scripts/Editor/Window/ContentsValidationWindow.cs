using Redbean.Dependencies;
using Redbean.Popup;
using Redbean.Popup.Content;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string Example = "Example";
		
		[TabGroup(Contents), Title(Example), DisableInEditorMode, Button]
		public void OpenPopupExample()
		{
			DependenciesSingleton.GetOrAdd<PopupManager>().Open<PopupExample>();
		}
	}
}