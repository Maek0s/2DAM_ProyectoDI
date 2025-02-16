using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

namespace LoginSystemPowerCode.ViewModels
{
    public class CarteraViewModel:INotifyPropertyChanged
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
        public ICommand NavegarPerfilCommand { get; }
        public ICommand ViajarHomeCommand { get; }
        public ICommand ViajarSalidaCommand { get; }
        public ICommand ViajarGestionUsuariosCommand { get; }

        public CarteraViewModel()
        {
            Debug.WriteLine("Este es el id: "+UsuarioID);

            NavegarPerfilCommand = new Command(viajarPerfil);
            ViajarHomeCommand = new Command(viajarHome);
            ViajarSalidaCommand = new Command(viajarSalida);
            ViajarGestionUsuariosCommand = new Command(viajarGestionUsuarios);
        }

        private async void viajarPerfil()
        {
            await Shell.Current.GoToAsync($"/Perfil?usuario={UsuarioID}");
        }
        private async void viajarGestionUsuarios()
        {
            await Shell.Current.GoToAsync($"/GestionUsuarios?usuario={UsuarioID}");
        }
        private async void viajarHome()
        {
            await Shell.Current.GoToAsync($"/PaginaPrincipal?usuario={UsuarioID}");
        }
        private async void viajarSalida()
        {
            await Shell.Current.GoToAsync($"/LoginPage?");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

