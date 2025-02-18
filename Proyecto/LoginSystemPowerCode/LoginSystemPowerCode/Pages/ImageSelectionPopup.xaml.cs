using CommunityToolkit.Maui.Views;
using LoginSystemPowerCode.Models;

namespace LoginSystemPowerCode.Pages;

public partial class ImageSelectionPopup : Popup
{
    public event Action<string> ImageSelected;
    public List<ImageModel> ImageCollectionView { get; set; }

    public ImageSelectionPopup()
    {
        InitializeComponent();

        // Lista de imagenes que sirven de avatar
        ImageCollectionView = new List<ImageModel>
        {
            new ImageModel { ImagePath = "avatarlogolol.png" },
            new ImageModel { ImagePath = "avatarlogomarvelrivals.jpg" },
            new ImageModel { ImagePath = "avatarlogoosu.png" },
            new ImageModel { ImagePath = "avatarlolahri.jpg" },
            new ImageModel { ImagePath = "avatarlogopowercode.png" }
        };
        BindingContext = this;
    }

    private void OnImageSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ImageModel selectedImage)
        {
            ImageSelected?.Invoke(selectedImage.ImagePath); // Devuelve la imagen seleccionada
            Close(); // Cierra el popup
        }
    }
}
