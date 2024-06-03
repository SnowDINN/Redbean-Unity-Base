using System;
using System.Collections.Generic;
using System.Linq;
using Redbean.ServiceBridge;

namespace Redbean.Singleton
{
	public class AuthenticationSingleton : ISingleton
	{
		private readonly Dictionary<AuthenticationType, IAuthentication> authentications = new();
		
		public AuthenticationSingleton()
		{
			var authentications = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => x.FullName != null
				            && typeof(IAuthentication).IsAssignableFrom(x)
				            && !x.IsInterface
				            && !x.IsAbstract)
				.Select(x => Activator.CreateInstance(Type.GetType(x.FullName)) as IAuthentication);

			foreach (var _ in authentications
				         .Where(authentication => authentication != null && this.authentications.TryAdd(authentication.Type, authentication))) ;
		}
		
		public void Dispose()
		{
			authentications.Clear();
		}

		public IAuthentication GetPlatform(AuthenticationType type) => authentications[type];
	}
}