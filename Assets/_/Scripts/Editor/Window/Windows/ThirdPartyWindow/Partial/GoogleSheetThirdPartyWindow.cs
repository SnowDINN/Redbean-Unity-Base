using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class ThirdPartyWindow
	{
		private const string GoogleSheetTab = "Google Sheet";
		private const string GoogleSheetGroup = "Tabs/Google Sheet/Sheet settings";
		
		[TabGroup(TabGroup, GoogleSheetTab), TitleGroup(GoogleSheetGroup), LabelText("Client Id"), ShowInInspector]
		private string SheetClientId
		{
			get => googleSheet.GoogleClientId;
			set
			{
				googleSheet.GoogleClientId = value;
				googleSheet.Save();
			}
		}

		[TabGroup(TabGroup, GoogleSheetTab), TitleGroup(GoogleSheetGroup), LabelText("Secret Id"), ShowInInspector]
		private string SheetSecretId
		{
			get => googleSheet.GoogleSecretId;
			set
			{
				googleSheet.GoogleSecretId = value;
				googleSheet.Save();
			}
		}

		[TabGroup(TabGroup, GoogleSheetTab), TitleGroup(GoogleSheetGroup), LabelText("Sheet Id"), ShowInInspector]
		private string SheetId
		{
			get => googleSheet.GoogleSheetId;
			set
			{
				googleSheet.GoogleSheetId = value;
				googleSheet.Save();
			}
		}
	}
}