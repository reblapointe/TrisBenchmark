using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace TrisBenchmark
{
    class Program
    {
        private static readonly object verrouPrint = new ();

        /// <summary>
        /// Démarre un tri sur un tableau
        /// </summary>
        /// <param name="t">tableau</param>
        /// <param name="tri">algorithme de tri</param>
        /// <returns>Tâche de tri</returns>
        static Task DemarrerTri(int[] t, Func<int[], int[]> tri)
        {
            return DemarrerTri(t, tri, tri.Method.Name);
        }

        /// <summary>
        /// Démarre un tri sur un tableau et affiche le temps écoulé à la fin du tri
        /// </summary>
        /// <param name="t">tableau</param>
        /// <param name="tri">algorithme de tri</param>
        /// <param name="nom">Nom de l'algorithme de tri</param>
        /// <returns>Tâche de tri</returns>
        static Task DemarrerTri(int[] t, Func<int[], int[]> tri, string nom)
        {
            Stopwatch sw = new();

            Task<int[]> tacheTri = new(() => tri(t));
            Task tacheImpression = tacheTri.ContinueWith(t =>
            {
                lock (verrouPrint)
                {
                    Console.WriteLine($"{nom,15} terminé sur {string.Format("{0:N0}",t.Result.Length)} entiers. " +
                        $"[{string.Format("{0:N0}",t.Result[0])}, ..., {string.Format("{0:N0}", t.Result[^1])}] " +
                        $"({ReduireMS(sw.ElapsedMilliseconds)})");
                    // AlgosTableaux.ImprimerTableau(t.Result);
                }
            });

            // Permet d'afficher les exceptions s'il y a lieu
            tacheTri.ContinueWith((t) => Console.Error.WriteLine(t.Exception), TaskContinuationOptions.OnlyOnFaulted);

            // Démarre le Thread
            sw.Start();
            tacheTri.Start();
            return tacheImpression;
        }

        /// <summary>
        /// Formatte le nombre de millisecondes
        /// </summary>
        /// <param name="t">temps en millisecondes</param>
        /// <returns>Une chaine de forme hh:mm:ss.fff</returns>
        static string ReduireMS(long t)
        {
            long ms = t % 1000;
            long s = t / 1000 % 60;
            long m = t / (1000 * 60) % 60;
            long h = t / (1000 * 60 * 60);
            return $"{h:00}:{m:00}:{s:00}.{ms:000}"; // hh:mm:ss.fff
        }

        /// <summary>
        /// Démarre les tris sur un tableau de 100_000 entiers
        /// </summary>
        static void Main(string[] _)
        {
            int[] t = AlgosTableaux.GenererTableau(100_000);

            Console.WriteLine("Début des tris.\nTemps d'exécution en format hh:mm:ss.fff\n\n");
            Task[] tasks = new Task[] {
                DemarrerTri(AlgosTableaux.CopierTableau(t), s => {Array.Sort(s); return s;}, "Array.Sort"),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriInsertion),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriSelection),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriABulle),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriRapide),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriFusion),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriTas),
            };

            Task.WaitAll(tasks);
            Console.WriteLine("\nFin des tris.");
            Console.WriteLine("Début du Benchmark");
            BenchmarkRunner.Run<BenchmarkTris>();

            Console.ReadKey();
        }
    }
}
