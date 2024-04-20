using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SInt = ENCODER.NumAlgoritm.SpecialInt;

namespace ENCODER.AsymetrikEncoder
{

    interface IFastAlgorithm
    {
        static abstract SInt GetValue(SInt n, int i);
    }


    /// <summary>
    /// Утилита для быстрого возведения числа в степень
    /// </summary>
    static class FastAlgorithm
    {

        /// <summary>
        /// Метод быстрого возведения числа в степень
        /// </summary>
        /// <param name="n"></param>
        /// <param name="i"></param>
        /// <returns>
        /// Число возведённое в степень
        /// </returns>
        static public SInt GetValue (SInt n, int i)
        {
            i -=1;
            SInt result = n;
            SInt last = n;
            while (i!=0)
            {
                if (i%2==1)
                {
                    result = result * last;
                }

                i /= 2;
                last = last * last;
            }

            return result;
        }


    }
}
