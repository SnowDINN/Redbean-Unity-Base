using R3;
using Redbean.Extension;
using Redbean.Static;

namespace Redbean.Content.MVP
{
	public class ExamplePresenter : Presenter
	{
		private ExampleView View => (ExampleView)ViewProperty;

		private const string Token = "Fill";

		public override void Setup()
		{
			View.Button.AsButtonObservable().Subscribe(_ =>
			{
				View.Image.SmoothFill(0, 1, 2.5f, View.GenerateCancellationToken(Token).Token);
			}).AddTo(View);
		}

		public override void Teardown()
		{
			Console.Log($"Destroy component : {View.GetType().FullName}");
		}
	}
}