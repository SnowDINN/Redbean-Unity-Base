using Cysharp.Threading.Tasks;

namespace Redbean.Core
{
	public interface IApplicationCore
	{
		int ExecutionOrder { get; }
		UniTask Setup();
		UniTask TearDown();
	}
}