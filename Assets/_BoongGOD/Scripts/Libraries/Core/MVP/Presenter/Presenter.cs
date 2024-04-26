using System;

namespace Redbean.Static
{
	public class Presenter : IPresenter, IDisposable
	{
		protected object ViewProperty { get; private set; }
		
		public void BindView(IView view)
		{
			ViewProperty = view;
		}
		
		public virtual void Setup()
		{
		}

		public virtual void Dispose()
		{
		}
	}
}