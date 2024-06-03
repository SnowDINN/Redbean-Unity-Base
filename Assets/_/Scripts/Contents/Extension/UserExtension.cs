using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Redbean.Firebase;
using Redbean.MVP;
using Redbean.MVP.Content;
using UnityEngine;

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
		public static UserModel SetReferenceUser(this UserModel model)
		{
			if (!string.IsNullOrEmpty(model.Social.Id))
				UserDB = Firestore.Collection(FirebaseDefine.Users).Document(model.Social.Id);
			
			return model.Publish().SetPlayerPrefs(typeof(UserModel).FullName);
		}
		
		public static async UniTask<bool> TryGetUserSnapshot(this UserModel model, string id)
		{
			var equalTo = Firestore.Collection(FirebaseDefine.Users).WhereEqualTo($"{DataKey.USER_SOCIAL_KEY}.{DataKey.USER_ID_KEY}", id);
			var querySnapshot = await equalTo.GetSnapshotAsync();
			if (querySnapshot.Any())
			{
				querySnapshot.Documents
				             .Select(_ => _.ConvertTo<UserModel>())
				             .FirstOrDefault(_ =>  _.Social.Id == id)
				             .Publish();
				
				Log.Print($"User stored on the server exists. [ {model.Information.Nickname} | {model.Social.Id} ]");
				return true;
			}
			
			Log.Print("User stored on the server not exists.", Color.red);
			return false;
		}
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static async UniTask<bool> UserCreateAsync(this UserModel model)
		{
			SetReferenceUser(model);
			
			var uniTask = model.CreateFirestore();
			await uniTask;
			
			return uniTask.Status == TaskStatus.RanToCompletion;
		}
		
		/// <summary>
		/// 유저 데이터 업데이트
		/// </summary>
		public static async UniTask<bool> UserUpdateAsync(this UserModel model)
		{
			var uniTask = model.UpdateFirestore(DataKey.USER_INFORMATION_KEY);
			await uniTask;

			return uniTask.Status == TaskStatus.RanToCompletion;
		}
	}
}