namespace Redbean.Table
{
	public class TLocalization : ITable
	{
		public string Id;
		public string Kr;

		public void Apply(string value)
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
