using Cysharp.Threading.Tasks;

namespace Redbean.Core
{
	public interface IApplicationCore
	{
		/// <summary>
		/// 실행 순서
		/// </summary>
		int ExecutionOrder { get; }
		
		/// <summary>
		/// 앱 시작 시 실행되는 함수
		/// </summary>
		UniTask Setup();
		
		/// <summary>
		/// 앱 종료 시 실행되는 함수
		/// </summary>
		UniTask TearDown();
	}
}