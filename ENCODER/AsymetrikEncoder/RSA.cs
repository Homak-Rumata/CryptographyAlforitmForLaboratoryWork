using ENCODER.NumAlgoritm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SInt = ENCODER.NumAlgoritm.SpecialInt;


namespace ENCODER.AsymetrikEncoder
{
    interface IRSA
    {

    }

    class RSA
    {

        public RSA(int P, int Q) 
        {
            N = P * Q;
            Phi = (P - 1) * (Q - 1);

            int n = (new Random()).Next(2, 5);


            var intsize = Int32.MaxValue;
            var temp = (Math.Pow(2, Math.Pow(2, n) % intsize) + 1);

            e = Convert.ToInt32(temp);

            d = (new SInt(e, Phi).Reverse).GetNum.Value;
        }

        private int N, Phi, e, d;

        public (int e, int n) GetKey ()
        {
            return (e, N);
        }

        public IEnumerable<int> Code (IEnumerable<int> sourse)
        {
            var function = (int x) =>
            {
                var temp = FastAlgorithm.GetValue(new SInt(x, N), e).GetNum.Value;
                return Convert.ToInt32(temp);
            };
            
            return sourse.Select(x=>function(x));
        }

        public IEnumerable<int> UnCode(IEnumerable<int> sourse)
        {
            var function = (int x) =>
            {
                var temp = FastAlgorithm.GetValue(new SInt(x, N), d).GetNum.Value;
                return Convert.ToInt32(temp);
            };
            return sourse.Select(x => function(x));
        }
    }
}
