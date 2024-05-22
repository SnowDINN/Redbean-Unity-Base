using System;
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
		public static UserModel User(this IPresenter mvp) => GetModel<UserModel>();

		/// <summary>
		/// 유저 데이터 검증
		/// </summary>
		public static async UniTask<UserModel> UserIdValidate(this UserModel model)
		{
			if (string.IsNullOrEmpty(model.Social.Id) && !string.IsNullOrEmpty(model.Social.Platform))
			{
				var x = true;
				while (x)
				{
					var id = $"{Guid.NewGuid()}".Replace("-", "");
					var any = await id.IsContainsData("users", "id");
					if (!any)
					{
						model.Social.Id = id;
						x = false;
					}
				}
			}
			
			FirebaseCore.UserDB = FirebaseCore.Firestore.Collection("users").Document(model.Social.Id);
			
			model.Publish().SetPlayerPrefs(typeof(UserModel).FullName);

			return model;
		}
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static async UniTask<bool> UserCreateAsync(this UserModel model)
		{
			var uniTask = model.CreateFirestore();
			await uniTask;
			
			return uniTask.Status == UniTaskStatus.Succeeded;
		}
		
		/// <summary>
		/// 유저 데이터 업데이트
		/// </summary>
		public static async UniTask<bool> UpdateUserAsync(this UserModel model)
		{
			var uniTask = model.UpdateFirestore(DataKey.USER_INFORMATION_KEY);
			await uniTask;
			
			return uniTask.Status == UniTaskStatus.Succeeded;
		}
	}
}