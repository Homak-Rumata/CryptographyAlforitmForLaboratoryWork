using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvklidAlgorithm = ENCODER.NumAlgoritm.ExpressionEvklidAlgoritm;
using SInt = ENCODER.NumAlgoritm.SpecialInt;

namespace ENCODER.AsymetrikEncoder
{
    static class Pollada
    {

        static public (int a1, int a2) GetValue (int i)
        {
            Func<SInt, SInt> function = ( SInt n) => ((n*n)+1);

            int seed;

            Random random = new Random();
            seed = random.Next(2, Convert.ToInt32(Math.Sqrt(i)));

            IEnumerable<int> X = GetX(seed, i, function);

            var Expression = (int a, int b, int num) =>
            {
                //Console.WriteLine(a + " - " + b+ " - "+ num);
                return a - b;
            };

            int step = Convert.ToInt32(Math.Sqrt(i));

            var result1 = X.Chunk(step);
            var result2 = result1.Select((item, num) => GetD(Expression(X.ElementAt(step * num / 2) , item.Last(), (i * step / 2)), i));

            foreach (int item in result2)
            {
                if (item > 1)
                {
                    return (item, i / item);
                }
            }

            return (0, 0);
            
        }

        static private IEnumerable<int> GetX (int seed, int i, Func<SInt, SInt> function)
        {
            SInt nextstage = new SInt (seed, i);
            int acc = 0;
            while (true)
            {
                yield return nextstage.GetNum.Value;
                //Console.WriteLine(acc+++ " ^ "+nextstage.GetNum);
                nextstage = function(nextstage);
            }
            yield break;
        }



        static private int GetD (int a, int b)
        {
            if (a==0 || b==0)
                return 0;

            var function = EvklidAlgorithm.GetNum;

            var result = function(a, b);

            return result.Last().n;
        }

    }
}
