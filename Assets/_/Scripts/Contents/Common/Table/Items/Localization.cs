namespace Redbean.Table
{
	public class Localization : IGoogleTable
	{
		public string Id;
		public string Kr;

		public void Apply(string value)
		{
			var split = value.Split("\t");
			var item = new Localization
			{
				Id = split[0],
				Kr = split[1],
			};

			TableContainer.Localization.Add(item.Id, item);
		}
	}
}
