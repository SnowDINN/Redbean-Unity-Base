using TMPro;
using UnityEngine;

namespace Redbean.MVP.Content
{
	public class TextView : View
	{
		[SerializeField] private TextMeshProUGUI text;
		public TextMeshProUGUI Text => text;
	}
}