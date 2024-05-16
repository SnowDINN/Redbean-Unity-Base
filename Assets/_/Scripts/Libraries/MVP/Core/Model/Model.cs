namespace Redbean.MVP
{
	public class Model : IModel
	{
	}
	
	public class SerializeModel : Model
	{
		public RxModel Rx { get; protected set; }
	}

	public class RxModel : Model
	{
		public virtual void Publish(SerializeModel value)
		{
		}
	}
}