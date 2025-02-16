using LoginSystemPowerCode.ViewModels;

namespace LoginSystemPowerCode.Pages
{
    [QueryProperty("UsuarioID", "usuario")]
    public partial class PaginaPrincipal : ContentPage
    {
        private string _usuarioID;
        public string UsuarioID
        {
            get { return _usuarioID; }
            set
            {
                _usuarioID = value;
            }
        }

        private PaginaPrincipalViewModel _viewModel;

        public PaginaPrincipal()
	    {
		    InitializeComponent();

		    _viewModel = new PaginaPrincipalViewModel();
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.DetenerTemporizador();
        }
    }
}