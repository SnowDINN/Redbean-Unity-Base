using System;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Redbean.Api;

namespace Redbean.Auth
{
	public interface IAuthentication : IExtension
	{
		AuthenticationType Type { get; }
		bool IsInitialize { get; set; }
		Task<bool> Initialize(CancellationToken cancellationToken = default);
		Task<AuthenticationResult> Login(CancellationToken cancellationToken = default);
		Task<AuthenticationResult> AutoLogin(CancellationToken cancellationToken = default);
	}

	public class AuthenticationResult
	{
		public Credential Credential;
		public int Code;
		public string Message;
	}
}