using R3;

namespace Redbean.Rx
{
	public class RxLocalizationBinder : RxBase
	{
		private static readonly Subject<LanguageType> onLanguageChanged = new();
		public static Observable<LanguageType> OnLanguageChanged => onLanguageChanged.Share();

		public static void Publish(LanguageType type) => onLanguageChanged.OnNext(type);
	}
}