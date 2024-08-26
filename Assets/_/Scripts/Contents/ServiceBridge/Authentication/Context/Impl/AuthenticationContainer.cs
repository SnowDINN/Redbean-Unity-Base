using System;
using System.Collections.Generic;
using System.Linq;
using Redbean.Auth;

namespace Redbean.Singleton
{
	public class AuthenticationContainer : ISingletonContainer
	{
		private readonly Dictionary<AuthenticationType, IAuthenticationContainer> authenticationGroup = new();
		
		public AuthenticationContainer()
		{
			var authentications = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(_ => _.GetTypes())
				.Where(_ => _.FullName != null
				            && typeof(IAuthenticationContainer).IsAssignableFrom(_)
				            && !_.IsInterface
				            && !_.IsAbstract)
				.Select(_ => Activator.CreateInstance(_) as IAuthenticationContainer)
				.ToArray();

			foreach (var authentication in authentications)
				authenticationGroup.TryAdd(authentication.Type, authentication);
		}
		
		public void Dispose()
		{
			foreach (var authentication in authenticationGroup.Values)
				authentication.Dispose();
			
			authenticationGroup.Clear();
		}

		public IAuthenticationContainer GetPlatform(AuthenticationType type) => authenticationGroup[type];
	}
}