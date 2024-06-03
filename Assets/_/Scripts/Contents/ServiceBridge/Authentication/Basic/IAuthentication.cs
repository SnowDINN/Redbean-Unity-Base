﻿using System.Threading.Tasks;
using Firebase.Auth;

namespace Redbean.Auth
{
	public interface IAuthentication
	{
		AuthenticationType Type { get; }
		Task<bool> Initialize();
		Task<AuthenticationResult> Login();
		Task<AuthenticationResult> AutoLogin();
	}

	public class AuthenticationResult
	{
		public Credential Credential;
		public int Code;
		public string Message;
	}
}