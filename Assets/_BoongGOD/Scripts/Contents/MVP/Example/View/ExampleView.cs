using Redbean.Static;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Content.MVP
{
	public class ExampleView : View
	{
		[SerializeField] private Button button;
		public Button Button => button;
		
		[SerializeField] private Image image;
		public Image Image => image;
	}
}