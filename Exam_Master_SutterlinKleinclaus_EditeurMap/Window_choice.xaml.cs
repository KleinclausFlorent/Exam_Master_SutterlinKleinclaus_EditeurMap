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
using System.Windows.Shapes;

namespace Exam_Master_SutterlinKleinclaus_EditeurMap
{
    /// <summary>
    /// Logique d'interaction pour Window_choice.xaml
    /// </summary>
    public partial class Window_choice : Window
    {
        public int choix_taille;
        public int choix_longueur;
        public int choix_largeur;
       // TextBox Textbox_largeur = new TextBox();
        public Window_choice()
        {
            InitializeComponent();
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            choix_largeur = int.Parse(Textbox_largeur.Text);
            choix_longueur = int.Parse(Textbox_longueur.Text);
            MainWindow winmain = new MainWindow(choix_taille,choix_longueur,choix_largeur);
            winmain.Show();
            this.Close();

        }

        private void Button16_Click(object sender, RoutedEventArgs e)
        {
            choix_taille = 16;
            Text_Affichage.Text = choix_taille.ToString();

        }

        private void Button32_Click(object sender, RoutedEventArgs e)
        {
            choix_taille = 32;
            Text_Affichage.Text = choix_taille.ToString();
        }

        private void Button64_Click(object sender, RoutedEventArgs e)
        {
            choix_taille = 64;
            Text_Affichage.Text = choix_taille.ToString();
        }
    }
}
