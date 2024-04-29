using Redbean.MVP;
using TMPro;
using UnityEngine;

namespace Redbean.Content.MVP
{
	public class TextView : View
	{
		[Header("View")]
		
		[SerializeField] private TextMeshProUGUI text;
		public TextMeshProUGUI Text => text;
	}
}