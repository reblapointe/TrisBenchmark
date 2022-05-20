using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrisBenchmark
{
    public class AlgosTableaux
    {
        private static readonly Random rand = new Random();

        public static int[] GenererTableau(int taille)
        {
            Console.WriteLine("GÉNÉRATION D'UN TABLEAU");
            int[] t = new int[taille];
            for (int i = 0; i < t.Length; i++)
                t[i] = rand.Next(int.MinValue, int.MaxValue);
            return t;
        }

        public static T[] CopierTableau<T>(T[] t)
        {
            T[] c = new T[t.Length];
            for (int i = 0; i < t.Length; i++)
                c[i] = t[i];
            return c;
        }

        public static void ImprimerTableau<T>(T[] t)
        {
            for (int i = 0; i < t.Length; i++)
                Console.Write(t[i] + (i < t.Length - 1 ? ", " : "."));
            Console.WriteLine();
        }

        public static T[] TriABulle<T>(T[] t) where T : IComparable
        {
            for (int i = 0; i < t.Length - 1; i++)
            {
                for (int j = 0; j < t.Length - 1; j++)
                {
                    if (t[j].CompareTo(t[j + 1]) > 0)
                    {
                        T temp = t[j];
                        t[j] = t[j + 1];
                        t[j + 1] = temp;
                    }
                }
            }
            return t;
        }

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
                T temp = t[min];
                t[min] = t[i];
                t[i] = temp;
            }
            return t;
        }

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

        public static T[] TriRapide<T>(T[] t) where T : IComparable
        {
            return TriRapide(t, 0, t.Length - 1);
        }

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
                    T temp = t[i];
                    t[i] = t[j];
                    t[j] = temp;
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

        public static T[] TriFusion<T>(T[] t) where T : IComparable
        {
            TriFusion(CopierTableau(t), t, 0, t.Length);
            return t;
        }

        // debut inclusif, fin exclusif
        static void TriFusion<T>(T[] entree, T[] sortie, int debut, int fin) where T : IComparable
        {
            if (fin - debut < 2)
                return;
            int milieu = (debut + fin) / 2;

            TriFusion(sortie, entree, debut, milieu);
            TriFusion(sortie, entree, milieu, fin);
            Fusionner(entree, sortie, debut, milieu, fin);
        }

        // début inclusif
        // milieu à droite
        // fin exclufif
        static void Fusionner<T>(T[] entree, T[] sortie, int debut, int milieu, int fin) where T : IComparable
        {
            int a = debut;
            int b = milieu;
            for (int i = debut; i < fin; i++)
            {
                if ((a < milieu) && ((b >= fin) || (entree[a].CompareTo(entree[b]) < 0)))
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
    }
}
