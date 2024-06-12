using R3;
using Redbean.Api;

namespace Redbean.Rx
{
	public class RxApiBinder : RxBase
	{
		private readonly Subject<IApi> onApiResponse = new();
		private Observable<IApi> OnApiResponse => onApiResponse.Share();
	}
}