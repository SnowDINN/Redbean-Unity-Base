using System.Collections.Generic;

namespace Redbean.Table
{
	public class TableContainer
	{
		public static Dictionary<string, TLocalization> Localization = new();
		public static Dictionary<int, TItem> Item = new();
	}
}
