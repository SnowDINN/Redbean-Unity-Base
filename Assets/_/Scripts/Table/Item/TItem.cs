using System.Collections.Generic;

namespace Redbean.Table
{
	public class TItem : ITable
	{
		public int Id;
		public string Name;

		public void Apply(IEnumerable<string> values)
		{
			TableContainer.Item.Clear();

			foreach (var value in values)
			{
				var split = value.Split("\t");
				var item = new TItem
				{
					Id = int.Parse(split[0]),
					Name = split[1],
				};

				TableContainer.Item.Add(item.Id, item);
			}
		}
	}
}
