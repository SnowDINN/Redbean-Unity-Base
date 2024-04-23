using Cysharp.Threading.Tasks;
using Redbean.Extension;
using Redbean.Static;
using UnityEngine;
using Console = Redbean.Extension.Console;

namespace Redbean.Example
{
	public class Example : MonoBehaviour, ISingleton
	{
		public int index;
		
		private void Awake()
		{
			index = 5;
			
			UniTask.Void(Async);
		}

		private async UniTaskVoid Async()
		{
			await UniTask.WaitForSeconds(2.5f);
			
			Console.Log($"{this.GetSingleton<Example>().index}");
		}
	}	
}