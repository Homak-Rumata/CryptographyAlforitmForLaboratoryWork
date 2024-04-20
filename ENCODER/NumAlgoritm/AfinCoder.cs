using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ENCODER.NumAlgoritm
{
    internal static class AfinCoder
    {
        public static Func<char, char> Shifr(int a, int b)
        {
            //E(x) = (ax+b) mod n
            Func<char, char> function = (temp) => MethodShifr(temp, a, b);


            return function;
        }

        static char MethodShifr(char value , int a, int b)
        {
            const int startvalue = 1040;
            if (((int)value >= 1040 & (int)value <= 1072) || (int)value == 1025)
            {
                int place = (int)value < 1046 && (int)value >= 1040 ? ((int)value - startvalue) : ((int)value == 1025 ? 6 : ((int)value + 1 - startvalue));
                int NewValue = ((place * a + b) % 33 + startvalue);
                return (char)(NewValue <= (5 + startvalue) ? NewValue : NewValue == (6 + startvalue) ? 1025 : NewValue - 1);
            }
            else return value;
        }

        public static IEnumerable<Func<char,char>> ShifrPolyalf((int, int) a, (int, int) b)
        {
            while (true)
            {
                yield return (temp) => MethodShifr(temp, a.Item1, b.Item2);
                (a.Item1, a.Item2) = (a.Item2, (a.Item2 * a.Item1) % 33);
                (b.Item1, b.Item2) = (b.Item2, (b.Item2 * b.Item1) % 33);
            }
            yield break;
        }

        private static char MethodUnshifr(char value, int b, int? reverseA)
        {
            const int startvalue = 1040;

            if (((int)value >= 1040 & (int)value <= 1072) || (int)value == 1025)
            {
                int place = (int)value < 1046 && (int)value >= 1040 ? ((int)value - startvalue) : ((int)value == 1025 ? 6 : ((int)value + 1 - startvalue));
                int? NewValue = (((place - b) * reverseA) % 33);
                NewValue = NewValue >= 0 ? NewValue : NewValue + 33;
                NewValue += startvalue;
                return (char)(NewValue <= (5 + startvalue) ? NewValue : NewValue == (6 + startvalue) ? 1025 : NewValue - 1);
            }
            else return value;
        }

        public static Func<char, char> UnShifr (int a, int b)
        {
            int? reverseA = ExpressionEvklidAlgoritm.GetReverse(a, 33);
            
            return (temp)=>MethodUnshifr(temp, b, reverseA);
        }

        public static IEnumerable<Func<char, char>> UnShifrPolyalf((int, int) a, (int, int) b)
        {
            while (true)
            {
                yield return (temp) => MethodUnshifr(temp, b.Item1, ExpressionEvklidAlgoritm.GetReverse(a.Item1, 33));
                (a.Item1, a.Item2) = (a.Item2, (a.Item2 * a.Item1)%33);
                (b.Item1, b.Item2) = (b.Item2, (b.Item2 * b.Item1) % 33);
            }
            yield break;
        }




    }

    internal static class AfinCoderExperimental
    {
        public static Func<char, char> Shifr(int a, int b)
        {
            //E(x) = (ax+b) mod n
            Func<char, char> function = (temp) => MethodShifr(temp, new SpecialInt(a, 33), new SpecialInt(b, 33));


            return function;
        }

        static char MethodShifr(char value, SpecialInt a, SpecialInt b)
        {
            const int startvalue = 1040;
            if (((int)value >= 1040 & (int)value <= 1072) || (int)value == 1025)
            {
                SpecialInt place = new SpecialInt((int)value < 1046 && (int)value >= 1040 ?
                    ((int)value - startvalue) :
                    ((int)value == 1025 ? 6 : 
                    ((int)value + 1 - startvalue)),33);
                
                int? NewValue = ((place * a + b).GetNum  + startvalue);
                return (char)(NewValue <= (5 + startvalue) ? NewValue :
                    NewValue == (6 + startvalue) ? 1025 :
                    NewValue - 1);
            }
            else return value;
        }

        public static IEnumerable<Func<char, char>> ShifrPolyalf((int, int) ValueA, (int, int) ValueB)
        {
            (SpecialInt, SpecialInt) a = SpecialInt.GetSpecialInt(ValueA, 33);
            (SpecialInt, SpecialInt) b = SpecialInt.GetSpecialInt(ValueB, 33);
            while (true)
            {
                yield return (temp) => MethodShifr(temp, a.Item1, b.Item2);
                (a.Item1, a.Item2) = (a.Item2, (a.Item2 * a.Item1));
                (b.Item1, b.Item2) = (b.Item2, (b.Item2 * b.Item1));
            }
            yield break;
        }

        private static char MethodUnshifr(char value, SpecialInt a, SpecialInt b)
        {
            const int startvalue = 1040;

            if (((int)value >= 1040 & (int)value <= 1072) || (int)value == 1025)
            {
                SpecialInt place = new SpecialInt ((int)value < 1046 && (int)value >= 1040 ? 
                    ((int)value - startvalue) : 
                    ((int)value == 1025 ?
                    6 :
                    ((int)value + 1 - startvalue)), 33);

                int? NewValue = ((place - b) / a).GetNum;
                NewValue = NewValue >= 0 ? NewValue : NewValue + 33;
                NewValue += startvalue;
                return (char)(NewValue <= (5 + startvalue) ? NewValue : NewValue == (6 + startvalue) ? 1025 : NewValue - 1);
            }
            else return value;
        }

        public static Func<char, char> Unshifr(int a, int b)
        {

            return (temp) => MethodUnshifr(temp, new SpecialInt(a, 33), new SpecialInt (b, 33));
        }

        public static IEnumerable<Func<char, char>> UnShifrPolyalf((int, int) ValueA, (int, int) ValueB)
        {
            (SpecialInt, SpecialInt) a = SpecialInt.GetSpecialInt(ValueA, 33);
            (SpecialInt, SpecialInt) b = SpecialInt.GetSpecialInt(ValueB, 33);
            while (true)
            {
                yield return (temp) => MethodUnshifr(temp, b.Item1,a.Item1);
                (a.Item1, a.Item2) = (a.Item2, (a.Item2 * a.Item1));
                (b.Item1, b.Item2) = (b.Item2, (b.Item2 * b.Item1));
            }
            yield break;
        }
    }
}
