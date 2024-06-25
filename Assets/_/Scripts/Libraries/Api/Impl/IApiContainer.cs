using System.Threading.Tasks;
using Redbean.Api;

namespace Redbean
{
	public interface IApiContainer : IExtension
	{
		Task<Response> Request(params object[] args);
	}
}