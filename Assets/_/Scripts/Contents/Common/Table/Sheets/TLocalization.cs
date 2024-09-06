using System.Collections.Generic;

namespace Redbean.Table
{
	public class TLocalization : ITable
	{
		public string Id;
		public string Kr;

		public void Apply(IEnumerable<string> values)
		{
			TableContainer.Localization.Clear();

			foreach (var value in values)
			{
				var split = value.Split("\t");
				var item = new TLocalization
				{
					Id = split[0],
					Kr = split[1],
				};

				TableContainer.Localization.Add(item.Id, item);
			}
		}
	}
}
