using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace TrisBenchmark
{
    class Program
    {
        private static readonly object verrouPrint = new object();

        static Task DemarrerTri(int[] t, Func<int[], int[]> tri)
        {
            return DemarrerTri(t, tri, tri.Method.Name);
        }

        static Task DemarrerTri(int[] t, Func<int[], int[]> tri, string nom)
        {
            Stopwatch sw = new Stopwatch();

            Task<int[]> tacheTri = new Task<int[]>(() => tri(t));
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

        static string ReduireMS(long ms)
        {
            if (ms < 1000)
                return ms + "ms";
            double reste = ms / 1000.0;
            if (reste < 60)
                return string.Format("{0:0.#}", reste) + "s";
            reste /= 60;
            if (reste < 60)
                return string.Format("{0:0.#}", reste) + "m";
            reste /= 60;
            return string.Format("{0:0.#}", reste) + "h";
        }

        static void Main(string[] args)
        {
            int[] t = AlgosTableaux.GenererTableau(100_000);

            Task[] tasks = new Task[] {
                DemarrerTri(AlgosTableaux.CopierTableau(t), s => {Array.Sort(s); return s;}, "Array.Sort"),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriInsertion),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriSelection),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriABulle),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriRapide),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriFusion),
            };

            Task.WaitAll(tasks);
            Console.WriteLine("Fin des tris. Début du Benchmark");
            // BenchmarkRunner.Run<BenchmarkTris>();

            Console.ReadKey();
        }
    }
}
