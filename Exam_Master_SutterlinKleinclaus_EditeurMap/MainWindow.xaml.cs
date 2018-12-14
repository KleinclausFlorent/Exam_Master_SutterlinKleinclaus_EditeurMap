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
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;

namespace Exam_Master_SutterlinKleinclaus_EditeurMap
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[,] tab_map; //ce tableau représente les données constituant la map sur laquelle on travaille, qu'on peut charger et enregistrer.
        int memoire; //cette variable contient la valeur de la case du tileset qu'on a cliqué.
        List<Image> List_ImageMap = new List<Image>(); //liste des images constituant la carte
        List<Image> Liste_ImgTiledmap = new List<Image>();
        List<Image_tile> List_ImageTile = new List<Image_tile>(); //liste des images constituant le tileset
        CroppedBitmap memory; //contient l'image que j'ai cliquée


        string emptyblock = @"D:\ludus\2018-2019\Exam_1\Exam_Master_SutterlinKleinclaus_EditeurMap\Exam_Master_SutterlinKleinclaus_EditeurMap\Assets\blockvide.png";
        string TilemapFile;
        string MapFile;





        ///<remarks>
        /// Code temporaire a placer dans une classe chargée de dessiner la map dans le canvas avec une méthode init ou draw empty map
        /// Permet de remplir de case vide le canvas en fonction de la taille de case voulu et de la taille de l'objet canvas
        /// Un objet contenant un tableau d'objet Case comme un objet Map permettra de garder en mémoire les cases créées et leurs positions. 
        /// On pourra rajouter d'autres informations dans l'objet case
        /// </remarks>
        public MainWindow()
        {

            InitializeComponent();
            

            Init_Tiledmap();

            Init_Map();
            
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

            //Charge l'image
            BitmapImage myBitmapImage = new BitmapImage();

            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(emptyblock);
            myBitmapImage.EndInit();

            int cmpt_image = 0;

            for (int i = 0; i < CanvasNbLigne; i++)
            {
                for (int j = 0; j < CanvasNbCol; j++)
                {
                    myimages[cmpt_image] = new Image();
                    myimages[cmpt_image].Width = 32;
                    Canvas.SetLeft(myimages[cmpt_image], j * 32 );
                    Canvas.SetTop(myimages[cmpt_image], i * 32 );
                    myimages[cmpt_image].Source = myBitmapImage;
                    Liste_ImgTiledmap.Add(myimages[cmpt_image]); // new line
                    MyTiledMap.Children.Add(myimages[cmpt_image]);
                    cmpt_image++;
                }
            }
        }

        /// <summary>
        ///  Fonction d'init Tilemap
        /// </summary>        
        private void Load_Tiledmap()
        {
                              
             //Charge l'image
            BitmapImage myBitmapImage = new BitmapImage();

            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(TilemapFile);
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
                   
                    Liste_ImgTiledmap[compteurImage].Source = cb;
                    Image_tile ig = new Image_tile();
                    ig.nombre = compteurImage;
                    ig.source = cb;
                    List_ImageTile.Add(ig); //je récupère les "images tiles" crées, constituées d'un nombre, et d'une source pour l'image.
                    imagetest.Source = cb; //je sais plus pourquoi je fais ça, je crois que ça sert à rien



                    Liste_ImgTiledmap[compteurImage].MouseEnter += (s, e) =>
                    {
                        //myimages[compteurImage].Source = memory;
                        //   MessageBox.Show("salut "+ compteurImage.ToString());
                        int index_click = MyTiledMap.Children.IndexOf(e.OriginalSource as UIElement);
                        Liste_ImgTiledmap[index_click].Opacity = 0.5;
                    };

                    Liste_ImgTiledmap[compteurImage].MouseLeave += (s, e) =>
                    {
                        //myimages[compteurImage].Source = memory;
                        //   MessageBox.Show("salut "+ compteurImage.ToString());
                        int index_click = MyTiledMap.Children.IndexOf(e.OriginalSource as UIElement);
                        Liste_ImgTiledmap[index_click].Opacity = 1;
                    };
                    
                    

                    compteurImage++;
                    


               }
            }
        }

        private void Init_Map()
        {

            
            int mTaille_Case = 32;
            int mNombre_Case = Convert.ToInt32(MyMap.Width) / mTaille_Case;
            //int mNombre_Case = 8;
            tab_map = new int[mNombre_Case, mNombre_Case];

            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(emptyblock);
            myBitmapImage.EndInit();

            Image myISave = new Image(); //Enregistre l'image avant le passage de la souris et le repositionne si pas cliquer (PAS FONCTIONNEL )
            bool mybool = false;


            for (int i = 0; i < mNombre_Case; i++)
            {
                for (int j = 0; j < mNombre_Case; j++)
                {

                    Case mCase = new Case(mTaille_Case, mTaille_Case, i * mTaille_Case, j * mTaille_Case, 0);


                    Image img = new Image(); //ça crée une image à chaque case de la grille.
                    img.Height = mTaille_Case;
                    img.Width = mTaille_Case;
                    img.Source = myBitmapImage; //je l'initialise sur une image random de la grille
                    img.Opacity = 1; //je met l'opacité à 0, pour la rendre invisible
                    Canvas.SetLeft(img, mCase.X);
                    Canvas.SetTop(img, mCase.Y);

                    List_ImageMap.Add(img);

                    MyMap.Children.Add(img);

                    tab_map[i, j] = -1;

                    /// <summary>
                    /// Evénement qui gère le clique gauche sur une image de la map
                    /// Remplace la source de l'image visée par l'image en mémoire
                    /// </summary>
                    img.MouseLeftButtonDown += (s, e) =>
                    {
                        // MessageBox.Show("You've touched n°" + MyMap.Children.IndexOf(e.OriginalSource as UIElement));
                        img.Source = memory;
                        img.Opacity = 1;

                        int k = mCase.X / mTaille_Case;
                        int l = mCase.Y / mTaille_Case;
                        tab_map[k, l] = memoire;
                        mybool = true;


                    };
                    /// <summary>
                    /// Evénement qui gère le clique droit sur une image de la map
                    /// Remplace la source de l'image visée par l'image en mémoire
                    /// </summary>
                    img.MouseRightButtonDown += (s, e) =>
                    {
                        // MessageBox.Show("You've touched n°" + MyMap.Children.IndexOf(e.OriginalSource as UIElement));
                        img.Source = myBitmapImage;
                        img.Opacity = 1;

                        int k = mCase.X / mTaille_Case;
                        int l = mCase.Y / mTaille_Case;
                        tab_map[k, l] = -1;
                        mybool = true;


                    };

                    /// <summary>
                    /// Evénement qui gère le passage de la souris sur une image de la map
                    /// Change l'opacité de l'image et remplace temporairement la source par celle en mémoire
                    /// </summary>
                    img.MouseEnter += (s, e) =>
                    {
                        img.Opacity = 0.5;
                        // Affichage temporaire de la case (PAS FONCTIONNEL)
                        if (memory != null)
                        {
                            myISave.Source = img.Source;
                            img.Source = memory;
                        }
                        
                    };

                    /// <summary>
                    /// Evénement qui gère le sortie de la souris d'une image de la map
                    /// Remet l'opacité à 1 et remet l'image si elle n'a pas été modifiée
                    /// </summary>
                    img.MouseLeave += (s, e) =>
                    {
                        img.Opacity = 1;
                        // Retour à la case d'origine (PAS FONCTIONNEL
                        if ((myISave.Source != null) && (mybool != true))
                        {
                            img.Source = myISave.Source;
                            mybool = false;
                        } else
                        {
                            mybool = false;
                        }
                        
                    };


                }
            }

        }
       

        

        /// <summary>
        /// La fonction qui gère le clique sur mon tilemap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTiledMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //permet de récupérer à la fois la valeur qu'on clique dans le tileset, mais aussi l'image correspondante.
            int index_click = MyTiledMap.Children.IndexOf(e.OriginalSource as UIElement);
            memory = List_ImageTile[index_click].source;
            memoire = List_ImageTile[index_click].nombre;

        }
        
        

        private void LoadTiledMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
               
        }

        /// <summary>
        /// La fonction qui s'active quand on appuie sur le bouton sauvegarder. Permet de générer un fichier xml.
        /// </summary>
        /// <param name="sender">l'objet appuyé</param>
        /// <param name="e">permet de récupérer des paramètres spécifiques (je m'en sers pas anyway)</param>
        private void Save_Map_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Filepath = System.IO.Path.GetDirectoryName(saveFileDialog.FileName) + "\\";
                Filepath += System.IO.Path.GetFileName(saveFileDialog.FileName);

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

                using (StreamWriter wr = new StreamWriter(Filepath)) //on spécifie qu'on veut écrire (streamwriter), et le nom du fichier
                {
                    xs.Serialize(wr, ListMap); //on lui donne streamwriter en paramètre, et l'objet liste.
                }
            }
        }

        /// <summary>
        /// La fonction qui s'active quand on appuie sur le bouton charger une map. Permet de mettre à jour le tableau liste map qui nous permet d'afficher la map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Charger_Map_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                MapFile = System.IO.Path.GetDirectoryName(openFileDialog.FileName) + "\\";
                MapFile += System.IO.Path.GetFileName(openFileDialog.FileName);

                List<Tile> ListMap = new List<Tile>();
                //je crée une liste de "tile" (classe que j'ai défini moi même)



                //le script à utiliser pour le xml. Si tu veux de la doc: https://tlevesque.developpez.com/dotnet/xml-serialization/#LI-A-3

                XmlSerializer xs = new XmlSerializer(typeof(List<Tile>)); //il faut spécifier à l'avance ce que tu lui envoies. Ici, je lui dis que je lui envoie
                                                                          //une liste de tile.

                using (StreamReader rd = new StreamReader(MapFile)) //on spécifie qu'on veut écrire (streamwriter), et le nom du fichier
                {
                    ListMap = xs.Deserialize(rd) as List<Tile>; //on lui donne streamwriter en paramètre, et l'objet liste.
                }

                //ça récupère bien les données xml,et ça les met dans une liste
                int k;
                //on met le tableau à jour
                for (k = 0; k < ListMap.Count; k++)
                {

                    int i = ListMap[k].i;
                    int j = ListMap[k].j;
                    int value = ListMap[k].value;
                    tab_map[i, j] = value;
                }
                //là, il faudrait que cette fonction mette à jour les images de la map.
                tab_update();
            }


        }
       
        /// <summary>
        /// La fonction met à jour les images de la map
        /// </summary>
        public void tab_update()
        {
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(emptyblock);
            myBitmapImage.EndInit();
            int k = 0;
            for (int i = 0; i < tab_map.GetLength(0); i++)
            {
                for (int j = 0; j < tab_map.GetLength(1); j++)
                { //c'est le code qu'il faut faire marcher. On parcourt le tableau tilemap. Dans chacune des images du tableau, on les met à jour avec les images correspondantes.
                    if (tab_map[i, j] == -1)
                    {
                        List_ImageMap[k].Source = myBitmapImage;
                        k++;
                    }
                    else
                    {
                        List_ImageMap[k].Source = List_ImageTile[tab_map[i, j]].source;
                        k++;
                    }

                    ///<example>
                    ///Dans la case 0 0, on a comme valeur "53". Cela signifie que je veux afficher en 0 0, l'image 53 du tilset.
                    /// Ces images existent dans list_imagetile. List_ImageTile[53].source est censé renvoyer l'image 53 du tileset (normalement c'est bon,
                    /// c'est le fait de donner ça à la liste des images map qui foire
                    /// </example>

                }
            }




        }

        /// <summary>
        /// S'active quand on appuie sur le bouton charger TileMap. Permet de charger un fichier .png correspondant au tileset à utiliser
        /// Appel la fonction Load Tilemap qui remplace les sources des images du tileset avec celle du png
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Charger_TileMap_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TilemapFile = System.IO.Path.GetDirectoryName(openFileDialog.FileName) + "\\";
                TilemapFile += System.IO.Path.GetFileName(openFileDialog.FileName);
                Load_Tiledmap();
            }
                
                
        }

       
        

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(emptyblock);
            myBitmapImage.EndInit();
            int k = 0;
            for (int i = 0; i < tab_map.GetLength(0); i++)
            {
                for (int j = 0; j < tab_map.GetLength(1); j++)
                { //c'est le code qu'il faut faire marcher. On parcourt le tableau tilemap. Dans chacune des images du tableau, on les met à jour avec les images correspondantes.
                    List_ImageMap[k].Source = myBitmapImage;
                    k++;
                }
            }
        }
    }
}
