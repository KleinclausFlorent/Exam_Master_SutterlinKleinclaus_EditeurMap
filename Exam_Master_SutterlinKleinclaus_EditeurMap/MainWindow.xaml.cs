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
        Rectangle RectMem;
        List<Image> List_ImageMap = new List<Image>(); //liste des images constituant la carte
        List<Image_tile> List_ImageTile = new List<Image_tile>(); //liste des images constituant le tileset
        CroppedBitmap memory; //contient l'image que j'ai cliquée
        List<Tile> Final_ListMap = new List<Tile>();//je crois que je l'utilise pas





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

            Init_Tiledmap();

            int mTaille_Case = 32;
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
                    //on met tout en blanc de toutes façons
                    
                    rect.StrokeThickness = 2;
                    rect.Width = mCase.Width;
                    rect.Height = mCase.Height; 
                    Canvas.SetLeft(rect, mCase.X);
                    Canvas.SetTop(rect, mCase.Y);
                    MyMap.Children.Add(rect); 

                    Image img = new Image(); //ça crée une image à chaque case de la grille.
                    img.Height = mTaille_Case;
                    img.Width = mTaille_Case;
                    img.Source = List_ImageTile[2].source; //je l'initialise sur une image random de la grille
                    img.Opacity = 0; //je met l'opacité à 0, pour la rendre invisible
                    Canvas.SetLeft(img, mCase.X);
                    Canvas.SetTop(img, mCase.Y);
                    List_ImageMap.Add(img);
                   
                    MyMap.Children.Add(img);


                   /*  rect.MouseLeftButtonDown += (s, e) => //Important: ce code gère l'évènement "click sur un rectangle ajouté dans le canvas"
                      {                                     //le += est une façon de dire "override" je crois, mais jsuis pas sur. En tous cas faut le mettre.
                          //on est dans le cas de figure ou un rectangle est cliqué. Ce qui est pratique, c'est qu'il sait quel rectangle est cliqué !
                         
                          //on transforme les cordonnées de la case en coordonnée de tableau (on fait l'inverse de ce que tu as fait avant, quand tu as initialisé
                          //les coordonnées x et y des cases

                          ; //on met la valeur qu'on a gardé en mémoire dans le tableau.
                          Texte_test.Text = tab_map[k,l].ToString(); //ca me sert à débugguer. Ici j'affiche ce que je viens de mettre dans le tableau.
                          rect.Fill = test_couleur(tab_map, k, l); //je remplis le carré avec la bonne couleur.                   
                          MessageBox.Show("You've touched n°" + MyMap.Children.IndexOf(e.OriginalSource as UIElement));

                      }; */
                      //les codes gérant ce qui se passe quand on clique sur une image de la map
                    img.MouseLeftButtonDown += (s, e) =>
                    {
                        // MessageBox.Show("You've touched n°" + MyMap.Children.IndexOf(e.OriginalSource as UIElement));
                        img.Source = memory;
                        img.Opacity = 1;

                        int k = mCase.X / mTaille_Case;
                        int l = mCase.Y / mTaille_Case;
                        tab_map[k, l] = memoire;
                       
                        
                    };
                    //purement esthétique
                    img.MouseEnter += (s, e) =>
                    {
                        if (img.Opacity != 1)
                        {

                            img.Opacity = 0.8;
                            img.Source = memory;
                            
                        }
                    };

                    img.MouseLeave += (s, e) =>
                    {
                        if (img.Opacity != 1)
                        {
                            img.Opacity = 0;
                        }
                    };

                    
                }
            }

            //je pense que ce tableau est inutile maintenant
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

                   
                }
            }
        }

        /// <summary>
        ///  Fonction d'init Tilemap
        /// </summary>        
        private void Init_Tiledmap()
        {
            //Nombre de place dispo dans mon canvas 
            int CanvasNbCol = Convert.ToInt32(MyTiledMap.Width) / 32;
            int CanvasNbLigne = Convert.ToInt32(MyTiledMap.Height) / 32;
            Image[] myimages = new Image[CanvasNbCol * CanvasNbLigne];
            
            int cmpt_image = 0;

            for (int i = 0; i < CanvasNbLigne; i++)
            {
                for (int j = 0; j < CanvasNbCol; j++)
                {
                    myimages[cmpt_image] = new Image();
                    myimages[cmpt_image].Width = 32;
                    Canvas.SetLeft(myimages[cmpt_image], j * 32 );
                    Canvas.SetTop(myimages[cmpt_image], i * 32 );
                    cmpt_image++;
                }
            }
             //Charge l'image
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(@"C:\Users\Seb\Documents\CoursLUDUS\Master\M1\Examen_1\Exam_Master_SutterlinKleinclaus_EditeurMap\Exam_Master_SutterlinKleinclaus_EditeurMap\Assets\blocks1.png");
            myBitmapImage.EndInit();

            
            int compteurImage = 0;
            int nbColonne = Convert.ToInt32(myBitmapImage.PixelWidth) / 32;
            int nbLigne = Convert.ToInt32(myBitmapImage.PixelHeight) / 32;
           

            for (int i = 0; i < nbLigne; i++)
            {
                for (int j = 0; j < nbColonne-1; j++)
                {
                    Image imagetest = new Image();
                    Int32Rect myrect = new Int32Rect( (j*34)+2, (i*34)+2 ,  32, 32);
                    CroppedBitmap cb = new CroppedBitmap(myBitmapImage,myrect);
                   
                    myimages[compteurImage].Source = cb;
                    Image_tile ig = new Image_tile();
                    ig.nombre = compteurImage;
                    ig.source = cb;
                    List_ImageTile.Add(ig); //je récupère les "images tiles" crées, constituées d'un nombre, et d'une source pour l'image.
                    imagetest.Source = cb; //je sais plus pourquoi je fais ça, je crois que ça sert à rien
                    

                    MyTiledMap.Children.Add(myimages[compteurImage]);
                    //ca ce sont les évènements. Quand tu cliques tu mets à jour le tableau map. Sinon le reste, c'est purement esthétique.
                    myimages[compteurImage].MouseEnter += (s, e) =>
                    {
                        //myimages[compteurImage].Source = memory;
                        //   MessageBox.Show("salut "+ compteurImage.ToString());
                        int index_click = MyTiledMap.Children.IndexOf(e.OriginalSource as UIElement);
                        myimages[index_click].Opacity = 0.8;
                    };

                    myimages[compteurImage].MouseLeave += (s, e) =>
                    {
                        //myimages[compteurImage].Source = memory;
                        //   MessageBox.Show("salut "+ compteurImage.ToString());
                        int index_click = MyTiledMap.Children.IndexOf(e.OriginalSource as UIElement);
                        myimages[index_click].Opacity = 1;
                    };




                    compteurImage++;
                    


               }
            }
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
                    tab_map[i, j] = 0; //si tu mets 1,2,3,ou 4, tu modifies la couleur de base du tableau.
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
                    return new SolidColorBrush(Colors.White);

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

        /// <summary>
        /// La fonction gère le clique sur canvas test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_Test_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /* if (e.OriginalSource is Rectangle)
             {
                 MessageBox.Show("You've touched n°" + Canvas_Test.Children.IndexOf(e.OriginalSource as UIElement));
             } */

            //  MessageBox.Show("AYA " + List_ImageTile[5].source.ToString());
           


        }

        /// <summary>
        /// La fonction gère le clique sur canvas my map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
               // MessageBox.Show("You've touched n°" + MyMap.Children.IndexOf(e.OriginalSource as UIElement));
            }
        }

        /// <summary>
        /// La fonction qui gère le clique sur mon tilemap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTiledMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           // MessageBox.Show("You've touched n°" + MyTiledMap.Children.IndexOf(e.OriginalSource as UIElement));
            // int index = MyTiledMap.Children.IndexOf(e.OriginalSource as UIElement); 

            //permet de récupérer à la fois la valeur qu'on clique dans le tileset, mais aussi l'image correspondante.
            int index_click = MyTiledMap.Children.IndexOf(e.OriginalSource as UIElement);
             memory = List_ImageTile[index_click].source;
            memoire = List_ImageTile[index_click].nombre;



        }

        private void MyTiledMap_MouseEnter(object sender, MouseEventArgs e)
        {
            



        }
        //ce qui se passe quand on clique sur le bouton chargement
        private void Click_Charger(object sender, RoutedEventArgs e)
        {
            List<Tile> ListMap = new List<Tile>();
            //je crée une liste de "tile" (classe que j'ai défini moi même)

           

            //le script à utiliser pour le xml. Si tu veux de la doc: https://tlevesque.developpez.com/dotnet/xml-serialization/#LI-A-3

            XmlSerializer xs = new XmlSerializer(typeof(List<Tile>)); //il faut spécifier à l'avance ce que tu lui envoies. Ici, je lui dis que je lui envoie
                                                                      //une liste de tile.

            using (StreamReader rd = new StreamReader("Map.xml")) //on spécifie qu'on veut écrire (streamwriter), et le nom du fichier
            {
               ListMap = xs.Deserialize(rd) as List<Tile>; //on lui donne streamwriter en paramètre, et l'objet liste.
            } 

            //ça récupère bien les données xml,et ça les met dans une liste
            int k;
            //on met le tableau à jour
            for ( k = 0; k < ListMap.Count; k++) {

                int i = ListMap[k].i;
                int j = ListMap[k].j;
                int value = ListMap[k].value;
                tab_map[i, j] = value;
            }
            //là, il faudrait que cette fonction mette à jour les images de la map.
            tab_update();


        }
        /// <summary>
        /// La fonction devrait mettre à jour les images de la map
        /// </summary>
        public void tab_update()
        {
            
            int k = 0;
            for (int i = 0; i < tab_map.GetLength(0); i++)
            {
                for (int j = 0; j < tab_map.GetLength(1); j++)
                { //c'est le code qu'il faut faire marcher. On parcourt le tableau tilemap. Dans chacune des images du tableau, on les met à jour avec les images correspondantes.
                    List_ImageMap[k].Source = List_ImageTile[tab_map[i, j]].source;
                       k++;
                    ///<example>
                    ///Dans la case 0 0, on a comme valeur "53". Cela signifie que je veux afficher en 0 0, l'image 53 du tilset.
                    /// Ces images existent dans list_imagetile. List_ImageTile[53].source est censé renvoyer l'image 53 du tileset (normalement c'est bon,
                    /// c'est le fait de donner ça à la liste des images map qui foire
                    /// </example>
                    
                }
            }
           

              

        }
    }
}
