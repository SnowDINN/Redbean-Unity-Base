using Redbean.Static;
using TMPro;
using UnityEngine;

namespace Redbean.Content.MVP
{
	public class ExampleView : View
	{
		[SerializeField] private TextMeshProUGUI text;
		public TextMeshProUGUI Text => text;
	}
}