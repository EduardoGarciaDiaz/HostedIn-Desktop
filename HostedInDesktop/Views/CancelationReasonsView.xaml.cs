using HostedInDesktop.Helper;
using HostedInDesktop.viewmodels;

namespace HostedInDesktop.Views;

public partial class CancelationReasonsView : ContentPage
{
    private string GUEST_REASON_ONE = "Ya no necesito un alojamiento";
    private string GUEST_REASON_TWO = "Hice la reservación por error";
    private string GUEST_REASON_THREE = "Me incomoda el anfitrión";
    private string HOST_REASON_ONE = "El huésped no me inspira confianza.";
    private string HOST_REASON_TWO = "Ocuparé el alojamiento para otra actividad.";
    private string HOST_REASON_THREE = "Causa de fuerza mayor: siniestro, interrupción de servicios básicos, conflicto armado.";
    private string GENERAL_REASON = "No deseo especificar el motivo";
	CancelationReasonsViewModel viewModel;
	public CancelationReasonsView()
	{
		InitializeComponent();
        SetReasons();
        BindingContext = ServiceHelper.GetService<CancelationReasonsViewModel>();
		viewModel = (CancelationReasonsViewModel)BindingContext;
		rbtnReasonOne.IsChecked = true;
	}

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		if ((sender as RadioButton).IsChecked) 
		{
			viewModel.SelectedReason = (sender as RadioButton).Content.ToString();
		}
    }

	private void SetReasons()
	{
		if (App.hostMode)
		{
			SetHostReasons();
		}
		else
		{
			SetGuestReasons();
		}
		rbtnReasonFour.Content = GENERAL_REASON;
	}

	private void SetHostReasons()
	{
		rbtnReasonOne.Content = HOST_REASON_ONE;
		rbtnReasonTwo.Content = HOST_REASON_TWO;
		rbtnReasonThree.Content = HOST_REASON_THREE;
	}

	private void SetGuestReasons()
	{
        rbtnReasonOne.Content = GUEST_REASON_ONE;
        rbtnReasonTwo.Content = GUEST_REASON_TWO;
        rbtnReasonThree.Content = GUEST_REASON_THREE;
    }
}