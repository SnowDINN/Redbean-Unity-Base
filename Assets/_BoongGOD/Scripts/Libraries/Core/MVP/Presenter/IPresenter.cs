namespace Redbean.Static
{
	public interface IPresenter
	{
		void BindView(IView view);
		void Setup();
	}
}