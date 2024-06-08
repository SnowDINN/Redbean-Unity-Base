using System.Threading.Tasks;

namespace Redbean.Api
{
	public interface IApi
	{
		Task Request(params string[] parameters);
	}
}