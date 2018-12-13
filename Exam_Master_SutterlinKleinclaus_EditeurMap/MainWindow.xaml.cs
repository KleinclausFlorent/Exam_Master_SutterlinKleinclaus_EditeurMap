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

using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Exam_Master_SutterlinKleinclaus_EditeurMap
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[,] tab_map = new int[8, 8]; //ce tableau représente les données constituant la map sur laquelle on travaille, qu'on peut charger et enregistrer.
        int[,] tab_tileset = new int[6, 5]; //celui là représente les données constituant le tileset qu'on souhaite charger.
        int memoire; //cette variable contient la valeur de la case du tileset qu'on a cliqué.


       


        ///<remarks>
        /// Code temporaire a placer dans une classe chargée de dessiner la map dans le canvas avec une méthode init ou draw empty map
        /// Permet de remplir de case vide le canvas en fonction de la taille de case voulu et de la taille de l'objet canvas
        /// Un objet contenant un tableau d'objet Case comme un objet Map permettra de garder en mémoire les cases créées et leurs positions. 
        /// On pourra rajouter d'autres informations dans l'objet case
        /// </remarks>
        public MainWindow()
        {

            InitializeComponent();

            Init_Tabs(); //fonction définie un peu plus loin permettant d'initier les tableaux.

            int mTaille_Case = 64;
            // int mNombre_Case = Convert.ToInt32(MyMap.Width) / mTaille_Case;
            int mNombre_Case = 8;
            for (int i = 0; i < mNombre_Case; i++)
            {
                for (int j = 0; j < mNombre_Case; j++)
                {
                    System.Windows.Shapes.Rectangle rect;






                    rect = new System.Windows.Shapes.Rectangle();
                    rect.Stroke = new SolidColorBrush(Colors.Black);
                    Case mCase = new Case(mTaille_Case, mTaille_Case, i * mTaille_Case, j * mTaille_Case, 0); 
                    //j'ai modifié la classe case, il faut lui donner une valeur en dernier paramètre. 
                    rect.Fill = test_couleur(tab_map,i, j); //en fonction du tableau représentant la map, la fonction modifie la couleur du carré.
                    
                    rect.StrokeThickness = 2;
                    rect.Width = mCase.Width;
                    rect.Height = mCase.Height;
                    Canvas.SetLeft(rect, mCase.X);
                    Canvas.SetTop(rect, mCase.Y);
                    MyMap.Children.Add(rect); //jusque là, c'est ton code.
                    rect.MouseLeftButtonDown += (s, e) => //Important: ce code gère l'évènement "click sur un rectangle ajouté dans le canvas"
                    {                                     //le += est une façon de dire "override" je crois, mais jsuis pas sur. En tous cas faut le mettre.
                        //on est dans le cas de figure ou un rectangle est cliqué. Ce qui est pratique, c'est qu'il sait quel rectangle est cliqué !
                        int k = mCase.X / mTaille_Case;
                        int l = mCase.Y / mTaille_Case;
                        //on transforme les cordonnées de la case en coordonnée de tableau (on fait l'inverse de ce que tu as fait avant, quand tu as initialisé
                        //les coordonnées x et y des cases

                        tab_map[k, l] = memoire; //on met la valeur qu'on a gardé en mémoire dans le tableau.
                        Texte_test.Text = tab_map[k,l].ToString(); //ca me sert à débugguer. Ici j'affiche ce que je viens de mettre dans le tableau.
                        rect.Fill = test_couleur(tab_map, k, l); //je remplis le carré avec la bonne couleur.



                      

                    };





                }




            }
            
            //maintenant on dessine le tableau représentant le tileset
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    System.Windows.Shapes.Rectangle rect2; //j'ai recopié ton code pour créer une nouvelle grille.


                    rect2 = new Rectangle();
                    rect2.Stroke = System.Windows.Media.Brushes.Black;
                    Case mCase2 = new Case(mTaille_Case, mTaille_Case, i * mTaille_Case, j * mTaille_Case, tab_tileset[i,j]);

                    rect2.Fill = test_couleur(tab_tileset, i, j);


                    rect2.Width = 64;
                    rect2.Height = 64;
                    Canvas.SetLeft(rect2, mCase2.X);
                    Canvas.SetTop(rect2, mCase2.Y);
                    Canvas_Test.Children.Add(rect2);

                    rect2.MouseLeftButtonDown += (s, e) =>
                    {
                       //quand on clique sur une case de la grille deux, on récupère la valeur associée à la case cliquée.
                       //elle servira à mettre à jour la première grille en cliquant dessus
                        memoire =  mCase2.Value;
                        Texte_test.Text = mCase2.Value.ToString(); //pareil, c'est juste pour tester cette ligne.


                    };

                    // var img = new Image();
                    //pour créer des instances d'images: https://stackoverflow.com/questions/29201453/spawning-images-pictureboxes
                    //a terme, faudra remplacer le code qui crée des rectangles par des images. (je pense)






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
        /// <summary>
        /// la fonction permettant d'initialiser les tableaux.
        /// </summary>
        private void Init_Tabs()
        {
            for (int i = 0; i < tab_map.GetLength(0); i++)
            {
                for (int j = 0; j < tab_map.GetLength(1); j++)
                {
                    tab_map[i, j] = 1; //si tu mets 1,2,3,ou 4, tu modifies la couleur de base du tableau.
                }
            }

            int tmp = 0;
            for (int i = 0; i < tab_tileset.GetLength(0); i++)
            {
                for (int j = 0; j < tab_tileset.GetLength(1); j++)
                {
                    tab_tileset[i, j] = tmp;
                    tmp++;
                    if (tmp == 4)
                    {
                        tmp = 0; //on initialise le deuxième tableau. A chaque fois qu'il arrive à 4, il repasse à 0.
                    }
                }
            }
        }
        /// <summary>
        /// cette fonction permet de tester les couleurs du tableau que tu lui passes, et donner la couleur correspondante.
        /// </summary>
        /// <param name="tableau"> le tableau dans lequel tu veux faire le test</param>
        /// <param name="i"> le numéro de ligne</param>
        /// <param name="j"> le numéro de colonne</param>
        /// <returns> Une couleur</returns>
        private SolidColorBrush test_couleur(int[,] tableau, int i, int j)
        {
            switch (tableau[i, j]) {
                case 0:
                    return new SolidColorBrush(Colors.Blue);

                case 1:
                    return new SolidColorBrush(Colors.Yellow);

                case 2:
                    return new SolidColorBrush(Colors.Red);

                case 3:
                    return new SolidColorBrush(Colors.GreenYellow);

                 default:
                    return new SolidColorBrush(Colors.White);


            }
        }
        /// <summary>
        /// La fonction qui s'active quand on appuie sur le bouton sauvegarder. Permet de générer un fichier xml.
        /// </summary>
        /// <param name="sender">l'objet appuyé</param>
        /// <param name="e">permet de récupérer des paramètres spécifiques (je m'en sers pas anyway)</param>
        private void On_Save_Click(object sender, RoutedEventArgs e)
        {
            List<Tile> ListMap = new List<Tile>();
            //je crée une liste de "tile" (classe que j'ai défini moi même)

            for (int i = 0; i < tab_map.GetLength(0); i++)
            {
                for (int j = 0; j < tab_map.GetLength(1); j++)
                {
                    Tile a = new Tile(i, j, tab_map[i, j]);
                    ListMap.Add(a); //je parcours le tableau représentant la map, et j'ajoute chaque valeur dans la liste, ainsi que les coordonnées i et j correspondantes.
                }


            }


            //le script à utiliser pour le xml. Si tu veux de la doc: https://tlevesque.developpez.com/dotnet/xml-serialization/#LI-A-3

            XmlSerializer xs = new XmlSerializer(typeof(List<Tile>)); //il faut spécifier à l'avance ce que tu lui envoies. Ici, je lui dis que je lui envoie
                                                                      //une liste de tile.
            
            using (StreamWriter wr = new StreamWriter("Map.xml")) //on spécifie qu'on veut écrire (streamwriter), et le nom du fichier
            {
                xs.Serialize(wr, ListMap); //on lui donne streamwriter en paramètre, et l'objet liste.
            }
        }
    }
}
