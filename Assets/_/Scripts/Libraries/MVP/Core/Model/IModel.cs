namespace Redbean.MVP
{
	public interface IModel : IMVP
	{
	}

	public interface ISerializeModel : IModel
	{
		IRxModel Rx { get; }
	}

	public interface IRxModel : IModel
	{
		void Publish(ISerializeModel value);
	}
}