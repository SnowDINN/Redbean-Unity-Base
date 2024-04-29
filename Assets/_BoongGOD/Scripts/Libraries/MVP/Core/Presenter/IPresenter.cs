using UnityEngine;

namespace Redbean.MVP
{
	public interface IPresenter : IMVP
	{
		GameObject GetGameObject();
		void BindView(IView view);
		void Setup();
	}
}