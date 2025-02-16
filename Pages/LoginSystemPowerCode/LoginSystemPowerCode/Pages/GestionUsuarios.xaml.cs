using LoginSystemPowerCode.ViewModels;

namespace LoginSystemPowerCode.Pages
{
	public partial class GestionUsuarios : ContentPage
	{
		public GestionUsuariosViewModel _viewModel { get; set; } = new GestionUsuariosViewModel();

		public GestionUsuarios()
		{
			InitializeComponent();

            BindingContext = _viewModel;
        }
	}
}