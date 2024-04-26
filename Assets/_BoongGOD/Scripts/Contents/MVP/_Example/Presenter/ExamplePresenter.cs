using R3;
using Redbean.Extension;
using Redbean.Static;

namespace Redbean.Content.MVP
{
	public class ExampleButtonPresenter : Presenter
	{
		private ButtonView View => (ButtonView)ViewProperty;

		public static readonly ReactiveProperty<int> RxInteger = new();

		public override void Setup()
		{
			View.Button.AsButtonObservable().Subscribe(_ =>
			{
				RxInteger.Value += 1;
			}).AddTo(View);
		}

		public override void Dispose()
		{
			Log.Print($"Destroy component : {View.GetType().FullName}");
		}
	}
	
	public class ExampleTextPresenter : Presenter
	{
		private TextView View => (TextView)ViewProperty;

		public override void Setup()
		{
			ExampleButtonPresenter.RxInteger.Subscribe(_ =>
			{
				View.Text.text = $"{_}";
			}).AddTo(View);
		}

		public override void Dispose()
		{
			Log.Print($"Destroy component : {View.GetType().FullName}");
		}
	}
}