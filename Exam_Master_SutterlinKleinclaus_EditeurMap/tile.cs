using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_Master_SutterlinKleinclaus_EditeurMap
{ /// <summary>
/// La classe tile, composée d'une coordonnée i,j, et d'une valeur. Cela me permet de créer la liste à la fin. Obligatoire de la déclarer publique.
/// Possible de la modifier pour le xml (voir la doc)
/// </summary>
    public class Tile
    {
        /// <summary>
        /// attributs de la classe tile, qui servira à être stockée dans le xml
        /// </summary>
        int _i;
        int _j;
        int _value;

        /// <summary>
        /// accesseurs i
        /// </summary>

        public int i
        {
            get { return _i; }
            set { _i = value; }
        }

        /// <summary>
        /// accesseurs j
        /// </summary>

        public int j
        {
            get { return _j; }
            set { _j = value; }
        }

        /// <summary>
        /// accesseurs value
        /// </summary>

        public int value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Constructeur Tile
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="mj"></param>
        /// <param name="mvalue"></param>
        public Tile(int mi, int mj, int mvalue)
        {
            this.i = mi;
            this.j = mj;
            this.value = mvalue;
        }
        /// <summary>
        /// Obligé de créer un constructeur par défaut, sinon la sérialisation plante.
        /// </summary>
        public Tile() 
        {

        }

        
    }
}
