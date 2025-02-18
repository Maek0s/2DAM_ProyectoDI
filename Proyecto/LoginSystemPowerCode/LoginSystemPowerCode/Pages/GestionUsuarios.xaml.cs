using LoginSystemPowerCode.Systems;
using LoginSystemPowerCode.ViewModels;

namespace LoginSystemPowerCode.Pages
{
    [QueryProperty("UsuarioID", "usuario")]
    public partial class GestionUsuarios : ContentPage
	{
        private readonly MgtDatabase mgtDatabase = new MgtDatabase();
        private readonly GestionUsuariosViewModel _viewModel;
        public string UsuarioID { get; set; }  // Propiedad para recibir el parámetro

        public GestionUsuarios()
		{
			InitializeComponent();

            _viewModel = new GestionUsuariosViewModel(this);
            BindingContext = _viewModel;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            if (!string.IsNullOrWhiteSpace(UsuarioID))
            {
                _viewModel.UsuarioID = UsuarioID;
            }
        }
    }
}