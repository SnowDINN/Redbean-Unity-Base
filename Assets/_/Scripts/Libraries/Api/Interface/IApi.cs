using System.Threading.Tasks;

namespace Redbean.Api
{
	public interface IApi
	{
		Task<Response> Request(params object[] args);
	}
}