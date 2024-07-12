using System.Collections.Generic;
using System.Linq;

namespace Redbean.Editor
{
	public partial class ApplicationWindow : WindowBase
	{
		private const string TabGroup = "Tabs";
		
		protected override void OnEnable()
		{
			base.OnEnable();
			
			presenterList = presenterArray.Any() ? presenterArray : new List<PresenterSearchable>();
			playerPrefsList = playerPrefsArray.Any() ? playerPrefsArray : new List<PlayerPrefsViewer>();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			
			presenterList.Clear();
			playerPrefsList.Clear();
		}
	}
}