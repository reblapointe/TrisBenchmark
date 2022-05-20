using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace TrisBenchmark
{
    class Program
    {
        static Task DemarrerTri(int[] t, Func<int[], int[]> tri)
        {
            return DemarrerTri(t, tri, tri.Method.Name);
        }

        static Task DemarrerTri(int[] t, Func<int[], int[]> tri, string nom)
        {
            Stopwatch sw = new Stopwatch();

            Task<int[]> tacheTri = new Task<int[]>(() => tri(t));
            Task tacheImpression = tacheTri.ContinueWith((t) =>
            {
                Console.WriteLine($"{nom} terminé sur {t.Result.Length} entiers. ({sw.ElapsedMilliseconds} ms)");
                AlgosTableaux.ImprimerTableau(t.Result);
            });

            // Permet d'afficher les exceptions s'il y a lieu
            tacheTri.ContinueWith((t) => Console.Error.WriteLine(t.Exception), TaskContinuationOptions.OnlyOnFaulted);

            // Démarre le Thread
            sw.Start();
            tacheTri.Start();
            return tacheImpression;
        }

        static void Main(string[] args)
        {
            int[] t = AlgosTableaux.GenererTableau(30);
// t = AlgosTableaux.TriFusion(AlgosTableaux.CopierTableau(t));
  //          AlgosTableaux.ImprimerTableau(t);
            Task[] tasks = new Task[] {
            //    DemarrerTri(AlgosTableaux.CopierTableau(t), s => {Array.Sort(s); return s;}, "Array.Sort"),
            //    DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriInsertion),
            //    DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriSelection),
            //    DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriABulle),
            //    DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriRapide),
                DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriFusion),
            };

            Task.WaitAll(tasks);
            Console.WriteLine("Fin des tris. Début du Benchmark");

            Console.ReadKey();
            // BenchmarkRunner.Run<BenchmarkTris>();
        }
    }
}
