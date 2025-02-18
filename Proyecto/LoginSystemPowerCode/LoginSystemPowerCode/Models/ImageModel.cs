using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemPowerCode.Models
{
    public class ImageModel
    {
        public string ImagePath { get; set; }

        public ImageModel(string imagePath)
        {
            ImagePath = imagePath;
        }
        public ImageModel()
        {

        }
    }

}
