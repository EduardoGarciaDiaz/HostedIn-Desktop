namespace HostedInDesktop.Views;

public partial class SignupView : ContentPage
{

	public SignupView()
	{
		InitializeComponent();
		dpkBirthDate.Date = DateTime.Now.AddYears(-18);
		dpkBirthDate.MaximumDate = DateTime.Now.AddYears(-18);
		dpkBirthDate.MinimumDate = DateTime.Now.AddYears(-100);
	}
}