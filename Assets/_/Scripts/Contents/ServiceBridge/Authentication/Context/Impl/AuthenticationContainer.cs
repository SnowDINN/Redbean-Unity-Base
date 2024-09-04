using System;
using System.Collections.Generic;
using Redbean.Api;
using Redbean.Auth;

namespace Redbean.Singleton
{
	public class AuthenticationContainer
	{
		private static readonly Dictionary<AuthenticationType, IAuthentication> authentications = new();

		public static IAuthentication GetPlatform(AuthenticationType type)
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
	}
}