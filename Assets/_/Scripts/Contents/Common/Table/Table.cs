using System.Collections.Generic;

namespace Redbean.Table
{
	public partial class TableContainer
	{
		public static Dictionary<string, TLocalization> Localization = new();
		public static Dictionary<int, TItem> Item = new();
	}
}
