using System;

namespace Redbean.Static
{
	public interface IPresenter : IDisposable
	{
		void BindView(IView view);
		void Setup();
	}
}