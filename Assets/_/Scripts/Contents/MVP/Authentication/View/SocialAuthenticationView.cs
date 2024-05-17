using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.MVP.Content
{
	public class SocialAuthenticationView : View
	{
		[SerializeField] private AuthenticationType type;
		public AuthenticationType Type => type;

		[SerializeField] private Button button;
		public Button Button => button;
		
		[SerializeField] private TMP_InputField inputField;
		public TMP_InputField InputField => inputField;
	}
}