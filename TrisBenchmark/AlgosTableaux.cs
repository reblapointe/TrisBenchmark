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
                t[i] = rand.Next(-100, 100);// int.MinValue, int.MaxValue);
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
            if (gauche < droite)
            {
                int pivot = Partition(t, gauche, droite);
                if (pivot > 1)
                    TriRapide(t, gauche, pivot - 1);
                if (pivot + 1 < droite)
                    TriRapide(t, pivot + 1, droite);
            }
            return t;
        }

        private static int Partition<T>(T[] t, int gauche, int droite) where T : IComparable
        {
            T pivot = t[gauche];
            while (true)
            {
                while (t[gauche].CompareTo(pivot) < 0)
                    gauche++;

                while (t[droite].CompareTo(pivot) > 0)
                    droite--;

                if (gauche < droite)
                {
                    if (t[gauche].CompareTo(t[droite]) == 0)
                        return droite;
                    T temp = t[gauche];
                    t[gauche] = t[droite];
                    t[droite] = temp;
                }
                else
                {
                    return droite;
                }
            }
        }

        public static T[] TriFusion<T>(T[] t) where T : IComparable
        {
            triFusion(CopierTableau(t), t, 0, t.Length);
            return t;
            //return TriFusion(t, 0, t.Length - 1);
        }

        // début inclusif
        // milieu à droite
        // fin exclufif
        static void fusionner<T>(T[] entree, T[] sortie, int debut, int milieu, int fin) where T : IComparable
        {
            int a = debut;
            int b = milieu;
            for (int i = debut; i < fin; i++)
            {
               // Console.WriteLine($"a = {a}.   b = {b}.   i = {i}.   debut = {debut}.   milieu = {milieu}.   fin = {fin}");
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

        // debut inclusif, fin exclusif
        static void triFusion<T>(T[] entree, T[] sortie, int debut, int fin) where T : IComparable
        {
            if (fin - debut < 2)
                return;
            int milieu = (debut + fin) / 2;

            triFusion(sortie, entree, debut, milieu);
            triFusion(sortie, entree, milieu, fin);
            fusionner(entree, sortie, debut, milieu, fin);
        }

        /*private static T[] TriFusion<T>(T[] t, int gauche, int droite) where T : IComparable
        {
            if (gauche < droite)
            {
                int milieu = gauche + (droite - gauche) / 2;
                TriFusion(t, gauche, milieu);
                TriFusion(t, milieu + 1, droite);
                Fusionner(t, gauche, milieu, droite);
            }
            return t;
        }

        private static void Fusionner<T>(T[] t, int gauche, int milieu, int droite) where T : IComparable
        {
            var longueurGauche = milieu - gauche + 1;
            var longueurDroite = droite - milieu;
            var gaucheTemp = new T[longueurGauche];
            var droiteTemp = new T[longueurDroite];
            int i, j;
            for (i = 0; i < longueurGauche; ++i)
                gaucheTemp[i] = t[gauche + i];
            for (j = 0; j < longueurDroite; ++j)
                droiteTemp[j] = t[milieu + 1 + j];
            i = 0;
            j = 0;
            int k = gauche;
            while (i < longueurGauche && j < longueurDroite)
                if (gaucheTemp[i].CompareTo(droiteTemp[j]) <= 0)
                    t[k++] = gaucheTemp[i++];
                else
                    t[k++] = droiteTemp[j++];
            while (i < longueurGauche)
                t[k++] = gaucheTemp[i++];
            while (j < longueurDroite)
                t[k++] = droiteTemp[j++];
        }*/
    }
}
