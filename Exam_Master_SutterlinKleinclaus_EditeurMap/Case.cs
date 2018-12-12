using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_Master_SutterlinKleinclaus_EditeurMap
{
    /// <summary>
    /// Classe Case permet de définir une case
    /// </summary>
    public class Case
    {
        ///<summary>
        ///Attributs de la classe
        /// </summary>
        int _Width;
        int _Height;
        int _X;
        int _Y;

        ///<summary>
        ///Accesseurs Width
        /// </summary>
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        ///<summary>
        ///Accesseurs Height
        /// </summary>
        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        ///<summary>
        ///Accesseurs X
        /// </summary>
        public int X
        {
            get { return _X; }
            set { _X = value; }
        }
        ///<summary>
        ///Accesseurs Y
        /// </summary>
        public int Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        /// <summary>
        /// Constructeur permettant d'initiliser toutes les valeurs
        /// </summary>
        public Case(int mWidth,int mHeight, int mX, int mY)
        {
            _Width = mWidth;
            _Height = mHeight;
            _X = mX;
            _Y = mY;
        }
    }
}
