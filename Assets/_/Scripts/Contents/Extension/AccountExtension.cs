using Redbean.MVP;
using Redbean.MVP.Content;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static AccountModel User(this IMVP presenter) => Model.GetOrAdd<AccountModel>();
	}
}