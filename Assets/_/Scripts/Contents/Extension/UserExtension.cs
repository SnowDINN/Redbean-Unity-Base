using Cysharp.Threading.Tasks;
using Redbean.MVP;
using Redbean.MVP.Content;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static UserModel User(this IMVP mvp) => Model.GetOrAdd<UserModel>();

		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static async UniTask<bool> CreateAsync(this UserModel model)
		{
			model.SetPlayerPrefs(LocalKey.USER_INFO_KEY);
			var isDone = UniTaskStatus.Succeeded;

			if (GetModel<UserModel>().AuthenticationType != AuthenticationType.Guest)
			{
				var uniTask = model.CreateFirestore();
				await uniTask;
				isDone = uniTask.Status;
			}
			
			return isDone == UniTaskStatus.Succeeded;
		}
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static async UniTask<bool> UpdateAsync(this UserModel model)
		{
			model.SetPlayerPrefs(LocalKey.USER_INFO_KEY);
			var isDone = UniTaskStatus.Succeeded;

			if (GetModel<UserModel>().AuthenticationType != AuthenticationType.Guest)
			{
				var uniTask = model.UpdateFirestore(LocalKey.USER_INFO_KEY);
				await uniTask;
				isDone = uniTask.Status;
			}

			return isDone == UniTaskStatus.Succeeded;
		}
	}
}