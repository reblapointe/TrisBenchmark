using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrisBenchmark
{
    public class BenchmarkTris
    {

        public const int TAILLE = 300_000;

        public static int[] Tableau { get; set; }


        [Benchmark]
        public void TriABulle() => BenchMarkTri(AlgosTableaux.TriABulle);

        [Benchmark]
        public void TriInsertion() => BenchMarkTri(AlgosTableaux.TriInsertion);

        [Benchmark]
        public void TriSelection() => BenchMarkTri(AlgosTableaux.TriSelection);

        [Benchmark]
        public void TriRapide() => BenchMarkTri(AlgosTableaux.TriRapide);

        [Benchmark]
        public void TriFusion() => BenchMarkTri(AlgosTableaux.TriFusion);

        [Benchmark]
        public void TriTas() => BenchMarkTri(AlgosTableaux.TriTas);

        [Benchmark(Baseline = true)]
        public void ArraySort() => BenchMarkTri(s => { Array.Sort(s); return s; });

        private void BenchMarkTri(Func<int[], int[]> tri)
        {
            if (Tableau == null)
                Tableau = AlgosTableaux.GenererTableau(TAILLE);

            tri.Invoke(AlgosTableaux.CopierTableau(Tableau));
        }
    }
}
