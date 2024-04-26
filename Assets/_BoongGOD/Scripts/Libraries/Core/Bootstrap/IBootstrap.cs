using Cysharp.Threading.Tasks;

namespace Redbean
{
	public interface IBootstrap
	{
		UniTask Setup();
	}
}