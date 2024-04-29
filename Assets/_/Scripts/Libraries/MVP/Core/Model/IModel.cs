using Cysharp.Threading.Tasks;

namespace Redbean.MVP
{
	public interface IModel : IMVP
	{
	}

	public interface IApiModel : IModel
	{
		void Async();
	}

	public interface IRxModel : IModel
	{
	}
}