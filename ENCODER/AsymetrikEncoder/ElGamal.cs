using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SInt = ENCODER.NumAlgoritm.SpecialInt;

namespace ENCODER.AsymetrikEncoder
{
    class ElGamal
    {
        public ElGamal (int p, int g)
        {
            this.p = p;
            this.g = g;
            Random random = new Random();
            x = random.Next(2, p-1);
            y = FastAlgorithm.GetValue(new SInt(g, p), x).GetNum.Value;
        }

        int y, g, p, x;

        public (int y, int g, int p) GetKey ()
        {
            return (y, g, p);
        }

        public IEnumerable<(int a, int b)> Code (IEnumerable<int> x, int k)
        {
            Func<int, int, (int a, int b)> function = (int M, int k) =>
            { return (FastAlgorithm.GetValue(new SInt(g, p), k).GetNum.Value, (FastAlgorithm.GetValue(new SInt(y, p), k).GetNum.Value * M) % p); };

            return x.Select(i => function(i, k));
        }

        public IEnumerable<int> UnCode(IEnumerable<(int a, int b)> x)
        {
            var function = (int a, int b) =>
            {
                return (b * FastAlgorithm.GetValue(new SInt(a, p), this.x).Reverse.GetNum.Value) % p;
            };

            return x.Select(i => function(i.a, i.b));
        }
    }
}
