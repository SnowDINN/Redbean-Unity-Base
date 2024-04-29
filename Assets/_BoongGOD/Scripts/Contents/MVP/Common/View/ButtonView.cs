using Redbean.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Content.MVP
{
	public class ButtonView : View
	{
		[SerializeField] private Button button;
		public Button Button => button;
	}
}