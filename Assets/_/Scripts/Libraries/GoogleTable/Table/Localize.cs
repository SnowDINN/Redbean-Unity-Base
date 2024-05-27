namespace Redbean.Table
{
	public class Localize : IGoogleTable
	{
		public string Id;
		public string Kr;

		public void Injection(string value)
		{
			var split = value.Split("\t");
			var item = new Localize
			{
				Id = split[0],
				Kr = split[1],
			};

			GoogleTable.Localize.Add(item.Id, item);
		}
	}
}
