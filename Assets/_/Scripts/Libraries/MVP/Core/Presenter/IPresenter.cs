using UnityEngine;

namespace Redbean.MVP
{
	public interface IPresenter : IMVP
	{
		/// <summary>
		/// 연결되어 있는 게임 오브젝트 호출
		/// </summary>
		GameObject GetGameObject();
		
		/// <summary>
		/// View와 Presenter 연결
		/// </summary>
		void BindView(IView view);
		
		void Setup();
	}
}