using R3;

namespace Redbean.MVP.Content
{
	public class GuestAuthenticationPresenter : Presenter
	{
		[Model]
		private UserModel m_user;
		
		[View]
		private ButtonView view;
		
		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				Login();
			}).AddTo(this);
		}

		private void Login()
		{
			if (string.IsNullOrEmpty(m_user.Information.Nickname))
				m_user.Information.Nickname = "Guest";
			
			m_user.SetReferenceUser();
			
			Log.Print("User logged in as a guest.");
		}
	}
}