using Redbean.Extension;
using Redbean.Static;

namespace Redbean.Content.MVP
{
	public class ExamplePresenter : Presenter
	{
		private ExampleView View => (ExampleView)ViewProperty;
		
		public override void Setup()
		{
			Console.Log(View.Text.text);
		}

		public override void Teardown()
		{

		}
	}
}