using System;
using Firebase.Firestore;
using R3;
using Redbean.Rx;

namespace Redbean.MVP.Content
{
	[FirestoreData]
	public class AccountModel : IApiModel
	{
		[FirestoreProperty]
		public string uid { get; set; } = string.Empty;
		
		[FirestoreProperty]
		public string nickname { get; set; } = string.Empty;

		public void Async()
		{
			this.GetSingleton<RxModelBinder>().OnModelChanged
			    .Where(_ => _.type == typeof(AccountModel))
			    .Subscribe(_ =>
			    {
				    var account = (AccountModel)_.value;
			    });
		}
	}

	public class Account : IRxModel
	{
		public static ReactiveProperty<string> UID = new();
		public static ReactiveProperty<string> Nickname = new();

		public static void CreateAccount() => UID.Value = $"{Guid.NewGuid()}";
	}
}