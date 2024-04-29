using UnityEngine;
using UnityEngine.UI;

namespace Redbean.MVP.Content
{
	public class ButtonView : View
	{
		[SerializeField] private Button button;
		public Button Button => button;
	}
}