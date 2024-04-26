using Redbean.Static;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Content.MVP
{
	public class ButtonView : View
	{
		[Header("View")]
		
		[SerializeField] private Button button;
		public Button Button => button;
	}
}