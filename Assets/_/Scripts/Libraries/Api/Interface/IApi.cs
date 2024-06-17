using System.Threading.Tasks;

namespace Redbean.Api
{
	public interface IApi : IExtension
	{
		Task<Response> Request(params object[] args);
	}
}