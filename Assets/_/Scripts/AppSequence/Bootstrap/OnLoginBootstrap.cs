using System.Threading.Tasks;
using R3;
using Redbean.Bundle;
using Redbean.Rx;
using Redbean.Table;
using UnityEngine;

namespace Redbean
{
	public class OnLoginBootstrap : Bootstrap
	{
		private readonly CompositeDisposable disposables = new();
		
		protected override async Task Setup()
		{
			TableContainer.Setup();
			await BundleContainer.Setup();

			RxInputBinder.OnKeyInputDetected
				.Where(_ => _ == KeyCode.Escape)
				.Subscribe(_ =>
				{
					this.Popup().CurrentPopupClose();
				}).AddTo(disposables);
		}

		protected override Task Teardown()
		{
			disposables.Clear();
			disposables.Dispose();
			
			return Task.CompletedTask;
		}
	}
}