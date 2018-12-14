using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Exam_Master_SutterlinKleinclaus_EditeurMap
{
    class Image_tile
    {
        int _nombre;
        CroppedBitmap _source;

        public int nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public CroppedBitmap source
        {
            get { return _source; }
            set { _source = value; }
        }

        

        public Image_tile()
        {

        }

        public Image_tile(int mnombre, CroppedBitmap msource,Image myImg)
        {
            this.nombre = mnombre;
            this.source = msource;

        }

        
       

    }
}
