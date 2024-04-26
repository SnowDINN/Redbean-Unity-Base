using Redbean.Static;
using TMPro;
using UnityEngine;

namespace Redbean.Content.MVP
{
	public class TextView : View
	{
		[Header("Field")]
		
		[SerializeField] private TextMeshProUGUI text;
		public TextMeshProUGUI Text => text;
	}
}