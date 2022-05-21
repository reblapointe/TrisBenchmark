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
                        $"[{string.Format("{0:N0}",t.Result[0])}, ... , {string.Format("{0:N0}", t.Result[^1])}] " +
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

        static async Task Main(string[] args)
        {
            int[] t = AlgosTableaux.GenererTableau(1_000_000);

            // Array.Sort() : This method uses the introspective sort (introsort) algorithm as follows:
            //    If the partition size is less than or equal to 16 elements, it uses an insertion sort algorithm.
            //    If the number of partitions exceeds 2 * LogN, where N is the range of the input array, it uses a Heapsort algorithm.
            //    Otherwise, it uses a Quicksort algorithm.
            // https://docs.microsoft.com/en-us/dotnet/api/system.array.sort?redirectedfrom=MSDN&view=net-6.0#System_Array_Sort_System_Array_

            
            await DemarrerTri(AlgosTableaux.CopierTableau(t), s => {Array.Sort(s); return s;}, "Array.Sort");
            await DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriTas);
            await DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriRapide);
            await DemarrerTri(AlgosTableaux.CopierTableau(t), AlgosTableaux.TriFusion);
            
            Console.WriteLine("Fin des tris.");
            
            Console.WriteLine("Début du Benchmark");
            BenchmarkRunner.Run<BenchmarkTris>();

            Console.ReadKey();
        }
    }
}
