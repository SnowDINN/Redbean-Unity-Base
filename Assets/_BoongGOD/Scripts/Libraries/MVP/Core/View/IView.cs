using UnityEngine;

namespace Redbean.MVP
{
	public interface IView : IMVP
	{
		GameObject GetGameObject();
	}
}