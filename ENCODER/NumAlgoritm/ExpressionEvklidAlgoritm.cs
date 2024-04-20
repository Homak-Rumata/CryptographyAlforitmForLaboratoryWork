using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ENCODER.NumAlgoritm
{
    internal static class ExpressionEvklidAlgoritm
    {
        public static IEnumerable<(int i, int a,  int q, int n, int y2, int y1)> GetNum (int a, int n)
        {
            
            int i = 0;
            int q = n / a;
            int y2 = 0;
            int y1 = 1;
            while (a != 0)
            {
                yield return (i, a, q, n, y2, y1);
                i++;
                (y1, y2) = (y2 - (q * y1), y1);
                (n, a) = (a, n % a);
                q = n / (a > 0 ? a:1) ;
            }
                yield return (i, a, q, n, y2, y1);
            yield break;

        }


        public static int? GetReverse (int a, int n)
        {
            if (a==0 || n == 0)
            {
                return 0;
            }
            int? counter = 0;
            foreach (int temp in GetNum(a, n).Select(item => item.Item5))
                counter = temp;
            return counter>0?counter:counter+n;
        }

        
            

        
        

    }

    public struct SpecialInt
    {
        public int? GetNum { get { return _a; } }

        private int _n;
        private int? _a;
        

        public SpecialInt(int? a, int n)
        {
            this._a = a;
            this._n = n;
        }

        static public (SpecialInt, SpecialInt) GetSpecialInt ((int, int) values, int n)
        {
            return (new SpecialInt(values.Item1, n), new SpecialInt(values.Item2, n));
        }

        public static SpecialInt operator + (SpecialInt x, SpecialInt y)
        {
            return new SpecialInt((x._a + y._a)%x._n, x._n);
        }

        public static SpecialInt operator +(SpecialInt x, int y)
        {
            return new SpecialInt((x._a + y) % x._n, x._n);
        }

        public static SpecialInt operator + (int x, SpecialInt y)
        {
            return y + x;
        }

        public static SpecialInt operator * (SpecialInt x, SpecialInt y)
        {
            return new SpecialInt((x._a * y._a) % x._n, x._n);
        }

        public static SpecialInt operator / (SpecialInt x, SpecialInt y)
        {
            return new SpecialInt((x._a * ExpressionEvklidAlgoritm.GetReverse(y._a.Value ,x._n)) % x._n, x._n);
        }

        public static SpecialInt operator - (SpecialInt x, SpecialInt y)
        {
            return new SpecialInt( (x._a-y._a>=0?x._a-y._a: x._n+(x._a - y._a)) % x._n, x._n);
        }

        public SpecialInt Reverse
        {
            get
            {
                return new SpecialInt(ExpressionEvklidAlgoritm.GetReverse(this._a.Value, this._n), this._n);
            }
        }
    }
}
