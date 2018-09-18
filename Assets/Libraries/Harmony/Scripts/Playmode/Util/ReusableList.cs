using System.Collections.Generic;

namespace Harmony
{
    public delegate TReturn ReusableListUseFunction<T, out TReturn>(IList<T> list);

    /// <summary>
    /// Implémentation d'une liste réutilisable. Une liste réutilisable est une liste instanciée une seule fois et vidée
    /// après chaque utilisation.
    /// </summary>
    /// <typeparam name="T">Type d'élément dans la liste.</typeparam>
    public class ReusableList<T>
    {
        private IList<T> list;

        /// <summary>
        /// Utilise la liste. 
        /// </summary>
        /// <param name="useFunction">Fonction à appeler durant l'utilisation de la liste.</param>
        /// <typeparam name="TReturn"></typeparam>
        /// <returns>Résultat de la fonction.</returns>
        public TReturn Use<TReturn>(ReusableListUseFunction<T, TReturn> useFunction)
        {
            try
            {
                return useFunction(GetList());
            }
            finally
            {
                ClearList();
            }
        }

        private IList<T> GetList()
        {
            return list ?? (list = new List<T>());
        }

        private void ClearList()
        {
            list?.Clear();
        }
    }
}