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
			if (!string.IsNullOrEmpty(model.Social.Id))
				FirebaseSetup.UserDB = FirebaseSetup.Firestore.Collection("users").Document(model.Social.Id);
			
			return model.Publish().SetPlayerPrefs(typeof(UserModel).FullName);
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