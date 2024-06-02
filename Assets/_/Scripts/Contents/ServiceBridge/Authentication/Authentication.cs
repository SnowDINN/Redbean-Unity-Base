using System;
using System.Collections.Generic;
using System.Linq;

namespace Redbean.ServiceBridge
{
	public class Authentication : ISingleton
	{
		private readonly Dictionary<AuthenticationType, IAuthentication> authentications = new();
		
		public Authentication()
		{
			var authentications = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => x.FullName != null
				            && typeof(IAuthentication).IsAssignableFrom(x)
				            && !x.IsInterface
				            && !x.IsAbstract)
				.Select(x => Activator.CreateInstance(Type.GetType(x.FullName)) as IAuthentication);
			
			foreach (var auth in authentications
				         .Where(authentication => this.authentications.TryAdd(authentication.Type, authentication)))
				Log.Success("Authentication", $"Initialization succeeded {auth.GetType().FullName}");
		}
		
		public void Dispose()
		{
			authentications.Clear();
		}

		public IAuthentication GetPlatform(AuthenticationType type) => authentications[type];
	}
}