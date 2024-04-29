using UnityEngine;

namespace Redbean.Static
{
	public interface IPresenter
	{
		GameObject GetGameObject();
		void BindView(IView view);
		void Setup();
	}
}