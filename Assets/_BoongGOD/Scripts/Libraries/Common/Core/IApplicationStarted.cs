using Cysharp.Threading.Tasks;

namespace Redbean.Core
{
	public interface IApplicationStarted
	{
		int ExecutionOrder { get; }
		UniTask Setup();
	}
}