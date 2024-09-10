using System;
using System.Collections.Generic;
using Redbean.Api;

namespace Redbean.Auth
{
	public class AuthenticationManager : ISingleton
	{
		private readonly Dictionary<AuthenticationType, IAuthentication> authentications = new();

		public IAuthentication GetPlatform(AuthenticationType type)
		{
			var provider = type switch
			{
				AuthenticationType.Guest => typeof(GuestAuthenticationProvider),
				AuthenticationType.Google => typeof(GoogleAuthenticationProvider),
				AuthenticationType.Apple => typeof(AppleAuthenticationProvider),
				_ => null
			};
			
			if (!authentications.ContainsKey(type))
				authentications[type] = Activator.CreateInstance(provider) as IAuthentication;

			return authentications[type];
		}

		public void Dispose() => authentications.Clear();
	}
}