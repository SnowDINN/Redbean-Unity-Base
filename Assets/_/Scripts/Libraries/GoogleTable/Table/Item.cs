namespace Redbean.Table
{
	public class Item : IGoogleTable
	{
		public long Id;
		public string Name;

		public void Injection(string value)
		{
			var split = value.Split("\t");
			var item = new Item
			{
				Id = long.Parse(split[0]),
				Name = split[1],
			};

			GoogleTable.Item.Add(item.Id, item);
		}
	}
}
