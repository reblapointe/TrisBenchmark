using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrisBenchmark
{
    public class AlgosTableaux
    {
        private static readonly Random rand = new();

        /// <summary>
        /// Générer un tableau de longueur taille uniformément aléatoire entre min et max
        /// </summary>
        /// <param name="taille">taille du tableau</param>
        /// <param name="min">borne minimale pour les valeurs à mettre dans le tableau</param>
        /// <param name="max">borne maximale pour les valeurs à mettre dans le tableau</param>
        /// <returns>un tableau d'entiers de taille cases uniformément aléatoires entre min et max</returns>
        public static int[] GenererTableau(int taille, int min = int.MinValue, int max = int.MaxValue)
        {
            int[] t = new int[taille];
            for (int i = 0; i < t.Length; i++)
                t[i] = rand.Next(min, max);
            return t;
        }

        /// <summary>
        /// Fabrique une copie du tableau
        /// </summary>
        /// <typeparam name="T">Type du tableau</typeparam>
        /// <param name="t">tableau</param>
        /// <returns>copie profonde du tableau t</returns>
        public static T[] CopierTableau<T>(T[] t)
        {
            T[] c = new T[t.Length];
            for (int i = 0; i < t.Length; i++)
                c[i] = t[i];
            return c;
        }

        /// <summary>
        /// Imprimer le tableau à la console
        /// </summary>
        /// <typeparam name="T">Type du tableau</typeparam>
        /// <param name="t">tableau à imprimer</param>
        public static void ImprimerTableau<T>(T[] t)
        {
            for (int i = 0; i < t.Length; i++)
                Console.Write(t[i] + (i < t.Length - 1 ? ", " : "."));
            Console.WriteLine();
        }

        /// <summary>
        /// Trie t avec le tri à bulle
        /// </summary>
        /// <typeparam name="T">Type du tableau</typeparam>
        /// <param name="t">tableau à trier</param>
        /// <returns>Le tableau trié</returns>
        public static T[] TriABulle<T>(T[] t) where T : IComparable
        {
            for (int i = 0; i < t.Length - 1; i++)
            {
                for (int j = 0; j < t.Length - 1; j++)
                {
                    if (t[j].CompareTo(t[j + 1]) > 0)
                    {
                        (t[j + 1], t[j]) = (t[j], t[j + 1]);
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// Trie t avec le tri par sélection
        /// </summary>
        /// <typeparam name="T">Type du tableau</typeparam>
        /// <param name="t">tableau à trier</param>
        /// <returns>Le tableau trié</returns>
        public static T[] TriSelection<T>(T[] t) where T : IComparable
        {
            int min;
            for (int i = 0; i < t.Length - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < t.Length; j++)
                {
                    if (t[j].CompareTo(t[min]) < 0)
                    {
                        min = j;
                    }
                }
                (t[i], t[min]) = (t[min], t[i]);
            }
            return t;
        }


        /// <summary>
        /// Trie t avec le tri par insertion
        /// </summary>
        /// <typeparam name="T">Type du tableau</typeparam>
        /// <param name="t">tableau à trier</param>
        /// <returns>Le tableau trié</returns>
        public static T[] TriInsertion<T>(T[] t) where T : IComparable
        {
            for (int i = 1; i < t.Length; i++)
            {
                T v = t[i];
                int j = i - 1;

                while (j >= 0 && v.CompareTo(t[j]) < 0)
                {
                    t[j + 1] = t[j];
                    j--;
                }
                t[j + 1] = v;
            }
            return t;
        }


        /// <summary>
        /// Trie t avec le tri rapide
        /// </summary>
        /// <typeparam name="T">Type du tableau</typeparam>
        /// <param name="t">tableau à trier</param>
        /// <returns>Le tableau trié</returns>
        public static T[] TriRapide<T>(T[] t) where T : IComparable
        {
            return TriRapide(t, 0, t.Length - 1);
        }

        /// <summary>
        /// Tri rapide de t dans l'intervalle gauche - droite
        /// </summary>
        /// <typeparam name="T">Type de t</typeparam>
        /// <param name="t">tableau à trier</param>
        /// <param name="gauche">premier indice du tableau à partir duquel trier</param>
        /// <param name="droite">dernier indice du tableau jusqu'auquel trier</param>
        /// <returns>Le tableau trié</returns>
        private static T[] TriRapide<T>(T[] t, int gauche, int droite) where T : IComparable
        {
            int i = gauche;
            int j = droite;
            T pivot = t[gauche];
            while (i <= j)
            {
                while (t[i].CompareTo(pivot) < 0)
                    i++;
                while (t[j].CompareTo(pivot) > 0)
                    j--;
                if (i <= j)
                {
                    (t[j], t[i]) = (t[i], t[j]);
                    i++;
                    j--;
                }
            }

            if (gauche < j)
                TriRapide(t, gauche, j);
            if (i < droite)
                TriRapide(t, i, droite);
            return t;
        }


        /// <summary>
        /// Trie t avec le tri fusion, stable
        /// </summary>
        /// <typeparam name="T">Type du tableau</typeparam>
        /// <param name="t">tableau à trier</param>
        /// <returns>Le tableau trié</returns>
        public static T[] TriFusion<T>(T[] t) where T : IComparable
        {
            TriFusion(CopierTableau(t), t, 0, t.Length);
            return t;
        }

        /// <summary>
        /// Trie t avec le tri fusion, stable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entree">tableau à trier</param>
        /// <param name="sortie">tableau trié</param>
        /// <param name="debut">indice à partir duquel trier (inclusif)</param>
        /// <param name="fin">indice jusqu'auquel trier (exclusif)</param>
        static void TriFusion<T>(T[] entree, T[] sortie, int debut, int fin) where T : IComparable
        {
            if (fin - debut < 2)
                return;
            int milieu = (debut + fin) / 2;

            /*var g = Task.Run(() => TriFusion(sortie, entree, debut, milieu));
            var d = Task.Run(() => TriFusion(sortie, entree, milieu, fin));

            Task.WaitAll(g, d);*/
            TriFusion(sortie, entree, debut, milieu);
            TriFusion(sortie, entree, milieu, fin);
            Fusionner(entree, sortie, debut, milieu, fin);
        }


        /// <summary>
        /// Fusionne la portion du tableau entre debut et milieu avec la portion entre milieu et fin
        /// </summary>
        /// <typeparam name="T">Type du tableau</typeparam>
        /// <param name="entree">tableau à trier</param>
        /// <param name="sortie">tableau trié</param>
        /// <param name="debut">indice à partir duquel trier (inclusif)</param>
        /// <param name="milieu">Début de la deuxième portion</param>
        /// <param name="fin">indice jusqu'auquel trier (exclusif)</param>
        static void Fusionner<T>(T[] entree, T[] sortie, int debut, int milieu, int fin) where T : IComparable
        {
            int a = debut;
            int b = milieu;
            for (int i = debut; i < fin; i++)
            {
                if ((a < milieu) && ((b >= fin) || (entree[a].CompareTo(entree[b]) <= 0)))
                {
                    sortie[i] = entree[a];
                    a++;
                }
                else
                {
                    sortie[i] = entree[b];
                    b++;
                }
            }
        }

        /// <summary>
        /// Tri par tas
        /// </summary>
        /// <typeparam name="T">Type du tableau</typeparam>
        /// <param name="t">tableau à trier</param>
        /// <returns>tableau trié</returns>
        public static T[] TriTas<T>(T[] t) where T : IComparable
        {
            return TriTas(t, t.Length);
        }

        /// <summary>
        /// Tri par tas
        /// </summary>
        /// <typeparam name="T">Type du tableau</typeparam>
        /// <param name="t">tableau à trier</param>
        /// <param name="n">indice jusq'auquel trier</param>
        /// <returns>tableau trié</returns>
        static T[] TriTas<T>(T[] t, int n) where T : IComparable
        {
            for (int i = n / 2 - 1; i >= 0; i--)
                Tamiser(t, n, i);
            for (int i = n - 1; i >= 0; i--)
            {
                (t[i], t[0]) = (t[0], t[i]);
                Tamiser(t, i, 0);
            }
            return t;
        }

        /// <summary>
        /// Tamise pour le tri par tas
        /// </summary>
        /// <typeparam name="T">Type du tableau</typeparam>
        /// <param name="t">tableau à trier</param>
        /// <param name="n">indice jusq'auquel trier</param>
        /// <param name="i">indice du début du tas</param>
        private static void Tamiser<T>(T[] t, int n, int i) where T : IComparable
        {
            int maximum = i;
            int gauche = 2 * i + 1;
            int droite = 2 * i + 2;
            if (gauche < n && t[gauche].CompareTo(t[maximum]) > 0)
                maximum = gauche;
            if (droite < n && t[droite].CompareTo(t[maximum]) > 0)
                maximum = droite;
            if (maximum != i)
            {
                (t[maximum], t[i]) = (t[i], t[maximum]);
                Tamiser(t, n, maximum);
            }
        }
    }
}
