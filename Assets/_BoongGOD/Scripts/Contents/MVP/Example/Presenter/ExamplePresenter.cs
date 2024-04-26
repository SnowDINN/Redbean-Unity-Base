using R3;
using Redbean.Extension;
using Redbean.Static;

namespace Redbean.Content.MVP
{
	public class ExamplePresenter : Presenter
	{
		private ExampleView View => (ExampleView)ViewProperty;

		public override void Setup()
		{
			View.Button.AsButtonObservable().Subscribe(_ =>
			{
				Console.Log($"{this.GetModel<AppConfigModel>().android.version}");
			}).AddTo(View);
		}

		public override void Dispose()
		{
			Console.Log($"Destroy component : {View.GetType().FullName}");
		}
	}
}