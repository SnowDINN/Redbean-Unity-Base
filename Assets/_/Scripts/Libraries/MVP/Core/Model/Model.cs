namespace Redbean.MVP
{
	public interface ISerializeModel : IModel
	{
		public IRxModel Rx { get; }
	}

	public interface IRxModel : IModel
	{
		public void Publish(ISerializeModel value)
		{
		}
	}
}