using Cysharp.Threading.Tasks;

namespace Redbean
{
	public interface IBootstrap
	{
		int ExecutionOrder { get; }
		UniTask Setup();
	}
}