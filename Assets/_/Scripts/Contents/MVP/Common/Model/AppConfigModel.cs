using Redbean.Api;

namespace Redbean.MVP.Content
{
	public class AppConfigModel : AppConfigResponse, IModel
	{
		public AppConfigModel()
		{
		}
		
		public AppConfigModel(AppConfigResponse response)
		{
			Maintenance = response.Maintenance;
			Version = response.Version;
		}
	}
}