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

		public static UserModel UserValidation(this UserModel model)
		{
			if (string.IsNullOrEmpty(model.UserId))
				model.UserId = $"{Guid.NewGuid()}".Replace("-", "");
			model.Publish().SetPlayerPrefs(LocalKey.USER_INFO_KEY);

			return model;
		}
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static async UniTask<bool> CreateUserAsync(this UserModel model)
		{
			FirebaseCore.UserDB = FirebaseCore.Firestore.Collection("users").Document(model.UserId);
			
			var uniTask = model.CreateFirestore();
			await uniTask;
			
			return uniTask.Status == UniTaskStatus.Succeeded;
		}
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static async UniTask<bool> UpdateUserAsync(this UserModel model)
		{
			FirebaseCore.UserDB = FirebaseCore.Firestore.Collection("users").Document(model.UserId);
			
			var uniTask = model.UpdateFirestore(LocalKey.USER_INFO_KEY);
			await uniTask;
			
			return uniTask.Status == UniTaskStatus.Succeeded;
		}
	}
}