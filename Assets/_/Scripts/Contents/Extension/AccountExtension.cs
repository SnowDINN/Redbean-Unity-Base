using Cysharp.Threading.Tasks;
using Redbean.Firebase;
using Redbean.MVP;
using Redbean.MVP.Content;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static AccountModel User(this IMVP mvp) => Model.GetOrAdd<AccountModel>();

		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static async UniTask<bool> CreateAsync(this AccountModel model)
		{
			UniTaskStatus isDone;

			if (GetModel<AccountModel>().authenticationType == AuthenticationType.Guest)
			{
				model.SetPlayerPrefs(LocalKey.USER_ACCOUNT_KEY);
				isDone = UniTaskStatus.Succeeded;
			}
			else
			{
				var uniTask = model.CreateFirestore(LocalKey.USER_ACCOUNT_KEY);
				await uniTask;
				isDone = uniTask.Status;
			}
			
			return isDone == UniTaskStatus.Succeeded;
		}
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static async UniTask<bool> UpdateAsync(this AccountModel model)
		{
			UniTaskStatus isComplete;

			if (GetModel<AccountModel>().authenticationType == AuthenticationType.Guest)
			{
				model.SetPlayerPrefs(LocalKey.USER_ACCOUNT_KEY);
				isComplete = UniTaskStatus.Succeeded;
			}
			else
			{
				var uniTask = model.UpdateFirestore(LocalKey.USER_ACCOUNT_KEY);
				await uniTask;
				isComplete = uniTask.Status;
			}

			return isComplete == UniTaskStatus.Succeeded;
		}
	}
}