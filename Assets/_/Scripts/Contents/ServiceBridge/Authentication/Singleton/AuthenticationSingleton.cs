using System;
using System.Collections.Generic;
using System.Linq;
using Redbean.Auth;

namespace Redbean.Singleton
{
	public class AuthenticationSingleton : ISingletonContainer
	{
		private readonly Dictionary<AuthenticationType, IAuthentication> authenticationGroup = new();
		
		public AuthenticationSingleton()
		{
			var authentications = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(_ => _.GetTypes())
				.Where(_ => _.FullName != null
				            && typeof(IAuthentication).IsAssignableFrom(_)
				            && !_.IsInterface
				            && !_.IsAbstract)
				.Select(_ => Activator.CreateInstance(Type.GetType(_.FullName)) as IAuthentication)
				.ToArray();

			foreach (var authentication in authentications)
				authenticationGroup.TryAdd(authentication.Type, authentication);
		}
		
		public void Dispose()
		{
			authenticationGroup.Clear();
		}

		public IAuthentication GetPlatform(AuthenticationType type) => authenticationGroup[type];
	}
}