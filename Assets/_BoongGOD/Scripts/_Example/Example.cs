using Cysharp.Threading.Tasks;
using Redbean.Content.Model;
using Redbean.Extension;
using UnityEngine;
using Console = Redbean.Extension.Console;

namespace Redbean.Example
{
	public class Example : MonoBehaviour
	{
		private void Start()
		{
			UniTask.Void(Async);
		}

		private async UniTaskVoid Async()
		{
			await UniTask.WaitForSeconds(1.0f);
			Console.Log(this.GetModel<AppConfigModel>().android.version);
		}
	}	
}