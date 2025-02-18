using LoginSystemPowerCode.Systems;
using LoginSystemPowerCode.ViewModels;

namespace LoginSystemPowerCode.Pages
{
    [QueryProperty("UsuarioID", "usuario")]
    public partial class Cartera : ContentPage
    {
        private readonly MgtDatabase mgtDatabase = new MgtDatabase();
        private readonly CarteraViewModel _viewModel;

        private string _usuarioID;
        public string UsuarioID
        {
            get { return _usuarioID; }
            set
            {
                _usuarioID = value;
            }
        }

        public Cartera()
        {
            InitializeComponent();

            _viewModel = new CarteraViewModel();
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
