namespace Redbean.Table
{
	public class TItem : ITableContainer
	{
		public int Id;
		public string Name;

		public void Apply(string value)
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
