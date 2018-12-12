using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exam_Master_SutterlinKleinclaus_EditeurMap
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ///<remarks>
        /// Code temporaire a placer dans une classe charger de dessiner la map dans le canvas avec une méthode init ou draw empty map
        /// Permet de remplir de case vide le canvas en fonction de la taille de case voulu et de la taille de l'objet canvas
        /// Un objet contenant un tableau d'objet Case comme un objet Map permettra de garder en mémoire les cases créées et leurs positions. 
        /// On pourra rajouter d'autres informations dans l'objet case
        /// </remarks>
        public MainWindow()
        {
           
            InitializeComponent();

            
            int mTaille_Case = 16;
            int mNombre_Case = Convert.ToInt32(MyMap.Width) / mTaille_Case;
            for (int i = 0; i < mNombre_Case; i++)
            {
                for (int j = 0; j < mNombre_Case; j++)
                {
                    System.Windows.Shapes.Rectangle rect;
                    rect = new System.Windows.Shapes.Rectangle();
                    rect.Stroke = new SolidColorBrush(Colors.Black);
                    Case mCase = new Case(mTaille_Case, mTaille_Case, i * mTaille_Case, j * mTaille_Case);
                    rect.StrokeThickness = 2;
                    rect.Width = mCase.Width;
                    rect.Height = mCase.Height;
                    Canvas.SetLeft(rect, mCase.X);
                    Canvas.SetTop(rect, mCase.Y);
                    MyMap.Children.Add(rect);
                }
            }
            /* ça marche pas et je sais pas pourquoi. Du coup, j'ai fait la même chose dans le xaml et ça s'affiche correctement dans l'éditeur mais pas dans l'exe
            Image image = new Image();
            image.Source = (new ImageSourceConverter()).ConvertFromString("/Assets/blocks1.png") as ImageSource;
            Canvas.SetLeft(image, 0);
            Canvas.SetTop(image, 0);
            MyTiledMap.Children.Add(image);
            */
        }
    }
}
