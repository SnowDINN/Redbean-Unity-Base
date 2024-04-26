using Redbean.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Content.MVP
{
	public class ExampleView : View
	{
		[SerializeField] private Button button;
		public Button Button => button;
	}
}